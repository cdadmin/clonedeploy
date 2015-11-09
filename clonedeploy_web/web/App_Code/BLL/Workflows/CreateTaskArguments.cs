using System;
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

        private void AppendString(string value)
        {
            _activeTaskArguments.Append(value);
            _activeTaskArguments.Append(" ");
        }

        public string Run()
        {
            string preScripts = null;
            string postScripts = null;
            foreach (var script in ImageProfileScript.SearchImageProfileScripts(_imageProfile.Id))
            {
                if (Convert.ToBoolean(script.RunPre))
                    preScripts += script.Id + " ";

                if (Convert.ToBoolean(script.RunPost))
                    postScripts += script.Id + " ";
            }

            string sysprepTags = null;
            foreach (var sysprepTag in ImageProfileSysprepTag.SearchImageProfileSysprepTags(_imageProfile.Id))
                sysprepTags += sysprepTag.Id + " ";

            string filesFolders = null;
            foreach (var fileFolder in ImageProfileFileFolder.SearchImageProfileFileFolders(_imageProfile.Id))
                filesFolders += fileFolder.Id + " ";

            AppendString("computer_name=" + _computer.Name);
            AppendString("computer_id=" + _computer.Id);
            AppendString("image_name=" + _imageProfile.Image.Name);
            AppendString("profile_id=" + _imageProfile.Id);
            AppendString("storage=" + Computer.GetDistributionPoint(_computer));
            AppendString("server_ip=" + Settings.ServerIp);
            AppendString(_direction == "multicast" ? "multicast=true" : "multicast=false");
            AppendString("pre_scripts=" + preScripts);
            AppendString("post_scripts=" + postScripts);
            AppendString("file_copy=" + filesFolders);
            AppendString("syprep_tags=" + sysprepTags);
            if (Convert.ToBoolean(_imageProfile.SkipCore))
                AppendString("skip_core_download=true");
            if (Convert.ToBoolean(_imageProfile.SkipClock))
                AppendString("skip_clock=true");
            AppendString("task_completed_action=" + _imageProfile.TaskCompletedAction);

            switch (_direction)
            {
                case "pull":
                    if (Convert.ToBoolean(_imageProfile.RemoveGPT)) AppendString("remove_gpt_structures=true");
                    if (Convert.ToBoolean(_imageProfile.SkipShrinkVolumes)) AppendString("skip_shrink_volumes=true");
                    if (Convert.ToBoolean(_imageProfile.SkipShrinkLvm)) AppendString("skip_shrink_lvm=true");
                    AppendString("compression_algorithm=" + _imageProfile.Compression);
                    AppendString("comperssion_level=" + _imageProfile.CompressionLevel);
                    if (Convert.ToBoolean(_imageProfile.UploadSchemaOnly)) AppendString("upload_schema_only=true");
                    if (Convert.ToBoolean(_imageProfile.CustomUploadSchema))
                        AppendString("custom_upload_schema=true");

                    break;
                case "push":
                case "multicast":
                    if (Convert.ToBoolean(_imageProfile.SkipExpandVolumes)) AppendString("skip_expand_volumes=true");
                    if (Convert.ToBoolean(_imageProfile.FixBcd)) AppendString("fix_bcd=true");
                    if (Convert.ToBoolean(_imageProfile.FixBootloader)) AppendString("fix_bootloader=true");
                    AppendString("partition_method=" + _imageProfile.PartitionMethod);
                    break;
            }

            return _activeTaskArguments.ToString();
        }
    }
}