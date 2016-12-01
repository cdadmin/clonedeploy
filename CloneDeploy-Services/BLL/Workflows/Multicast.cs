using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using CloneDeploy_App.BLL.DynamicClientPartition;
using CloneDeploy_App.Helpers;
using CloneDeploy_Entities;
using CloneDeploy_Services;

namespace CloneDeploy_App.BLL.Workflows
{
    public class Multicast
    {
        private readonly string _clientCount;
        private readonly bool _isOnDemand;
        private readonly ActiveMulticastSessionEntity _multicastSession;
        private List<ComputerEntity> _computers;
        private readonly GroupEntity _group;
        private ImageProfileEntity _imageProfile;
        private readonly int _userId;
        //Constructor For Starting Multicast For Group
        public Multicast(GroupEntity group, int userId)
        {
            _computers = new List<ComputerEntity>();
            _multicastSession = new ActiveMulticastSessionEntity();
            _isOnDemand = false;
            _group = group;
            _userId = userId;

        }

        //Constructor For Starting Multicast For On Demand
        public Multicast(ImageProfileEntity imageProfile, string clientCount, int userId)
        {
            _multicastSession = new ActiveMulticastSessionEntity();
            _isOnDemand = true;
            _imageProfile = imageProfile;
            _clientCount = clientCount;
            _group = new GroupEntity { ImageProfileId = _imageProfile.Id };
            _userId = userId;
            _multicastSession.ImageProfileId = imageProfile.Id;
        }

        public string Create()
        {
            _imageProfile = ImageProfileServices.ReadProfile(_group.ImageProfileId);
            if (_imageProfile == null) return "The Image Profile Does Not Exist";

            if (_imageProfile.Image == null) return "The Image Does Not Exist";

            var validation = ImageServices.CheckApprovalAndChecksum(_imageProfile.Image,_userId);
            if (!validation.Success) return validation.Message;

            _multicastSession.Port = PortServices.GetNextPort();
            if (_multicastSession.Port == 0)
            {
                return "Could Not Determine Current Port Base";
            }

            var dp = DistributionPointServices.GetPrimaryDistributionPoint();
            if (dp == null) return "Could Not Find A Primary Distribution Point";

            _multicastSession.UserId = _userId;
            //End of the road for starting an on demand multicast
            if (_isOnDemand)
            {
                _multicastSession.Name = _group.Name;
                _group.Name =_multicastSession.Port.ToString();
                if (!StartMulticastSender())
                    return "Could Not Start The Multicast Application";
                else
                    return "Successfully Started Multicast " + _group.Name;
            }

            //Continue On If multicast is for a group
            _multicastSession.Name = _group.Name;
            _computers = GroupServices.GetGroupMembers(_group.Id);
            if (_computers.Count < 1)
            {
                return "The group Does Not Have Any Members";
            }

            var activeMulticastSessionServices = new ActiveMulticastSessionServices();
            if (!activeMulticastSessionServices.AddActiveMulticastSession(_multicastSession))
            {
                return "Could Not Create Multicast Database Task.  An Existing Task May Be Running.";
            }

            if (!CreateComputerTasks())
            {
                activeMulticastSessionServices.Delete(_multicastSession.Id);
                return "Could Not Create Computer Database Tasks.  A Computer May Have An Existing Task.";
            }

            if (!CreatePxeFiles())
            {
                activeMulticastSessionServices.Delete(_multicastSession.Id);
                return "Could Not Create Computer Boot Files";
            }

            if (!CreateTaskArguments())
            {
                activeMulticastSessionServices.Delete(_multicastSession.Id);
                return "Could Not Create Computer Task Arguments";
            }

            if (!StartMulticastSender())
            {
                activeMulticastSessionServices.Delete(_multicastSession.Id);
                return "Could Not Start The Multicast Application";
            }

            foreach (var computer in _computers)
                Utility.WakeUp(computer.Mac);

            return "Successfully Started Multicast " + _group.Name;
        }

        private bool CreateComputerTasks()
        {
            var error = false;
            var activeTaskIds = new List<int>();
            var activeImagingTaskServices = new ActiveImagingTaskServices();
            foreach (var computer in _computers)
            {
                if (new ComputerServices().IsComputerActive(computer.Id)) return false;
                var activeTask = new ActiveImagingTaskEntity
                {
                    Type = "multicast",
                    ComputerId = computer.Id,
                    Direction = "push",
                    MulticastId = _multicastSession.Id,
                    UserId = _userId
                    
                };

                if (activeImagingTaskServices.AddActiveImagingTask(activeTask))
                {
                    activeTaskIds.Add(activeTask.Id);
                    computer.ActiveImagingTask = activeTask;
                }
                else
                {
                    error = true;
                    break;
                }
            }
            if (error)
            {
                foreach (var taskId in activeTaskIds)
                    activeImagingTaskServices.DeleteActiveImagingTask(taskId);

                return false;
            }
            return true;
        }

