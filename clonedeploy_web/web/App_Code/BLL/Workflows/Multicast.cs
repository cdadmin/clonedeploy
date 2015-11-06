using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Web;
using BLL.ClientPartitioning;
using Helpers;
using Models;
using Models.ImageSchema;
using Newtonsoft.Json;
using Partition;
using Pxe;
using GroupMembership = BLL.GroupMembership;

namespace Tasks
{
    public class Multicast
    {
        public Multicast(Models.Group group)
        {
            Computers = new List<Computer>();
            Direction = "push";
            IsCustom = false;
            MulticastSession = new ActiveMulticastSession();
            Group = group;
        }

        public string Direction { get; set; }
        public Group Group { get; set; }
        private List<Computer> Computers { get; set; }
        public bool IsCustom { get; set; }
        public ActiveMulticastSession MulticastSession { get; set; }
        private Models.ImageProfile _imageProfile;

        public string Create()
        {
            _imageProfile = BLL.LinuxProfile.ReadProfile(Group.ImageProfile);
            if (_imageProfile == null) return "The Image Profile Does Not Exist";

            if (_imageProfile.Image == null) return "The Image Does Not Exist";

            Computers = BLL.Group.GetGroupMembers(Group.Id);
            if (Computers.Count < 1)
            {
                return "The Group Does Not Have Any Members";
            }

            MulticastSession.Name = Group.Name;
            MulticastSession.Port = BLL.Port.GetNextPort();
            if (MulticastSession.Port == 0)
            {
                return "Could Not Determine Current Port Base";
            }
           
            if (!BLL.ActiveMulticastSession.AddActiveMulticastSession(MulticastSession))
            {
                return "Could Not Create Multicast Database Task";
            }

            if (!CreateComputerTasks())
            {
                BLL.ActiveMulticastSession.Delete(MulticastSession.Id);
                return "Could Not Create Host Database Tasks";
            }

            if (!CreatePxeFiles())
            {
                BLL.ActiveMulticastSession.Delete(MulticastSession.Id);
                return "Could Not Create Computer Boot Files";
            }

            if (!CreateTaskArguments())
            {
                BLL.ActiveMulticastSession.Delete(MulticastSession.Id);
                return "Could Not Create Host Task Arguments";
            }

            if (!StartMulticastSender())
            {
                BLL.ActiveMulticastSession.Delete(MulticastSession.Id);
                return  "Could Not Start The Multicast Sender";
            }

            foreach (var host in Computers)
                Utility.WakeUp(host.Mac);

           
            CreateHistoryEvents();

            return "Successfully Started Multicast " + Group.Name;
        }

        private void CreateHistoryEvents()
        {
            var history = new History
            {
                Event = "Multicast",
                Type = "Group",
                TypeId = Group.Id.ToString()
            };
            history.CreateEvent();

            foreach (var host in Computers)
            {
                history.Event = "Deploy";
                history.Type = "Host";
                history.Notes = "Via Group Multicast: " + Group.Name;
                history.TypeId = host.Id.ToString();
                history.CreateEvent();

                var image = BLL.Image.GetImage(Group.Image);
                history.Event = "Deploy";
                history.Type = "Image";
                history.Notes = Group.Image.ToString();
                history.TypeId = image.Id.ToString();
                history.CreateEvent();
            }
        }

