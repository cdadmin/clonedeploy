using System;
using Helpers;
using Models;
using Pxe;

namespace BLL.Workflows
{
    public class Unicast
    {
        private string Direction { get; set; }
        private Models.Computer Host { get; set; }
        private Models.ActiveImagingTask ActiveTask { get; set; }
        private Models.Image Image { get; set; }
        private Models.LinuxProfile ImageProfile { get; set; }

        public string Run(Models.Computer computer, string direction)
        {
            if (computer == null)
                return "computer_error";

            Host = computer;
            Direction = direction;

            Image = new Image().GetImage(Host.Image);
            if (Image == null) return "image_error";

            ImageProfile = new LinuxProfile().ReadProfile(Host.ImageProfile);
            if (ImageProfile == null) return "profile_error";

            ActiveTask = new Models.ActiveImagingTask
            {
                Status = "0",
                Type = "unicast",
                QueuePosition = 0,
                ComputerId = Host.Id
            };

            var bllActiveImagingTask = new ActiveImagingTask();
            if (bllActiveImagingTask.IsComputerActive(Host.Id)) return "active";
            if (!bllActiveImagingTask.AddActiveImagingTask(ActiveTask)) return "database_error";

            if (!CreatePxeFile())
            {
                bllActiveImagingTask.DeleteActiveImagingTask(ActiveTask.Id);
                return "pxe_error";
            }

            if (!CreateTaskArguments())
            {
                bllActiveImagingTask.DeleteActiveImagingTask(ActiveTask.Id);
                return "arguments_error";
            }

            Utility.WakeUp(Host.Mac);

            CreateHistoryEvents();

            return "true";
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

            var image = new Image().GetImage(Host.Image);
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
            foreach (var script in new ImageProfileScript().SearchImageProfileScripts(ImageProfile.Id))
            {
                if (Convert.ToBoolean(script.RunPre))
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
                                   " multicast=false" + " pre_scripts=" + preScripts + " post_scripts=" + postScripts +
                                   " xfer_mode=" + xferMode +
                                   " server_ip=" + Settings.ServerIp + " host_name=" + Host.Name +
                                   " comp_alg=" + Settings.CompressionAlgorithm + " comp_evel=-" +
                                   Settings.CompressionLevel + " partition_method=" + ImageProfile.PartitionMethod + " " +
                                   profileArgs;


            return new ActiveImagingTask().UpdateActiveImagingTask(ActiveTask);
        }
    }
}