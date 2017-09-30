using System;
using System.Linq;
using System.Text;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;

namespace CloneDeploy_Services.Workflows
{
    public class CreateTaskArguments
    {
        private readonly StringBuilder _activeTaskArguments;
        private readonly ComputerEntity _computer;
        private readonly string _direction;
        private readonly ImageProfileWithImage _imageProfile;
        private readonly ImageProfileServices _imageProfileServices;

        public CreateTaskArguments(ComputerEntity computer, ImageProfileWithImage imageProfile, string direction)
        {
            _computer = computer;
            _imageProfile = imageProfile;
            _direction = direction;
            _activeTaskArguments = new StringBuilder();
            _imageProfileServices = new ImageProfileServices();
        }

        private void AppendString(string value)
        {
            _activeTaskArguments.Append(value);
            _activeTaskArguments.Append(_imageProfile.Image.Environment == "winpe" ? "\r\n" : " ");
        }

        public string Execute(string multicastPort = "")
        {
            var preScripts = "\"";
            var postScripts = "\"";
            foreach (var script in _imageProfileServices.SearchImageProfileScripts(_imageProfile.Id))
            {
                if (Convert.ToBoolean(script.RunPre))
                    preScripts += script.ScriptId + " ";

                if (Convert.ToBoolean(script.RunPost))
                    postScripts += script.ScriptId + " ";
            }
            postScripts += "\"";
            preScripts += "\"";

            var sysprepTags = "\"";
            foreach (var sysprepTag in _imageProfileServices.SearchImageProfileSysprepTags(_imageProfile.Id))
                sysprepTags += sysprepTag.SysprepId + " ";

            sysprepTags = sysprepTags.Trim();
            sysprepTags += "\"";

            var areFilesToCopy = _imageProfileServices.SearchImageProfileFileFolders(_imageProfile.Id).Any();

            //On demand computer may be null if not registered
            if (_computer != null)
            {
                AppendString("computer_name=" + _computer.Name);
                //AppendString("computer_id=" + _computer.Id);
            }

            AppendString("image_name=" + _imageProfile.Image.Name);
            AppendString("profile_id=" + _imageProfile.Id);
            AppendString("server_ip=" + SettingServices.GetSettingValue(SettingStrings.ServerIp));
            //AppendString(_direction == "multicast" ? "multicast=true" : "multicast=false");
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
            AppendString("task_completed_action=" + "\"" + _imageProfile.TaskCompletedAction + "\"");

            if (_direction.Contains("upload"))
            {
                AppendString("osx_target_volume=" + "\"" + _imageProfile.OsxTargetVolume + "\"");
                AppendString("image_type=" + _imageProfile.Image.Type);
                if (Convert.ToBoolean(_imageProfile.RemoveGPT)) AppendString("remove_gpt_structures=true");
                if (Convert.ToBoolean(_imageProfile.SkipShrinkVolumes)) AppendString("skip_shrink_volumes=true");
                if (Convert.ToBoolean(_imageProfile.SkipShrinkLvm)) AppendString("skip_shrink_lvm=true");
                AppendString("compression_algorithm=" + _imageProfile.Compression);
                AppendString("compression_level=-" + _imageProfile.CompressionLevel);
                if (Convert.ToBoolean(_imageProfile.UploadSchemaOnly)) AppendString("upload_schema_only=true");
                if (_imageProfile.Image.Type == "File" && Convert.ToBoolean(_imageProfile.WimMulticastEnabled))
                    AppendString("web_wim_args=--pipable");
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
                {
                    if (!string.IsNullOrEmpty(_computer.CustomAttribute1))
                        AppendString("cust_attr_1=" + "\"" + _computer.CustomAttribute1 + "\"");
                    if (!string.IsNullOrEmpty(_computer.CustomAttribute2))
                        AppendString("cust_attr_2=" + "\"" + _computer.CustomAttribute2 + "\"");
                    if (!string.IsNullOrEmpty(_computer.CustomAttribute3))
                        AppendString("cust_attr_3=" + "\"" + _computer.CustomAttribute3 + "\"");
                    if (!string.IsNullOrEmpty(_computer.CustomAttribute4))
                        AppendString("cust_attr_4=" + "\"" + _computer.CustomAttribute4 + "\"");
                    if (!string.IsNullOrEmpty(_computer.CustomAttribute5))
                        AppendString("cust_attr_5=" + "\"" + _computer.CustomAttribute5 + "\"");
                }

                if (Convert.ToBoolean(_imageProfile.OsxInstallMunki)) AppendString("install_munki=true");
                AppendString("osx_target_volume=" + "\"" + _imageProfile.OsxTargetVolume + "\"");
                AppendString("munki_repo_url=" + "\"" + _imageProfile.MunkiRepoUrl + "\"");
                if (!string.IsNullOrEmpty(_imageProfile.MunkiAuthUsername) &&
                    !string.IsNullOrEmpty(_imageProfile.MunkiAuthPassword))
                    AppendString("munki_requires_auth=true");
                if (Convert.ToBoolean(_imageProfile.ChangeName)) AppendString("change_computer_name=true");
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
                if (_direction.Contains("multicast"))
                {
                    if (SettingServices.GetSettingValue(SettingStrings.MulticastDecompression) == "client")
                        AppendString("decompress_multicast_on_client=true");
                    AppendString("client_receiver_args=" + "\"" + _imageProfile.ReceiverArguments + "\"");
                    AppendString("multicast_port=" + multicastPort);
                }
            }

            return _activeTaskArguments.ToString();
        }

        private void SetCustomSchemaDeploy()
        {
            var imageSchemaRequest = new ImageSchemaRequestDTO();
            imageSchemaRequest.imageProfile = _imageProfile;
            imageSchemaRequest.schemaType = "deploy";
            var customSchema = new ImageSchemaFEServices(imageSchemaRequest).GetImageSchema();
            var customHardDrives = new StringBuilder();
            customHardDrives.Append("custom_hard_drives=\"");

            foreach (var hd in customSchema.HardDrives.Where(x => x.Active && !string.IsNullOrEmpty(x.Destination)))
                customHardDrives.Append(hd.Destination + " ");

            customHardDrives.Append("\"");
            AppendString(customHardDrives.ToString());
        }

        private void SetCustomSchemaUpload()
        {
            var imageSchemaRequest = new ImageSchemaRequestDTO();
            imageSchemaRequest.imageProfile = _imageProfile;
            imageSchemaRequest.schemaType = "upload";
            var customSchema = new ImageSchemaFEServices(imageSchemaRequest).GetImageSchema();
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

        private string SetPartitionMethod()
        {
            switch (_imageProfile.PartitionMethod)
            {
                case "Use Original MBR / GPT":
                    return "partition_method=original";

                case "Dynamic":
                    return "partition_method=dynamic";

                case "Custom Script":
                    return "partition_method=script";

                case "Custom Layout":
                    return "partition_method=layout";

                case "Standard":
                    return "partition_method=standard";

                case "Standard Core Storage":
                    return "partition_method=standardCS";
                default:
                    return "";
            }
        }
    }
}