        private bool CreateComputerTasks()
        {
            var error = false;
            var activeTaskIds = new List<int>();
            foreach (var computer in Computers)
            {
                if (BLL.ActiveImagingTask.IsComputerActive(computer.Id)) return false;
                var activeTask = new ActiveImagingTask
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
            foreach (var computer in Computers)
            {
                if (!new TaskBootMenu(computer, _imageProfile, "push").CreatePxeBootFiles())
                    return false;
            }
            return true;
        }

        private bool CreateTaskArguments()
        {

            foreach (var computer in Computers)
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

        public bool StartMulticastSender()
        {
            if (IsCustom)
            {
                MulticastSession.Port = BLL.Port.GetNextPort();
                Group = new Group {Name = MulticastSession.Port.ToString()};
            }
            string shell;

            var appPath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "data" +
                          Path.DirectorySeparatorChar + "apps" + Path.DirectorySeparatorChar;
            var logPath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "data" +
                          Path.DirectorySeparatorChar + "logs" + Path.DirectorySeparatorChar;
            if (Environment.OSVersion.ToString().Contains("Unix"))
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
                {
                    if (process != null) dist = process.StandardOutput.ReadToEnd();
                }

                shell = dist != null && dist.ToLower().Contains("bsd") ? "/bin/csh" : "/bin/bash";
            }
            else
            {
                shell = "cmd.exe";
            }

            var receivers = Computers.Count;

            Process sender;
            var senderInfo = new ProcessStartInfo {FileName = (shell)};

            string compExt;
            string compAlg;
            string stdout;

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
            

            try
            {
                var partFiles = Directory.GetFiles(imagePath + Path.DirectorySeparatorChar, "*.gz*");
                if (partFiles.Length == 0)
                {
                    partFiles = Directory.GetFiles(imagePath + Path.DirectorySeparatorChar, "*.lz4*");
                    if (partFiles.Length == 0)
                    {
                        //Message.Text = "Image Files Could Not Be Located";
                        return false;
                    }
                    compAlg = Environment.OSVersion.ToString().Contains("Unix") ? "lz4 -d " : "lz4.exe -d ";
                    compExt = ".lz4";
                    stdout = " - ";
                }
                else
                {
                    compAlg = Environment.OSVersion.ToString().Contains("Unix") ? "gzip -c -d " : "gzip.exe -c -d ";

                    compExt = ".gz";
                    stdout = "";
                }
            }
            catch
            {
                //Message.Text = "Image Files Could Not Be Located";
                return false;
            }

            var x = 0;
            foreach (var part in schema.HardDrives[activeCounter - 1].Partitions)
            {
                string udpFile = null;
                if (!part.Active) continue;
                if (File.Exists(imagePath + Path.DirectorySeparatorChar + "part" + part.Number + ".ntfs" + compExt))
                    udpFile = imagePath + Path.DirectorySeparatorChar + "part" + part.Number + ".ntfs" + compExt;
                else if (File.Exists(imagePath + Path.DirectorySeparatorChar + "part" + part.Number + ".fat" + compExt))
                    udpFile = imagePath + Path.DirectorySeparatorChar + "part" + part.Number + ".fat" + compExt;
                else if (File.Exists(imagePath + Path.DirectorySeparatorChar + "part" + part.Number + ".extfs" + compExt))
                    udpFile = imagePath + Path.DirectorySeparatorChar + "part" + part.Number + ".extfs" + compExt;
                else if (
                    File.Exists(imagePath + Path.DirectorySeparatorChar + "part" + part.Number + ".hfsp" +
                                compExt))
                    udpFile = imagePath + Path.DirectorySeparatorChar + "part" + part.Number + ".hfsp" + compExt;
                else if (
                    File.Exists(imagePath + Path.DirectorySeparatorChar + "part" + part.Number + ".imager" +
                                compExt))
                    udpFile = imagePath + Path.DirectorySeparatorChar + "part" + part.Number + ".imager" +
                              compExt;
                else
                {
                    //Look for lvm
                    if (part.VolumeGroup != null)
                    {
                        if (part.VolumeGroup.LogicalVolumes != null)
                        {
                            foreach (var lv in part.VolumeGroup.LogicalVolumes)
                            {
                                if (!lv.Active) continue;
                                if (
                                    File.Exists(imagePath + Path.DirectorySeparatorChar + lv.VolumeGroup + "-" +
                                                lv.Name + ".ntfs" +
                                                compExt))
                                    udpFile = imagePath + Path.DirectorySeparatorChar + lv.VolumeGroup + "-" +
                                              lv.Name + ".ntfs" +
                                              compExt;
                                else if (
                                    File.Exists(imagePath + Path.DirectorySeparatorChar + lv.VolumeGroup + "-" +
                                                lv.Name + ".fat" +
                                                compExt))
                                    udpFile = imagePath + Path.DirectorySeparatorChar + lv.VolumeGroup + "-" +
                                              lv.Name + ".fat" +
                                              compExt;
                                else if (
                                    File.Exists(imagePath + Path.DirectorySeparatorChar + lv.VolumeGroup + "-" +
                                                lv.Name +
                                                ".extfs" + compExt))
                                    udpFile = imagePath + Path.DirectorySeparatorChar + lv.VolumeGroup + "-" +
                                              lv.Name +
                                              ".extfs" + compExt;
                                else if (
                                    File.Exists(imagePath + Path.DirectorySeparatorChar + lv.VolumeGroup + "-" +
                                                lv.Name +
                                                ".hfsp" + compExt))
                                    udpFile = imagePath + Path.DirectorySeparatorChar + lv.VolumeGroup + "-" +
                                              lv.Name +
                                              ".hfsp" + compExt;
                                else if (
                                    File.Exists(imagePath + Path.DirectorySeparatorChar + lv.VolumeGroup + "-" +
                                                lv.Name + ".imager" + compExt))
                                    udpFile = imagePath + Path.DirectorySeparatorChar + lv.VolumeGroup + "-" +
                                              lv.Name + ".imager" + compExt;
                            }
                        }
                    }
                }

                if (udpFile == null)
                    continue;
                x++;

                if (IsCustom)
                {
                    var senderArgs = Settings.SenderArgs;
                    if (Environment.OSVersion.ToString().Contains("Unix"))
                    {
                        if (x == 1)
                            senderInfo.Arguments = (" -c \"" + compAlg + udpFile + stdout + " | udp-sender" +
                                                    " --portbase " + MulticastSession.Port + " " + " --ttl 32 " + senderArgs);
                        else
                            senderInfo.Arguments += (" ; " + compAlg + udpFile + stdout + " | udp-sender" +
                                                     " --portbase " + MulticastSession.Port + " " + " --ttl 32 " + senderArgs);
                    }
                    else
                    {
                        if (x == 1)
                            senderInfo.Arguments = (" /c " + appPath + compAlg + udpFile + stdout + " | " + appPath +
                                                    "udp-sender.exe" +
                                                    " --portbase " + MulticastSession.Port + " " + " --ttl 32 " + senderArgs);
                        else
                            senderInfo.Arguments += (" & " + appPath + compAlg + udpFile + stdout + " | " + appPath +
                                                     "udp-sender.exe" +
                                                     " --portbase " + MulticastSession.Port + " " + " --ttl 32 " + senderArgs);
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(Group.SenderArguments))
                        Group.SenderArguments = Settings.SenderArgs;

                    if (Environment.OSVersion.ToString().Contains("Unix"))
                    {
                        if (x == 1)
                            senderInfo.Arguments = (" -c \"" + compAlg + udpFile + stdout + " | udp-sender" +
                                                    " --portbase " + MulticastSession.Port + " --min-receivers " + receivers +
                                                    " " + " --ttl 32 " +
                                                    Group.SenderArguments);

                        else
                            senderInfo.Arguments += (" ; " + compAlg + udpFile + stdout + " | udp-sender" +
                                                     " --portbase " + MulticastSession.Port + " --min-receivers " +
                                                     receivers + " " + " --ttl 32 " +
                                                     Group.SenderArguments);
                    }
                    else
                    {
                        if (x == 1)
                            senderInfo.Arguments = (" /c " + appPath + compAlg + udpFile + stdout + " | " + appPath +
                                                    "udp-sender.exe" +
                                                    " --portbase " + MulticastSession.Port + " --min-receivers " + receivers +
                                                    " " + " --ttl 32 " +
                                                    Group.SenderArguments);
                        else
                            senderInfo.Arguments += (" & " + appPath + compAlg + udpFile + stdout + " | " + appPath +
                                                     "udp-sender.exe" +
                                                     " --portbase " + MulticastSession.Port + " --min-receivers " +
                                                     receivers + " " + " --ttl 32 " +
                                                     Group.SenderArguments);
                    }
                }
            }

            if (Environment.OSVersion.ToString().Contains("Unix"))
            {
                senderInfo.Arguments += "\"";
            }


            var log = ("\r\n" + DateTime.Now.ToString("MM.dd.yy hh:mm") + " Starting Multicast Session " +
                       Group.Name +
                       " With The Following Command:\r\n\r\n" + senderInfo.FileName + senderInfo.Arguments +
                       "\r\n\r\n");
            File.AppendAllText(logPath + "multicast.log", log);


            try
            {
                sender = Process.Start(senderInfo);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString());
                //Message.Text = "Could Not Start Multicast Sender.  Check The Exception Log For More Info";
                File.AppendAllText(logPath + "multicast.log",
                    "Could Not Start Session " + Group.Name + " Try Pasting The Command Into A Command Prompt");
                return false;
            }

            Thread.Sleep(2000);

            if (sender != null && sender.HasExited)
            {
                //Message.Text = "Could Not Start Multicast Sender";
                File.AppendAllText(logPath + "multicast.log",
                    "Session " + Group.Name +
                    @" Started And Was Forced To Quit, Try Pasting The Command Into A Command Prompt");
                return false;
            }

            if (IsCustom)
            {
                if (sender != null) MulticastSession.Pid = sender.Id;
                MulticastSession.Name = Group.Name;
                BLL.ActiveMulticastSession.AddActiveMulticastSession(MulticastSession);
                //Message.Text = "Successfully Started Multicast " + Group.Name;
                return true;
            }

            if (sender != null)
            {
                MulticastSession.Pid = sender.Id;
                BLL.ActiveMulticastSession.UpdateActiveMulticastSession(MulticastSession);
            }
        
           return true;
        }


    }
}