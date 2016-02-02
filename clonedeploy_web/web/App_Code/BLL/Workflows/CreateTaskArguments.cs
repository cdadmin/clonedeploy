using System;
using System.Linq;
using System.Text;
using Helpers;

namespace BLL.Workflows
{
    public class CreateTaskArguments
    {
        private readonly StringBuilder _activeTaskArguments;
        private readonly Models.Computer _computer;
        private readonly string _direction;
        private readonly Models.ImageProfile _imageProfile;

        public CreateTaskArguments(Models.Computer computer, Models.ImageProfile imageProfile, string direction)
        {
            _computer = computer;
            _imageProfile = imageProfile;
            _direction = direction;
            _activeTaskArguments = new StringBuilder();
        }

        private string SetPartitionMethod()
        {
            switch (_imageProfile.PartitionMethod)
            {
                case "Use Original MBR / GPT":
                    return "partition_method=original";
                    break;
                case "Dynamic":
                    return "partition_method=dynamic";
                    break;
                case "Custom Script":
                    return "partition_method=script";
                    break;
                case "Custom Layout":
                    return "partition_method=layout";
                    break;
                default:
                    return "";
            }
        }

        private void SetCustomSchemaUpload()
        {
            var customSchema = new BLL.ImageSchema(_imageProfile, "upload").GetImageSchema();
            var customHardDrives = new StringBuilder();
            customHardDrives.Append("custom_hard_drives=\"");
            var customPartitions = new StringBuilder();
            customPartitions.Append("custom_partitions=\"");
            var customFixedPartitions = new StringBuilder();
            customFixedPartitions.Append("custom_fixed_partitions=\"");
            var customLogicalVolumes = new StringBuilder();
            customLogicalVolumes.Append("custom_logical_volumes=\"");
            var customFixedLogicalVolumes = new StringBuilder();
            customFixedLogicalVolumes.Append("custom_fixed_logical_volumes=\"");
            foreach (var hd in customSchema.HardDrives.Where(x => x.Active))
            {
                customHardDrives.Append(hd.Name + " ");
                foreach (var partition in hd.Partitions.Where(x => x.Active))
                {
                    customPartitions.Append(hd.Name + partition.Prefix + partition.Number + " ");
                    if (partition.ForceFixedSize)
                        customFixedPartitions.Append(hd.Name + partition.Prefix + partition.Number + " ");

                    if (partition.VolumeGroup.LogicalVolumes != null)
                    {
                        foreach (
                            var logicalVolume in partition.VolumeGroup.LogicalVolumes.Where(x => x.Active))
                        {
                            var vgName = partition.VolumeGroup.Name.Replace("-", "--");
                            var lvName = logicalVolume.Name.Replace("-", "--");
                            customLogicalVolumes.Append(vgName + "-" + lvName + " ");
                            if (logicalVolume.ForceFixedSize)
                                customFixedLogicalVolumes.Append(vgName + "-" + lvName + " ");
                        }
                    }
                }
            }
            customHardDrives.Append("\"");
            customPartitions.Append("\"");
            customFixedPartitions.Append("\"");
            customLogicalVolumes.Append("\"");
            customFixedLogicalVolumes.Append("\"");
            AppendString(customHardDrives.ToString());
            AppendString(customPartitions.ToString());
            AppendString(customFixedPartitions.ToString());
            AppendString(customLogicalVolumes.ToString());
            AppendString(customFixedLogicalVolumes.ToString());
        }

        private void SetCustomSchemaDeploy()
        {
            var customSchema = new BLL.ImageSchema(_imageProfile, "deploy").GetImageSchema();
            var customHardDrives = new StringBuilder();
            customHardDrives.Append("custom_hard_drives=\"");
                    
            foreach (var hd in customSchema.HardDrives.Where(x => x.Active && !string.IsNullOrEmpty(x.Destination)))          
                customHardDrives.Append(hd.Destination + " ");
                         
            customHardDrives.Append("\"");
            AppendString(customHardDrives.ToString());
        }

        private void AppendString(string value)
        {
            _activeTaskArguments.Append(value);
            _activeTaskArguments.Append(" ");
        }

