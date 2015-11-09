using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using BLL.ClientPartitioning;
using Helpers;
using Pxe;

namespace BLL.Workflows
{
    public class Multicast
    {
        public Multicast(Models.Group group, bool isOnDemand = false, Models.ImageProfile imageProfile = null)
        {
            _computers = new List<Models.Computer>();
            _multicastSession = new Models.ActiveMulticastSession();
            _group = group;
            _isOnDemand = isOnDemand;
            //This is only set for an on demand multicast
            if (imageProfile != null)
                _imageProfile = imageProfile;
        }

        private Models.Group _group;
        private List<Models.Computer> _computers;
        private readonly bool _isOnDemand;
        private readonly Models.ActiveMulticastSession _multicastSession;
        private Models.ImageProfile _imageProfile;

        public string Create()
        {
            _imageProfile = BLL.ImageProfile.ReadProfile(_group.ImageProfile);
            if (_imageProfile == null) return "The Image Profile Does Not Exist";

            if (_imageProfile.Image == null) return "The Image Does Not Exist";

            _computers = BLL.Group.GetGroupMembers(_group.Id);
            if (_computers.Count < 1)
            {
                return "The _group Does Not Have Any Members";
            }

            _multicastSession.Name = _group.Name;
            _multicastSession.Port = BLL.Port.GetNextPort();
            if (_multicastSession.Port == 0)
            {
                return "Could Not Determine Current Port Base";
            }
           
            if (!BLL.ActiveMulticastSession.AddActiveMulticastSession(_multicastSession))
            {
                return "Could Not Create Multicast Database Task";
            }

            if (!CreateComputerTasks())
            {
                BLL.ActiveMulticastSession.Delete(_multicastSession.Id);
                return "Could Not Create Host Database Tasks";
            }

            if (!CreatePxeFiles())
            {
                BLL.ActiveMulticastSession.Delete(_multicastSession.Id);
                return "Could Not Create Computer Boot Files";
            }

            if (!CreateTaskArguments())
            {
                BLL.ActiveMulticastSession.Delete(_multicastSession.Id);
                return "Could Not Create Host Task Arguments";
            }

            if (!StartMulticastSender())
            {
                BLL.ActiveMulticastSession.Delete(_multicastSession.Id);
                return  "Could Not Start The Multicast Sender";
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
                if (BLL.ActiveImagingTask.IsComputerActive(computer.Id)) return false;
                var activeTask = new Models.ActiveImagingTask
                {
                    Type = "multicast",
                    ComputerId = computer.Id,
                    Direction = "push"
                };

                if (BLL.ActiveImagingTask.AddActiveImagingTask(activeTask))
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
                foreach(var taskId in activeTaskIds)
                    BLL.ActiveImagingTask.DeleteActiveImagingTask(taskId);

                return false;
            }
            else
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
                string preScripts = null;
                string postScripts = null;
                foreach (var script in BLL.ImageProfileScript.SearchImageProfileScripts(_imageProfile.Id))
                {
                    if (Convert.ToBoolean(script.RunPre))
                        preScripts += script.Id + " ";

                    if (Convert.ToBoolean(script.RunPost))
                        postScripts += script.Id + " ";
                }

                string profileArgs = "";
                if (Convert.ToBoolean(_imageProfile.SkipCore)) profileArgs += "skip_core_download=true ";
                if (Convert.ToBoolean(_imageProfile.SkipClock)) profileArgs += "skip_clock=true ";
                profileArgs += "task_completed_action=" + _imageProfile.TaskCompletedAction + " ";


                if (Convert.ToBoolean(_imageProfile.RemoveGPT)) profileArgs += "remove_gpt_structures=true ";
                if (Convert.ToBoolean(_imageProfile.SkipShrinkVolumes)) profileArgs += "skip_shrink_volumes=true ";
                if (Convert.ToBoolean(_imageProfile.SkipShrinkLvm)) profileArgs += "skip_shrink_lvm=true ";

                computer.ActiveImagingTask.Arguments = "image_name=" + _imageProfile.Image.Name + " storage=" +
                                                       BLL.Computer.GetDistributionPoint(computer) + " host_id=" +
                                                       computer.Id +
                                                       " multicast=false" + " pre_scripts=" + preScripts +
                                                       " post_scripts=" + postScripts +
                                                       " server_ip=" + Settings.ServerIp + " host_name=" + computer.Name +
                                                       " comp_alg=" + Settings.CompressionAlgorithm + " comp_level=-" +
                                                       Settings.CompressionLevel + " partition_method=" +
                                                       _imageProfile.PartitionMethod + " " +
                                                       profileArgs;

                if (!BLL.ActiveImagingTask.UpdateActiveImagingTask(computer.ActiveImagingTask))
                    return false;
            }
            return true;
        }

