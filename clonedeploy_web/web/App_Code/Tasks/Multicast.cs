/*  
    CrucibleWDS A Windows Deployment Solution
    Copyright (C) 2011  Jon Dolny

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Web;
using Global;
using Models;
using Newtonsoft.Json;
using Partition;
using Pxe;

namespace Tasks
{
    public class Multicast
    {
        public Multicast()
        {
            Hosts = new List<Host>();
            Direction = "push";
            IsCustom = false;
            ActiveMcTask = new ActiveMcTask();
        }

        public string Direction { get; set; }
        public Group Group { get; set; }
        public List<Host> Hosts { get; set; }
        public bool IsCustom { get; set; }
        public ActiveMcTask ActiveMcTask { get; set; }

        public void Create()
        {
            if (Group.Image == null)
            {
                Utility.Message = "The Groups Current Image No Longer Exists";
                return;
            }

            Hosts = Group.GroupMembers();
            if (Hosts.Count < 1)
            {
                Utility.Message = "The Group Does Not Have Any Hosts";
                return;
            }

            ActiveMcTask.Name = Group.Name;
            ActiveMcTask.Image = Group.Image;
            ActiveMcTask.Port = new Port().GetPort();
            if (ActiveMcTask.Port == "0")
            {
                Utility.Message = "Could Not Determine Current Port Base";
                return;
            }
           
            if (!ActiveMcTask.Create())
            {
                Utility.Message = "Could Not Create Multicast Database Task";
                return;
            }

            if (!CreateHostTask())
            {
                Utility.Message = "Could Not Create Host Database Tasks";
                ActiveMcTask.Delete();
                return;
            }

            if (!CreatePxeFiles())
            {
                Utility.Message = "Could Not Create Host PXE Files";
                ActiveMcTask.Delete();
                return;
            }

            if (!CreateTaskArguments())
            {
                Utility.Message = "Could Not Create Host Task Arguments";
                ActiveMcTask.Delete();
                return;
            }

            if (!StartMulticastSender())
            {
                Utility.Message = "Could Not Start The Multicast Sender";
                ActiveMcTask.Delete();
                return;
            }

            foreach (var host in Hosts)
                ActiveTask.WakeUp(host.Mac);

            Utility.Message = "Successfully Started Multicast " + Group.Name;
            CreateHistoryEvents();
        }

        private void CreateHistoryEvents()
        {
            var history = new History
            {
                Event = "Multicast",
                Type = "Group",
                TypeId = Group.Id
            };
            history.CreateEvent();

            foreach (var host in Hosts)
            {
                history.Event = "Deploy";
                history.Type = "Host";
                history.Notes = "Via Group Multicast: " + Group.Name;
                history.TypeId = host.Id.ToString();
                history.CreateEvent();

                var image = new Image {Name = Group.Image};
                image.Read();
                history.Event = "Deploy";
                history.Type = "Image";
                history.Notes = Group.Image;
                history.TypeId = image.Id.ToString();
                history.CreateEvent();
            }
        }

        private bool CreateHostTask()
        {
            foreach (var host in Hosts)
            {
                var activeTask = new ActiveTask
                {
                    Name = host.Name,
                    Status = "0",
                    Type = "multicast",
                    MulticastName = Group.Name
                };
                if (!activeTask.Create())
                    return false;
                host.TaskId = activeTask.Id;
            }
            return true;
        }

        private bool CreatePxeFiles()
        {
            foreach (var host in Hosts)
            {
                var menu = new TaskBootMenu
                {
                    Kernel = Group.Kernel,
                    BootImage = Group.BootImage,
                    Arguments = Group.Args,
                    PxeHostMac = Utility.MacToPxeMac(host.Mac),
                    Direction = "push",
                    IsMulticast = true
                };
                if (!menu.CreatePxeBoot())
                    return false;
            }
            return true;
        }

        private bool CreateTaskArguments()
        {
            var xferMode = Settings.ImageTransferMode;
            foreach (var host in Hosts)
            {
                var activeTask = new ActiveTask {Id = host.TaskId};
                string storagePath;

                if (xferMode == "smb" || xferMode == "smb+http")
                    storagePath = Settings.SmbPath;
                else
                    storagePath = Direction == "pull" ? Settings.NfsUploadPath : Settings.NfsDeployPath;
                activeTask.Arguments = "imgName=" + Group.Image + " storage=" + storagePath +
                                       " hostID=" + host.Id + " multicast=true " + " hostScripts=" + Group.Scripts +
                                       " xferMode=" + xferMode + " serverIP=" + Settings.ServerIp +
                                       " hostName=" + host.Name + " portBase=" + ActiveMcTask.Port + 
                                       " clientReceiverArgs=" + Settings.ClientReceiverArgs;

                if(!activeTask.Update("arguments"))
                    return false;
            }
            return true;
        }

        public bool StartMulticastSender()
        {
            if (IsCustom)
            {
                ActiveMcTask.Port = new Port().GetPort();
                Group = new Group {Name = ActiveMcTask.Port};
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

            var receivers = Hosts.Count;

            Process sender;
            var senderInfo = new ProcessStartInfo {FileName = (shell)};

            string compExt;
            string compAlg;
            string stdout;

            //Multicasting currently only supports the first active hd
            //Find First Active HD
            var image = new Image {Name = ActiveMcTask.Image};
            image.Read();
            ImagePhysicalSpecs specs;
            if (!string.IsNullOrEmpty(image.ClientSizeCustom))
            {
                specs = JsonConvert.DeserializeObject<ImagePhysicalSpecs>(image.ClientSizeCustom);
                try
                {
                    specs = JsonConvert.DeserializeObject<ImagePhysicalSpecs>(image.ClientSizeCustom);
                }
                catch
                {
                    // ignored
                }
            }
            else
            {
                specs = JsonConvert.DeserializeObject<ImagePhysicalSpecs>(image.ClientSize);
                try
                {
                    specs = JsonConvert.DeserializeObject<ImagePhysicalSpecs>(image.ClientSize);
                }
                catch
                {
                    // ignored
                }
            }
            var activeCounter = 0;
            foreach (var hd in specs.Hd)
            {
                if (hd.Active == "1")
                {
                    break;
                }
                activeCounter++;
            }

            string imagePath;
            if (activeCounter == 0)
            {
                imagePath = Settings.ImageStorePath + ActiveMcTask.Image;
            }
            else
            {
                imagePath = Settings.ImageStorePath + ActiveMcTask.Image + Path.DirectorySeparatorChar + "hd" +
                            (activeCounter + 1);
            }

            try
            {
                var partFiles = Directory.GetFiles(imagePath + Path.DirectorySeparatorChar, "*.gz*");
                if (partFiles.Length == 0)
                {
                    partFiles = Directory.GetFiles(imagePath + Path.DirectorySeparatorChar, "*.lz4*");
                    if (partFiles.Length == 0)
                    {
                        Utility.Message = "Image Files Could Not Be Located";
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
                Utility.Message = "Image Files Could Not Be Located";
                return false;
            }

            var x = 0;
            foreach (var part in specs.Hd[activeCounter].Partition)
            {
                string udpFile = null;
                if (part.Active != "1") continue;
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
                    if (part.Vg != null)
                    {
                        if (part.Vg.Lv != null)
                        {
                            foreach (var lv in part.Vg.Lv)
                            {
                                if (lv.Active != "1") continue;
                                if (
                                    File.Exists(imagePath + Path.DirectorySeparatorChar + lv.Vg + "-" +
                                                lv.Name + ".ntfs" +
                                                compExt))
                                    udpFile = imagePath + Path.DirectorySeparatorChar + lv.Vg + "-" +
                                              lv.Name + ".ntfs" +
                                              compExt;
                                else if (
                                    File.Exists(imagePath + Path.DirectorySeparatorChar + lv.Vg + "-" +
                                                lv.Name + ".fat" +
                                                compExt))
                                    udpFile = imagePath + Path.DirectorySeparatorChar + lv.Vg + "-" +
                                              lv.Name + ".fat" +
                                              compExt;
                                else if (
                                    File.Exists(imagePath + Path.DirectorySeparatorChar + lv.Vg + "-" +
                                                lv.Name +
                                                ".extfs" + compExt))
                                    udpFile = imagePath + Path.DirectorySeparatorChar + lv.Vg + "-" +
                                              lv.Name +
                                              ".extfs" + compExt;
                                else if (
                                    File.Exists(imagePath + Path.DirectorySeparatorChar + lv.Vg + "-" +
                                                lv.Name +
                                                ".hfsp" + compExt))
                                    udpFile = imagePath + Path.DirectorySeparatorChar + lv.Vg + "-" +
                                              lv.Name +
                                              ".hfsp" + compExt;
                                else if (
                                    File.Exists(imagePath + Path.DirectorySeparatorChar + lv.Vg + "-" +
                                                lv.Name + ".imager" + compExt))
                                    udpFile = imagePath + Path.DirectorySeparatorChar + lv.Vg + "-" +
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
                                                    " --portbase " + ActiveMcTask.Port + " " + senderArgs + " --ttl 32");
                        else
                            senderInfo.Arguments += (" ; " + compAlg + udpFile + stdout + " | udp-sender" +
                                                     " --portbase " + ActiveMcTask.Port + " " + senderArgs + " --ttl 32");
                    }
                    else
                    {
                        if (x == 1)
                            senderInfo.Arguments = (" /c " + appPath + compAlg + udpFile + stdout + " | " + appPath +
                                                    "udp-sender.exe" +
                                                    " --portbase " + ActiveMcTask.Port + " " + senderArgs + " --ttl 32");
                        else
                            senderInfo.Arguments += (" & " + appPath + compAlg + udpFile + stdout + " | " + appPath +
                                                     "udp-sender.exe" +
                                                     " --portbase " + ActiveMcTask.Port + " " + senderArgs + " --ttl 32");
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(Group.SenderArgs))
                        Group.SenderArgs = Settings.SenderArgs;

                    if (Environment.OSVersion.ToString().Contains("Unix"))
                    {
                        if (x == 1)
                            senderInfo.Arguments = (" -c \"" + compAlg + udpFile + stdout + " | udp-sender" +
                                                    " --portbase " + ActiveMcTask.Port + " --min-receivers " + receivers +
                                                    " " +
                                                    Group.SenderArgs + " --ttl 32");

                        else
                            senderInfo.Arguments += (" ; " + compAlg + udpFile + stdout + " | udp-sender" +
                                                     " --portbase " + ActiveMcTask.Port + " --min-receivers " +
                                                     receivers + " " +
                                                     Group.SenderArgs + " --ttl 32");
                    }
                    else
                    {
                        if (x == 1)
                            senderInfo.Arguments = (" /c " + appPath + compAlg + udpFile + stdout + " | " + appPath +
                                                    "udp-sender.exe" +
                                                    " --portbase " + ActiveMcTask.Port + " --min-receivers " + receivers +
                                                    " " +
                                                    Group.SenderArgs + " --ttl 32");
                        else
                            senderInfo.Arguments += (" & " + appPath + compAlg + udpFile + stdout + " | " + appPath +
                                                     "udp-sender.exe" +
                                                     " --portbase " + ActiveMcTask.Port + " --min-receivers " +
                                                     receivers + " " +
                                                     Group.SenderArgs + " --ttl 32");
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
                Utility.Message = "Could Not Start Multicast Sender.  Check The Exception Log For More Info";
                File.AppendAllText(logPath + "multicast.log",
                    "Could Not Start Session " + Group.Name + " Try Pasting The Command Into A Command Prompt");
                return false;
            }

            Thread.Sleep(2000);

            if (sender != null && sender.HasExited)
            {
                Utility.Message = "Could Not Start Multicast Sender";
                File.AppendAllText(logPath + "multicast.log",
                    "Session " + Group.Name +
                    @" Started And Was Forced To Quit, Try Pasting The Command Into A Command Prompt");
                return false;
            }

            if (IsCustom)
            {
                if (sender != null) ActiveMcTask.Pid = sender.Id.ToString();
                ActiveMcTask.Name = Group.Name;
                ActiveMcTask.Create();
                Utility.Message = "Successfully Started Multicast " + Group.Name;
                return true;
            }

            if (sender != null)
            {
                ActiveMcTask.Pid = sender.Id.ToString();
                ActiveMcTask.Update();
            }
        
           return true;
        }


    }
}