        public string Run(string multicastPort = "")
        {
            string preScripts = null;
            string postScripts = null;
            foreach (var script in ImageProfileScript.SearchImageProfileScripts(_imageProfile.Id))
            {
                if (Convert.ToBoolean(script.RunPre))
                    preScripts += script.ScriptId + " ";

                if (Convert.ToBoolean(script.RunPost))
                    postScripts += script.ScriptId + " ";
            }

            string sysprepTags = null;
            foreach (var sysprepTag in ImageProfileSysprepTag.SearchImageProfileSysprepTags(_imageProfile.Id))
                sysprepTags += sysprepTag.SysprepId + " ";

            var areFilesToCopy = ImageProfileFileFolder.SearchImageProfileFileFolders(_imageProfile.Id).Any();

            //On demand computer may be null if not registered
            if (_computer != null)
            {
                AppendString("computer_name=" + _computer.Name);
                AppendString("computer_id=" + _computer.Id);
            }

            AppendString("image_name=" + _imageProfile.Image.Name);
            AppendString("profile_id=" + _imageProfile.Id);       
            AppendString("server_ip=" + Settings.ServerIp);
            AppendString(_direction == "multicast" ? "multicast=true" : "multicast=false");
            AppendString("pre_scripts=" + preScripts);
            AppendString("post_scripts=" + postScripts);
            AppendString("file_copy=" + areFilesToCopy);
            AppendString("sysprep_tags=" + sysprepTags);
            
            if (Convert.ToBoolean(_imageProfile.SkipCore))
                AppendString("skip_core_download=true");
            if (Convert.ToBoolean(_imageProfile.SkipClock))
                AppendString("skip_clock=true");
            if (Convert.ToBoolean(_imageProfile.WebCancel))
                AppendString("web_cancel=true");
            AppendString("task_completed_action=" + _imageProfile.TaskCompletedAction);

            if (_direction == "pull")
            {
                //Upload currently only support going to the primary distribution point
                AppendString("dp_id=" + DistributionPoint.GetPrimaryDistributionPoint().Id);
                
                AppendString("image_type=" + _imageProfile.Image.Type);
                if (Convert.ToBoolean(_imageProfile.RemoveGPT)) AppendString("remove_gpt_structures=true");
                if (Convert.ToBoolean(_imageProfile.SkipShrinkVolumes)) AppendString("skip_shrink_volumes=true");
                if (Convert.ToBoolean(_imageProfile.SkipShrinkLvm)) AppendString("skip_shrink_lvm=true");
                AppendString("compression_algorithm=" + _imageProfile.Compression);
                AppendString("compression_level=-" + _imageProfile.CompressionLevel);
                if (Convert.ToBoolean(_imageProfile.UploadSchemaOnly)) AppendString("upload_schema_only=true");
                if (!string.IsNullOrEmpty(_imageProfile.CustomUploadSchema))
                {
                    AppendString("custom_upload_schema=true");
                    SetCustomSchemaUpload();
                }
            }
            else // push or multicast
            {
                //Support For on demand 
                if (_computer != null)
                    AppendString("dp_id=" + Computer.GetDistributionPoint(_computer).Id);              
                else           
                    AppendString("dp_id=" + DistributionPoint.GetPrimaryDistributionPoint().Id);
                
                if (Convert.ToBoolean(_imageProfile.SkipExpandVolumes)) AppendString("skip_expand_volumes=true");
                if (Convert.ToBoolean(_imageProfile.FixBcd)) AppendString("fix_bcd=true");
                if (Convert.ToBoolean(_imageProfile.FixBootloader)) AppendString("fix_bootloader=true");
                if (Convert.ToBoolean(_imageProfile.ForceDynamicPartitions))
                    AppendString("force_dynamic_partitions=true");
                AppendString(SetPartitionMethod());
                if (!string.IsNullOrEmpty(_imageProfile.CustomSchema))
                {
                    AppendString("custom_deploy_schema=true");
                    SetCustomSchemaDeploy();
                }
                if (_direction == "multicast")
                {
                    if (Settings.MulticastDecompression == "client") AppendString("decompress_multicast_on_client=true");
                    AppendString("client_receiver_args=" + _imageProfile.ReceiverArguments);
                    AppendString("multicast_port=" + multicastPort);
                }
            }

            return _activeTaskArguments.ToString();
        }
    }
}