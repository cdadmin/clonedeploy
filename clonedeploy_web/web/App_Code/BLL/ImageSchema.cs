using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using BLL.ClientPartitioning;
using Helpers;
using Models.ImageSchema;
using Newtonsoft.Json;
using Partition;

namespace BLL
{

    public class ImageSchema
    {
        private readonly Models.ImageSchema.GridView.Schema _imageSchema;

        public ImageSchema(Models.ImageProfile imageProfile, Models.Image image = null)
        {
            string schema = null;

            //Only To display the main image specs file when not using a profile.
            if (image != null)
            {
                var path = Settings.PrimaryStoragePath + image.Name + Path.DirectorySeparatorChar + "schema";
                if (File.Exists(path))
                {
                    using (StreamReader reader = new StreamReader(path))
                    {
                        schema = reader.ReadLine() ?? "";
                    }
                }
            }

            if (imageProfile != null)
            {
                if (!string.IsNullOrEmpty(imageProfile.CustomSchema))
                {
                    schema = imageProfile.CustomSchema;
                }
                else
                {
                    var path = Settings.PrimaryStoragePath + imageProfile.Image.Name + Path.DirectorySeparatorChar +
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
                _imageSchema = JsonConvert.DeserializeObject<Models.ImageSchema.GridView.Schema>(schema);
            }
        }

        public List<Models.ImageSchema.GridView.HardDrive> GetHardDrivesForGridView()
        {
            if (_imageSchema == null) return null;

            var hardDrives = new List<Models.ImageSchema.GridView.HardDrive>();

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

        public List<Models.ImageSchema.GridView.Partition> GetPartitionsForGridView(string selectedHd)
        {
            var partitions = new List<Models.ImageSchema.GridView.Partition>();

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

        public List<Models.ImageSchema.GridView.LogicalVolume> GetLogicalVolumesForGridView(string selectedHd)
        {
            var lvList = new List<Models.ImageSchema.GridView.LogicalVolume>();

            foreach (var partition in _imageSchema.HardDrives[Convert.ToInt16(selectedHd)].Partitions)
            {
                if (partition.VolumeGroup.Name == null) continue;
                if (partition.VolumeGroup.LogicalVolumes == null) continue;
                var lbs = _imageSchema.HardDrives[Convert.ToInt16(selectedHd)].Lbs;
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

        public static List<Models.ImageSchema.ImageFileInfo> GetPartitionImageFileInfoForGridView(Models.Image image,string selectedHd, string selectedPartition)
        {
            try
            {
                var imageFile =
                    Directory.GetFiles(
                        Settings.PrimaryStoragePath + image.Name + Path.DirectorySeparatorChar + "hd" + (Convert.ToInt16(selectedHd) + 1) +
                        Path.DirectorySeparatorChar,
                        "part" + selectedPartition + ".*").FirstOrDefault();

                var fi = new FileInfo(imageFile);
                var imageFileInfo = new ImageFileInfo
                {
                    FileName = fi.Name,
                    FileSize = (fi.Length/1024f/1024f).ToString("#.##") + " MB"
                };

                return new List<ImageFileInfo> {imageFileInfo};
            }
            catch
            {
                return null;
            }
        }

        public static string ImageSizeOnServerForGridView(string imageName, string hdNumber)
        {
            try
            {
                var imagePath = Settings.PrimaryStoragePath + imageName + Path.DirectorySeparatorChar + "hd" + hdNumber;
                var size = new FileOps().GetDirectorySize(new DirectoryInfo(imagePath)) / 1024f / 1024f / 1024f;
                return Math.Abs(size) < 0.1f ? "< 100M" : size.ToString("#.##") + " GB";
            }
            catch
            {
                return "N/A";
            }
        }

        public static string MinimumClientSizeForGridView(int imageId, int hdNumber)
        {
            try
            {   
                var img = BLL.Image.GetImage(Convert.ToInt32(imageId));
                var fltClientSize = new ClientPartitionHelper(img).HardDrive(hdNumber, 1) / 1024f / 1024f / 1024f;
                return Math.Abs(fltClientSize) < 0.1f ? "< 100M" : fltClientSize.ToString("#.##") + " GB";
            }
            catch
            {
                return "N/A";
            }
        }

        public Models.ImageSchema.GridView.Schema GetImageSchema()
        {
            return _imageSchema;
        }
    }
}

