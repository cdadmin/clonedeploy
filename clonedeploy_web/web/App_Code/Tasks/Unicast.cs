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
using Global;
using Models;
using Pxe;

namespace Tasks
{
    public class Unicast
    {
        public string Direction { get; set; }
        public Computer Host { get; set; }
        private ActiveImagingTask ActiveTask { get; set; }
        private Image Image { get; set; }
        private LinuxEnvironmentProfile ImageProfile { get; set; }
        
        public void Create()
        {
            if (Host == null) return;

            ActiveTask = new ActiveImagingTask
            {
                Status = "0",
                Type = "unicast",
                QueuePosition = 0,
                ComputerId = Host.Id
            };

            Image = new Image {Id = Host.Image};
            Image.Read();

            ImageProfile = new LinuxEnvironmentProfile {Id = Host.ImageProfile};
            ImageProfile.Read();

            if (!ActiveTask.Create())
            {
                Utility.Message = "Could Not Create Host Database Task";
                return;
            }


            if (!CreatePxeFile())
            {
                ActiveTask.Delete();
                Utility.Message = "Could Not Create PXE File";
                return;
            }

            if (!CreateTaskArguments())
            {
                ActiveTask.Delete();
                Utility.Message = "Could Not Create Host Task Arguments";
                return;
            }

            ActiveImagingTask.WakeUp(Host.Mac);
            Utility.Message = "Successfully Created Task For " + Host.Name;
            CreateHistoryEvents();
        }

        private void CreateHistoryEvents()
        {
            var history = new History
            {
                Type = "Host",
                Notes = Host.Mac,
                TypeId = Host.Id.ToString(),
                Event = Direction == "push" ? "Deploy" : "Upload"
            };
            history.CreateEvent();

            var image = new Image {Id = Host.Image};
            image.Read();
            history.Type = "Image";
            history.Notes = Host.Name;
            history.TypeId = image.Id.ToString();
            history.Event = Direction == "push" ? "Deploy" : "Upload";

            history.CreateEvent();
        }

        private bool CreatePxeFile()
        {
            var taskBootMenu = new TaskBootMenu
            {
                Direction = Direction,
                PxeHostMac = Utility.MacToPxeMac(Host.Mac),
                Kernel = ImageProfile.Kernel,
                BootImage = ImageProfile.BootImage,
                IsMulticast = false,
                Arguments = ImageProfile.KernelArguments
            };

            return taskBootMenu.CreatePxeBoot();
        }

        private bool CreateTaskArguments()
        {
            string preScripts = null;
            string postScripts = null;
            foreach(var script in new ImageProfileScript().Search(ImageProfile.Id))
            {
                if(Convert.ToBoolean(script.RunPre))
                preScripts += script.Id + " ";

                if (Convert.ToBoolean(script.RunPost))
                    postScripts += script.Id + " ";
            }

            string storagePath;
            var xferMode = Settings.ImageTransferMode;
            if (xferMode == "smb" || xferMode == "smb+http")
                storagePath = Settings.SmbPath;
            else
                storagePath = Direction == "pull" ? Settings.NfsUploadPath : Settings.NfsDeployPath;
            //FIX ME
            ActiveTask.Arguments = "imgName=" + Image.Name + " storage=" + storagePath + " hostID=" + Host.Id +
                                   " multicast=false" + " preScripts=" + preScripts + " postScripts=" + postScripts + " xferMode=" + xferMode +
                                   " serverIP=" + Settings.ServerIp + " hostName=" + Host.Name +
                                   " compAlg=" + Settings.CompressionAlgorithm + " compLevel=-";
                                   //Settings.CompressionLevel + " customPartition=" + "\"" + Host.PartitionScript + "\"";

            if (Direction == "pull" && xferMode == "udp+http")
            {
                var portBase = new Port().GetPort();
                ActiveTask.Arguments = ActiveTask.Arguments + " portBase=" + portBase;
            }

            return ActiveTask.Update("arguments");
        }

       
    }
}