        private bool CreatePxeFiles()
        {
            foreach (var computer in _computers)
            {
                if (!new TaskBootMenu(computer, _imageProfile, "push").CreatePxeBootFiles())
                    return false;
            }
            return true;
        }

        private bool CreateTaskArguments()
        {
            foreach (var computer in _computers)
            {
                computer.ActiveImagingTask.Arguments =
                    new CreateTaskArguments(computer, _imageProfile, "multicast").Run(_multicastSession.Port.ToString());
                if (!new ActiveImagingTaskServices().UpdateActiveImagingTask(computer.ActiveImagingTask))
                    return false;
            }
            return true;
        }

        private string GenerateProcessArguments()
        {
            var isUnix = Environment.OSVersion.ToString().Contains("Unix");

            var schema = new ClientPartitionHelper(_imageProfile).GetImageSchema();

            var schemaCounter = -1;
            var multicastHdCounter = 0;
            string processArguments = null;
            foreach (var hd in schema.HardDrives)
            {
                schemaCounter++;
                if (!hd.Active) continue;
                multicastHdCounter++;

                var imagePath = Settings.PrimaryStoragePath + "images" + Path.DirectorySeparatorChar +
                                _imageProfile.Image.Name + Path.DirectorySeparatorChar + "hd" +
                                schemaCounter;

                var x = 0;
                foreach (var part in schema.HardDrives[schemaCounter].Partitions)
                {
                    if (!part.Active) continue;
                    string imageFile = null;
                    foreach (var ext in new[] {".ntfs", ".fat", ".extfs", ".hfsp", ".imager", ".winpe", ".xfs"})
                    {
                        try
                        {
                            imageFile =
                         Directory.GetFiles(
                             imagePath + Path.DirectorySeparatorChar, "part" + part.Number + ext + ".*")
                             .FirstOrDefault();
                        }
                        catch (Exception ex)
                        {
                            Logger.Log(ex.Message);
                            return null;

                        }
                     

                        if (imageFile != null) break;

                        //Look for lvm
                        if (part.VolumeGroup == null) continue;
                        if (part.VolumeGroup.LogicalVolumes == null) continue;
                        foreach (var lv in part.VolumeGroup.LogicalVolumes.Where(lv => lv.Active))
                        {
                            try
                            {
                                imageFile =
                             Directory.GetFiles(
                                 imagePath + imagePath + Path.DirectorySeparatorChar, lv.VolumeGroup + "-" +
                                                                                      lv.Name + ext + ".*")
                                 .FirstOrDefault();
                            }
                            catch (Exception ex)
                            {
                                Logger.Log(ex.Message);
                                return null;
                            }
                         
                        }
                    }

                    if (imageFile == null)
                        continue;
                    if (_imageProfile.Image.Environment == "winpe" &&
                        schema.HardDrives[schemaCounter].Table.ToLower() == "gpt")
                    {
                        if (part.Type.ToLower() == "system" || part.Type.ToLower() == "recovery" ||
                            part.Type.ToLower() == "reserved")
                            continue;
                    }
                    if (_imageProfile.Image.Environment == "winpe" &&
                        schema.HardDrives[schemaCounter].Table.ToLower() == "mbr")
                    {
                        if (part.Number == schema.HardDrives[schemaCounter].Boot && schema.HardDrives[schemaCounter].Partitions.Length > 1)
                            continue;
                    }
                    x++;

                    string minReceivers;
                    string senderArgs;
                    if (_isOnDemand)
                    {
                        senderArgs = Settings.SenderArgs;
                        if (!string.IsNullOrEmpty(_clientCount))
                            minReceivers = " --min-receivers " + _clientCount;
                        else
                            minReceivers = "";
                    }
                    else
                    {
                        senderArgs = string.IsNullOrEmpty(_imageProfile.SenderArguments)
                            ? Settings.SenderArgs
                            : _imageProfile.SenderArguments;
                        minReceivers = " --min-receivers " + _computers.Count;
                    }

                    string compAlg;
                    string stdout = "";
                    switch (Path.GetExtension(imageFile))
                    {
                        case ".lz4":
                            compAlg = isUnix ? "lz4 -d " : "lz4.exe\" -d ";
                            stdout = " - ";
                            break;
                        case ".gz":
                            compAlg = isUnix ? "gzip -c -d " : "gzip.exe\" -c -d ";
                            stdout = "";
                            break;
                        case ".uncp":
                            compAlg = "none";
                            break;
                        case ".wim":
                            compAlg = "none";
                            break;
                        default:
                            return null;
                    }

                    if (isUnix)
                    {
                        string prefix = null;
                        if(multicastHdCounter == 1)
                            prefix = x == 1 ? " -c \"" : " ; ";
                        else
                            prefix = " ; ";


                        if (compAlg == "none" || Settings.MulticastDecompression == "client")
                        {
                            processArguments += (prefix + "cat " + "\"" + imageFile + "\"" + " | udp-sender" +
                                                 " --portbase " + _multicastSession.Port + minReceivers + " " +
                                                 " --ttl 32 " +
                                                 senderArgs);
                        }

                        else
                        {
                            processArguments += (prefix + compAlg + "\"" + imageFile + "\"" + stdout + " | udp-sender" +
                                                 " --portbase " + _multicastSession.Port + minReceivers + " " +
                                                 " --ttl 32 " +
                                                 senderArgs);
                        }
                    }
                    else
                    {
                        var appPath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "private" +
                                      Path.DirectorySeparatorChar + "apps" + Path.DirectorySeparatorChar;

                        string prefix = null;
                        if (multicastHdCounter == 1)
                            prefix = x == 1 ? " /c \"" : " & ";
                        else
                            prefix = " & ";
                       

                        if (compAlg == "none" || Settings.MulticastDecompression == "client")
                        {
                            processArguments += (prefix + "\"" + appPath +
                                                 "udp-sender.exe" + "\"" + " --file " + "\"" + imageFile + "\"" +
                                                 " --portbase " + _multicastSession.Port + minReceivers + " " +
                                                 " --ttl 32 " +
                                                 senderArgs);
                        }
                        else
                        {
                            processArguments += (prefix + "\"" + appPath + compAlg + "\"" + imageFile + "\"" + stdout + " | " + "\"" + appPath +
                                                 "udp-sender.exe" + "\"" +
                                                 " --portbase " + _multicastSession.Port + minReceivers + " " +
                                                 " --ttl 32 " +
                                                 senderArgs);
                        }

                    }
                }
            }

            processArguments += "\"";
            return processArguments;
        }

