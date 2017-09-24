using System;
using System.Collections.Generic;
using System.Linq;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Entities.DTOs.ImageSchemaFE;
using Newtonsoft.Json;

namespace CloneDeploy_Services
{
    public class ImageSchemaFEServices
    {
        private readonly ImageSchemaGridView _imageSchema;

        public ImageSchemaFEServices(ImageSchemaRequestDTO schemaRequest)
        {
            string schema = null;

            //Only To display the main image specs file when not using a profile.
            if (schemaRequest.image != null)
            {
                schema = new FilesystemServices().ReadSchemaFile(schemaRequest.image.Name);
            }

            if (schemaRequest.imageProfile != null)
            {
                if (!string.IsNullOrEmpty(schemaRequest.imageProfile.CustomSchema) &&
                    schemaRequest.schemaType == "deploy")
                {
                    schema = schemaRequest.imageProfile.CustomSchema;
                }
                else if (!string.IsNullOrEmpty(schemaRequest.imageProfile.CustomUploadSchema) &&
                         schemaRequest.schemaType == "upload")
                {
                    schema = schemaRequest.imageProfile.CustomUploadSchema;
                }
                else
                {
                    schema = new FilesystemServices().ReadSchemaFile(schemaRequest.imageProfile.Image.Name);
                }
            }

            if (!string.IsNullOrEmpty(schema))
            {
                _imageSchema = JsonConvert.DeserializeObject<ImageSchemaGridView>(schema);
            }
        }

        public List<HardDrive> GetHardDrivesForGridView()
        {
            if (_imageSchema == null) return null;

            var hardDrives = new List<HardDrive>();

            foreach (var harddrive in _imageSchema.HardDrives)
            {
                harddrive.Size = (Convert.ToInt64(harddrive.Size)*harddrive.Lbs/1000f/1000f/1000f).ToString("#.##") +
                                 " GB" +
                                 " / " +
                                 (Convert.ToInt64(harddrive.Size)*harddrive.Lbs/1024f/1024f/1024f).ToString("#.##") +
                                 " GB";
                hardDrives
                    .Add(harddrive);
            }
            return hardDrives;
        }

        public ImageSchemaGridView GetImageSchema()
        {
            return _imageSchema;
        }

        public List<LogicalVolume> GetLogicalVolumesForGridView(string selectedHd)
        {
            var lvList = new List<LogicalVolume>();

            foreach (var partition in _imageSchema.HardDrives[Convert.ToInt32(selectedHd)].Partitions)
            {
                if (partition.VolumeGroup.Name == null) continue;
                if (partition.VolumeGroup.LogicalVolumes == null) continue;
                var lbs = _imageSchema.HardDrives[Convert.ToInt32(selectedHd)].Lbs;
                foreach (var lv in partition.VolumeGroup.LogicalVolumes)
                {
                    if (Convert.ToInt64(lv.Size)*lbs < 1048576000)
                        lv.Size = (Convert.ToInt64(lv.Size)*lbs/1024f/1024f).ToString("#.##") +
                                  " MB";
                    else
                        lv.Size =
                            (Convert.ToInt64(lv.Size)*lbs/1024f/1024f/1024f).ToString("#.##") +
                            " GB";
                    lv.UsedMb = lv.UsedMb + " MB";

                    lv.VolumeSize = lv.VolumeSize + " MB";

                    lvList.Add(lv);
                }
            }
            return lvList;
        }

        public List<Partition> GetPartitionsForGridView(string selectedHd)
        {
            var partitions = new List<Partition>();

            foreach (var hardDrive in _imageSchema.HardDrives.Where(x => x.Name == selectedHd))
            {
                foreach (var part in hardDrive.Partitions)
                {
                    if (Convert.ToInt64(part.Size)*hardDrive.Lbs < 1048576000)
                        part.Size = (Convert.ToInt64(part.Size)*hardDrive.Lbs/1024f/1024f).ToString("#.##") + " MB";
                    else
                        part.Size = (Convert.ToInt64(part.Size)*hardDrive.Lbs/1024f/1024f/1024f).ToString("#.##") +
                                    " GB";
                    part.UsedMb = part.UsedMb + " MB";

                    part.VolumeSize = part.VolumeSize + " MB";
                    partitions.Add(part);
                }
            }
            return partitions;
        }
    }
}