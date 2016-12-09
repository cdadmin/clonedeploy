using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Entities.DTOs.ImageSchemaFE;
using CloneDeploy_Services.Helpers;
using Newtonsoft.Json;
using HardDrive = CloneDeploy_Entities.DTOs.ImageSchemaFE.HardDrive;
using LogicalVolume = CloneDeploy_Entities.DTOs.ImageSchemaFE.LogicalVolume;
using Partition = CloneDeploy_Entities.DTOs.ImageSchemaFE.Partition;

namespace CloneDeploy_Services
{

    public class ImageSchemaFEServices
    {
        private readonly  ImageSchemaGridView _imageSchema;

        public ImageSchemaFEServices(ImageSchemaRequestDTO schemaRequest)
        {
            string schema = null;

            //Only To display the main image specs file when not using a profile.
            if (schemaRequest.image != null)
            {
                var path = Settings.PrimaryStoragePath + "images" + Path.DirectorySeparatorChar + schemaRequest.image.Name + Path.DirectorySeparatorChar + "schema";
                if (File.Exists(path))
                {
                    using (StreamReader reader = new StreamReader(path))
                    {
                        schema = reader.ReadLine() ?? "";
                    }
                }
            }

            if (schemaRequest.imageProfile != null)
            {
                if (!string.IsNullOrEmpty(schemaRequest.imageProfile.CustomSchema) && schemaRequest.schemaType == "deploy")
                {
                    schema = schemaRequest.imageProfile.CustomSchema;
                }
                else if (!string.IsNullOrEmpty(schemaRequest.imageProfile.CustomUploadSchema) && schemaRequest.schemaType == "upload")
                {
                    schema = schemaRequest.imageProfile.CustomUploadSchema;
                }
                else
                {
                    var path = Settings.PrimaryStoragePath + "images" + Path.DirectorySeparatorChar + schemaRequest.imageProfile.Image.Name + Path.DirectorySeparatorChar +
                               "schema";
                    if (File.Exists(path))
                    {
                        using (StreamReader reader = new StreamReader(path))
                        {
                            schema = reader.ReadLine() ?? "";
                        }
                    }
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
                harddrive.Size = (Convert.ToInt64(harddrive.Size) * harddrive.Lbs / 1000f / 1000f / 1000f).ToString("#.##") + " GB" +
                          " / " + (Convert.ToInt64(harddrive.Size) * harddrive.Lbs / 1024f / 1024f / 1024f).ToString("#.##") +
                          " GB";
                hardDrives
                    .Add(harddrive);
            }
            return hardDrives;
        }

        public List<Partition> GetPartitionsForGridView(string selectedHd)
        {
            var partitions = new List<Partition>();

            foreach (var hardDrive in _imageSchema.HardDrives.Where(x => x.Name == selectedHd))
            {
                foreach (var part in hardDrive.Partitions)
                {
                    if ((Convert.ToInt64(part.Size) * hardDrive.Lbs) < 1048576000)
                        part.Size = (Convert.ToInt64(part.Size) * hardDrive.Lbs / 1024f / 1024f).ToString("#.##") + " MB";
                    else
                        part.Size = (Convert.ToInt64(part.Size) * hardDrive.Lbs / 1024f / 1024f / 1024f).ToString("#.##") +
                                    " GB";
                    part.UsedMb = part.UsedMb + " MB";
                    
                  
                    part.VolumeSize = part.VolumeSize + " MB";
                    partitions.Add(part);

                 
                }
            }
            return partitions;
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
                    if ((Convert.ToInt64(lv.Size) * lbs ) < 1048576000)
                        lv.Size = (Convert.ToInt64(lv.Size) * lbs / 1024f / 1024f).ToString("#.##") +
                                  " MB";
                    else
                        lv.Size =
                            (Convert.ToInt64(lv.Size) * lbs / 1024f / 1024f / 1024f).ToString("#.##") +
                            " GB";
                    lv.UsedMb = lv.UsedMb + " MB";
                   
                   
                        lv.VolumeSize = lv.VolumeSize + " MB";

                    lvList.Add(lv);
                }
            }
            return lvList;
        }

       

        
       

      

        public ImageSchemaGridView GetImageSchema()
        {
            return _imageSchema;
        }
    }
}

