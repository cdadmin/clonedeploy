using System;
using Helpers;
using Models;
using Pxe;

namespace BLL.Workflows
{
    public class Unicast
    {
        private readonly string _direction;
        private readonly Models.Computer _computer;
        private Models.ActiveImagingTask _activeTask;
        private Models.ImageProfile _imageProfile;

        public Unicast(Models.Computer computer, string direction)
        {
            _direction = direction;
            _computer = computer;
        }

        public string Start()
        {
            if (_computer == null)
                return "The Computer Does Not Exist";

            _imageProfile = BLL.LinuxProfile.ReadProfile(_computer.ImageProfile);
            if (_imageProfile == null) return "The Image Profile Does Not Exist";

            if (_imageProfile.Image == null) return "The Image Does Not Exist";

            if (BLL.ActiveImagingTask.IsComputerActive(_computer.Id)) return "This Computer Is Already Part Of An Active Task";

            _activeTask = new Models.ActiveImagingTask
            {
                Type = "unicast",
                ComputerId = _computer.Id,
                Direction = _direction
            };

            if (!BLL.ActiveImagingTask.AddActiveImagingTask(_activeTask)) return "Could Not Create The Database Entry For This Task";

            if (!CreatePxeFile())
            {
                BLL.ActiveImagingTask.DeleteActiveImagingTask(_activeTask.Id);
                return "Could Not Create PXE Boot File";
            }

            if (!CreateTaskArguments())
            {
                BLL.ActiveImagingTask.DeleteActiveImagingTask(_activeTask.Id);
                return "Could Not Create Task Arguments";
            }

            Utility.WakeUp(_computer.Mac);

            CreateHistoryEvents();

            return "true";
        }

        private void CreateHistoryEvents()
        {
          
        }

        private bool CreatePxeFile()
        {
            return new TaskBootMenu(_computer,_direction).CreatePxeBoot();
        }

        private bool CreateTaskArguments()
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
            switch (_direction)
            {
                case "pull":
                    if (Convert.ToBoolean(_imageProfile.RemoveGPT)) profileArgs += "remove_gpt_structures=true ";
                    if (Convert.ToBoolean(_imageProfile.SkipShrinkVolumes)) profileArgs += "skip_shrink_volumes=true ";
                    if (Convert.ToBoolean(_imageProfile.SkipShrinkLvm)) profileArgs += "skip_shrink_lvm=true ";
                  
                    break;
                case "push":
                    if (Convert.ToBoolean(_imageProfile.SkipExpandVolumes)) profileArgs += "skip_expand_volumes=true ";
                    if (Convert.ToBoolean(_imageProfile.FixBcd)) profileArgs += "fix_bcd=true ";
                    if (Convert.ToBoolean(_imageProfile.FixBootloader)) profileArgs += "fix_bootloader=true ";
                    break;
            }
            
         
            _activeTask.Arguments = "image_name=" + _imageProfile.Image.Name + " storage=" + BLL.Computer.GetDistributionPoint(_computer) + " host_id=" + _computer.Id +
                                   " multicast=false" + " pre_scripts=" + preScripts + " post_scripts=" + postScripts +
                                   " server_ip=" + Settings.ServerIp + " host_name=" + _computer.Name +
                                   " comp_alg=" + Settings.CompressionAlgorithm + " comp_evel=-" +
                                   Settings.CompressionLevel + " partition_method=" + _imageProfile.PartitionMethod + " " +
                                   profileArgs;


            return BLL.ActiveImagingTask.UpdateActiveImagingTask(_activeTask);
        }
    }
}