        private string GenerateProcessArguments()
        {
            bool isUnix = Environment.OSVersion.ToString().Contains("Unix");
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
                foreach (var ext in new[] { ".ntfs", ".fat", ".extfs", ".hfsp", ".imager" })
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
                    _multicastSession.Port = BLL.Port.GetNextPort();
                    _group = new Models.Group { Name = _multicastSession.Port.ToString() };
                    senderArgs = Settings.SenderArgs;
                    minReceivers = "";
                }
                else
                {
                    senderArgs = string.IsNullOrEmpty(_group.SenderArguments) ? Settings.SenderArgs : _group.SenderArguments;
                    minReceivers = " --min-receivers " + _computers.Count; ;
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
                    case ".none":
                        compAlg = isUnix ? "cat " : "type ";
                        stdout = "";
                        break;
                    default:
                        return null;
                }

                if (isUnix)
                {
                    string prefix = null;
                    prefix = x == 1 ? " -c \"" : " ; ";
                    processArguments += (prefix + compAlg + imageFile + stdout + " | udp-sender" +
                                            " --portbase " + _multicastSession.Port + minReceivers + " " + " --ttl 32 " +
                                            senderArgs);
                }
                else
                {
                    var appPath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "data" +
                          Path.DirectorySeparatorChar + "apps" + Path.DirectorySeparatorChar;

                    string prefix = null;
                    prefix = x == 1 ? " /c " : " & ";
                    processArguments += (prefix + appPath + compAlg + imageFile + stdout + " | " + appPath +
                                            "udp-sender.exe" +
                                            " --portbase " + _multicastSession.Port + minReceivers + " " + " --ttl 32 " +
                                            senderArgs);
                }
            }

            if (isUnix)
                processArguments += "\"";

            return processArguments;
        }

        public bool StartMulticastSender()
        {
            bool isUnix = Environment.OSVersion.ToString().Contains("Unix");
         
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
            var senderInfo = new ProcessStartInfo { FileName = shell, Arguments = processArguments};

            var logPath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "data" +
                          Path.DirectorySeparatorChar + "logs" + Path.DirectorySeparatorChar;

            var log = ("\r\n" + DateTime.Now.ToString("MM.dd.yy hh:mm") + " Starting Multicast Session " +
                       _group.Name +
                       " With The Following Command:\r\n\r\n" + senderInfo.FileName + senderInfo.Arguments +
                       "\r\n\r\n");
            File.AppendAllText(logPath + "multicast.log", log);


            Process sender;
            try
            {
                sender = Process.Start(senderInfo);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString());
                File.AppendAllText(logPath + "multicast.log",
                    "Could Not Start Session " + _group.Name + " Try Pasting The Command Into A Command Prompt");
                return false;
            }

            Thread.Sleep(2000);


            if (sender.HasExited)
            {
                File.AppendAllText(logPath + "multicast.log",
                    "Session " + _group.Name +
                    @" Started And Was Forced To Quit, Try Pasting The Command Into A Command Prompt");
                return false;
            }

            if (_isOnDemand)
            {
                _multicastSession.Pid = sender.Id;
                _multicastSession.Name = _group.Name;
                BLL.ActiveMulticastSession.AddActiveMulticastSession(_multicastSession);
            }
            else
            {
                _multicastSession.Pid = sender.Id;
                BLL.ActiveMulticastSession.UpdateActiveMulticastSession(_multicastSession);
            }
        
            return true;
        }


    }
}