        private bool StartMulticastSender()
        {
            var isUnix = Environment.OSVersion.ToString().Contains("Unix");

            string shell;
            if (isUnix)
            {
                string dist = null;
                var distInfo = new ProcessStartInfo
                {
                    UseShellExecute = false,
                    FileName = "uname",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                using (var process = Process.Start(distInfo))
                    if (process != null) dist = process.StandardOutput.ReadToEnd();

                shell = dist != null && dist.ToLower().Contains("bsd") ? "/bin/csh" : "/bin/bash";
            }
            else
            {
                shell = "cmd.exe";
            }

            var processArguments = GenerateProcessArguments();
            if (processArguments == null) return false;
            var senderInfo = new ProcessStartInfo {FileName = shell, Arguments = processArguments};

            var logPath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "private" +
                          Path.DirectorySeparatorChar + "logs" + Path.DirectorySeparatorChar + "multicast.log";

            var logText = (Environment.NewLine + DateTime.Now.ToString("MM-dd-yy hh:mm") +
                           " Starting Multicast Session " +
                           _group.Name +
                           " With The Following Command:" + Environment.NewLine + senderInfo.FileName +
                           senderInfo.Arguments
                           + Environment.NewLine);
            File.AppendAllText(logPath, logText);


            Process sender;
            try
            {
               
                sender = Process.Start(senderInfo);
            }
            catch (Exception ex)
            {
                
                Logger.Log(ex.ToString());
                File.AppendAllText(logPath,
                    "Could Not Start Session " + _group.Name + " Try Pasting The Command Into A Command Prompt");
                return false;
            }

            Thread.Sleep(2000);
            
            if (sender == null) return false;

            if (sender.HasExited)
            {

                File.AppendAllText(logPath,
                    "Session " + _group.Name + " Started And Was Forced To Quit, Try Running The Command Manually");
                return false;
            }

            var activeMulticastSessionServices = new ActiveMulticastSessionServices();
            if (_isOnDemand)
            {
                _multicastSession.Pid = sender.Id;
                _multicastSession.Name = _group.Name;
                activeMulticastSessionServices.AddActiveMulticastSession(_multicastSession);
            }
            else
            {
                
                _multicastSession.Pid = sender.Id;
                activeMulticastSessionServices.UpdateActiveMulticastSession(_multicastSession);
            }

            return true;
        }
    }
}