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
using BLL;
using Global;
using Models;
using Pxe;

namespace Tasks
{
    public class Unicast
    {
        public string Direction { get; set; }
        public Models.Computer Host { get; set; }
        private Models.ActiveImagingTask ActiveTask { get; set; }
        private Models.Image Image { get; set; }
        private Models.LinuxProfile ImageProfile { get; set; }
        
        public void Create()
        {
            if (Host == null) return;

            ActiveTask = new Models.ActiveImagingTask
            {
                Status = "0",
                Type = "unicast",
                QueuePosition = 0,
                ComputerId = Host.Id
            };

            Image = new BLL.Image().GetImage(Host.Image);



            ImageProfile = new BLL.LinuxProfile().ReadProfile(Host.ImageProfile);
            if (ImageProfile == null) return;
         
            var bllActiveImagingTask = new BLL.ActiveImagingTask();
            if (!bllActiveImagingTask.AddActiveImagingTask(ActiveTask)) return;
            


            if (!CreatePxeFile())
            {
                bllActiveImagingTask.DeleteActiveImagingTask(ActiveTask.Id);
                return;
            }

            if (!CreateTaskArguments())
            {
                 bllActiveImagingTask.DeleteActiveImagingTask(ActiveTask.Id);
                return;
            }



            BLL.ActiveImagingTask.WakeUp(Host.Mac);
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

            var image = new BLL.Image().GetImage(Host.Image);
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
            foreach(var script in new BLL.ImageProfileScript().SearchImageProfileScripts(ImageProfile.Id))
            {
                if(Convert.ToBoolean(script.RunPre))
                preScripts += script.Id + " ";

                if (Convert.ToBoolean(script.RunPost))
                    postScripts += script.Id + " ";
            }

            string profileArgs = "";
            if (Convert.ToBoolean(ImageProfile.SkipCore)) profileArgs += "skip_core_download=true ";
            if (Convert.ToBoolean(ImageProfile.SkipClock)) profileArgs += "skip_clock=true ";
            profileArgs += "task_completed_action=" + ImageProfile.TaskCompletedAction + " ";
            switch (Direction)
            {
                case "pull":
                    if (Convert.ToBoolean(ImageProfile.RemoveGPT)) profileArgs += "remove_gpt_structures=true ";
                    if (Convert.ToBoolean(ImageProfile.SkipShrinkVolumes)) profileArgs += "skip_shrink_volumes=true ";
                    if (Convert.ToBoolean(ImageProfile.SkipShrinkLvm)) profileArgs += "skip_shrink_lvm=true ";
                    if (Convert.ToBoolean(ImageProfile.DebugResize)) profileArgs += "debug_resize=true ";
                    break;
                case "push":
                    if (Convert.ToBoolean(ImageProfile.SkipExpandVolumes)) profileArgs += "skip_expand_volumes=true ";
                    if (Convert.ToBoolean(ImageProfile.FixBcd)) profileArgs += "fix_bcd=true ";
                    if (Convert.ToBoolean(ImageProfile.FixBootloader)) profileArgs += "fix_bootloader=true ";
                    break;
            }
            string storagePath;
            var xferMode = Settings.ImageTransferMode;
            if (xferMode == "smb" || xferMode == "smb+http")
                storagePath = Settings.SmbPath;
            else
                storagePath = Direction == "pull" ? Settings.NfsUploadPath : Settings.NfsDeployPath;
            
            ActiveTask.Arguments = "image_name=" + Image.Name + " storage=" + storagePath + " host_id=" + Host.Id +
                                   " multicast=false" + " pre_scripts=" + preScripts + " post_scripts=" + postScripts + " xfer_mode=" + xferMode +
                                   " server_ip=" + Settings.ServerIp + " host_name=" + Host.Name +
                                   " comp_alg=" + Settings.CompressionAlgorithm + " comp_evel=-" +
                                   Settings.CompressionLevel + " partition_method=" + ImageProfile.PartitionMethod + " " + profileArgs;



            return new BLL.ActiveImagingTask().UpdateActiveImagingTask(ActiveTask);
        }

       
    }
}