using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using BLL.DynamicClientPartition;
using Helpers;

namespace BLL.Workflows
{
    public class Multicast
    {
        private readonly string _clientCount;
        private readonly bool _isOnDemand;
        private readonly Models.ActiveMulticastSession _multicastSession;
        private List<Models.Computer> _computers;
        private readonly Models.Group _group;
        private Models.ImageProfile _imageProfile;

        //Constructor For Starting Multicast For Group
        public Multicast(Models.Group group)
        {
            _computers = new List<Models.Computer>();
            _multicastSession = new Models.ActiveMulticastSession();
            _isOnDemand = false;
            _group = group;
        }

        //Constructor For Starting Multicast For On Demand
        public Multicast(Models.ImageProfile imageProfile, string clientCount)
        {
            _multicastSession = new Models.ActiveMulticastSession();
            _isOnDemand = true;
            _imageProfile = imageProfile;
            _clientCount = clientCount;
            _group = new Models.Group{ImageProfile = _imageProfile.Id};
        }

        public string Create()
        {
            _imageProfile = ImageProfile.ReadProfile(_group.ImageProfile);
            if (_imageProfile == null) return "The Image Profile Does Not Exist";

            if (_imageProfile.Image == null) return "The Image Does Not Exist";

            _multicastSession.Port = Port.GetNextPort();
            if (_multicastSession.Port == 0)
            {
                return "Could Not Determine Current Port Base";
            }


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
            _computers = Group.GetGroupMembers(_group.Id);
            if (_computers.Count < 1)
            {
                return "The group Does Not Have Any Members";
            }

            if (!ActiveMulticastSession.AddActiveMulticastSession(_multicastSession))
            {
                return "Could Not Create Multicast Database Task";
            }

            if (!CreateComputerTasks())
            {
                ActiveMulticastSession.Delete(_multicastSession.Id);
                return "Could Not Create Host Database Tasks";
            }

            if (!CreatePxeFiles())
            {
                ActiveMulticastSession.Delete(_multicastSession.Id);
                return "Could Not Create Computer Boot Files";
            }

            if (!CreateTaskArguments())
            {
                ActiveMulticastSession.Delete(_multicastSession.Id);
                return "Could Not Create Host Task Arguments";
            }

            if (!StartMulticastSender())
            {
                ActiveMulticastSession.Delete(_multicastSession.Id);
                return "Could Not Start The Multicast Application";
            }

            foreach (var host in _computers)
                Utility.WakeUp(host.Mac);

            return "Successfully Started Multicast " + _group.Name;
        }

        private bool CreateComputerTasks()
        {
            var error = false;
            var activeTaskIds = new List<int>();
            foreach (var computer in _computers)
            {
                if (ActiveImagingTask.IsComputerActive(computer.Id)) return false;
                var activeTask = new Models.ActiveImagingTask
                {
                    Type = "multicast",
                    ComputerId = computer.Id,
                    Direction = "push"
                };

                if (ActiveImagingTask.AddActiveImagingTask(activeTask))
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
                    ActiveImagingTask.DeleteActiveImagingTask(taskId);

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
                    new CreateTaskArguments(computer, _imageProfile, "multicast").Run();
                if (!ActiveImagingTask.UpdateActiveImagingTask(computer.ActiveImagingTask))
                    return false;
            }
            return true;
        }

        private string GenerateProcessArguments()
        {
            var isUnix = Environment.OSVersion.ToString().Contains("Unix");
            //Multicasting currently only supports the first active hd
            //Find First Active HD
            var schema = new ClientPartitionHelper(_imageProfile).GetImageSchema();

            var activeCounter = 0;
            foreach (var hd in schema.HardDrives)
            {
                if (hd.Active)
                {
                    activeCounter++;
                    break;
                }
            }

            var imagePath = Settings.PrimaryStoragePath + _imageProfile.Image.Name + Path.DirectorySeparatorChar + "hd" +
                            activeCounter;

            string processArguments = null;
            var x = 0;
            foreach (var part in schema.HardDrives[activeCounter - 1].Partitions)
            {
                if (!part.Active) continue;
                string imageFile = null;
                foreach (var ext in new[] {".ntfs", ".fat", ".extfs", ".hfsp", ".imager", ".xfs"})
                {
                    imageFile =
                        Directory.GetFiles(
                            imagePath + Path.DirectorySeparatorChar, "part" + part.Number + ext + ".*")
                            .FirstOrDefault();

                    if (imageFile != null) break;

                    //Look for lvm
                    if (part.VolumeGroup == null) continue;
                    if (part.VolumeGroup.LogicalVolumes == null) continue;
                    foreach (var lv in part.VolumeGroup.LogicalVolumes.Where(lv => lv.Active))
                    {
                        imageFile =
                            Directory.GetFiles(
                                imagePath + imagePath + Path.DirectorySeparatorChar, lv.VolumeGroup + "-" +
                                                                                     lv.Name + ext + ".*")
                                .FirstOrDefault();
                    }
                }

                if (imageFile == null)
                    continue;
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
                    senderArgs = string.IsNullOrEmpty(_group.SenderArguments)
                        ? Settings.SenderArgs
                        : _group.SenderArguments;
                    minReceivers = " --min-receivers " + _computers.Count;
                }

                string compAlg;
                string stdout;
                switch (Path.GetExtension(imageFile))
                {
                    case ".lz4":
                        compAlg = isUnix ? "lz4 -d " : "lz4.exe -d ";
                        stdout = " - ";
                        break;
                    case ".gz":
                        compAlg = isUnix ? "gzip -c -d " : "gzip.exe -c -d ";
                        stdout = "";
                        break;
                    case ".uncp":
                        compAlg = isUnix ? "cat " : "type ";
                        stdout = "";
                        break;
                    default:
                        return null;
                }

                if (isUnix)
                {
                    var prefix = x == 1 ? " -c \"" : " ; ";
                    if (Settings.MulticastDecompression == "server")
                    {
                        processArguments += (prefix + compAlg + imageFile + stdout + " | udp-sender" +
                                             " --portbase " + _multicastSession.Port + minReceivers + " " + " --ttl 32 " +
                                             senderArgs);
                    }
                    else
                    {
                        processArguments += (prefix + " udp-sender" + " --file " + imageFile +
                                             " --portbase " + _multicastSession.Port + minReceivers + " " + " --ttl 32 " +
                                             senderArgs);
                    }

                }
                else
                {
                    var appPath = HttpContext.Current.Server.MapPath("~") + "data" +
                                  Path.DirectorySeparatorChar + "apps" + Path.DirectorySeparatorChar;

                    var prefix = x == 1 ? " /c " : " & ";
                    if (Settings.MulticastDecompression == "server")
                    {
                        processArguments += (prefix + appPath + compAlg + imageFile + stdout + " | " + appPath +
                                             "udp-sender.exe" +
                                             " --portbase " + _multicastSession.Port + minReceivers + " " + " --ttl 32 " +
                                             senderArgs);
                    }
                    else
                    {
                        processArguments += (prefix + appPath +
                                             "udp-sender.exe" + " --file " + imageFile +
                                             " --portbase " + _multicastSession.Port + minReceivers + " " + " --ttl 32 " +
                                             senderArgs);
                    }
                }
            }

            if (isUnix)
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

            var logPath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "data" +
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

            if (_isOnDemand)
            {
                _multicastSession.Pid = sender.Id;
                _multicastSession.Name = _group.Name;
                ActiveMulticastSession.AddActiveMulticastSession(_multicastSession);
            }
            else
            {
                _multicastSession.Pid = sender.Id;
                ActiveMulticastSession.UpdateActiveMulticastSession(_multicastSession);
            }

            return true;
        }
    }
}