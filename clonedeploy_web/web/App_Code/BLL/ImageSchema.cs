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
        public static List<Models.ImageSchema.GridView.HardDrive> GetHardDrivesForGridView(Models.Image image)
        {
            var imageSchema =
                JsonConvert.DeserializeObject<Models.ImageSchema.GridView.Schema>(FileOps.ReadImageSpecs(image.Name)
                    );
            if (imageSchema == null) return null;

            var hardDrives = new List<Models.ImageSchema.GridView.HardDrive>();

            foreach (var harddrive in imageSchema.HardDrives)
            {
                harddrive.Size = (Convert.ToInt64(harddrive.Size) * harddrive.Lbs / 1000f / 1000f / 1000f).ToString("#.##") + " GB" +
                          " / " + (Convert.ToInt64(harddrive.Size) * harddrive.Lbs / 1024f / 1024f / 1024f).ToString("#.##") +
                          " GB";
                hardDrives
                    .Add(harddrive);
            }
            return hardDrives;
        }

        public static List<Models.ImageSchema.GridView.Partition> GetPartitionsForGridView(Models.Image image, string selectedHd)
        {
            var imageSchema =
                JsonConvert.DeserializeObject<Models.ImageSchema.GridView.Schema>(FileOps.ReadImageSpecs(image.Name)
                    );

            var partitions = new List<Models.ImageSchema.GridView.Partition>();

            foreach (var hardDrive in imageSchema.HardDrives.Where(x => x.Name == selectedHd))
            {
                foreach (var part in hardDrive.Partitions)
                {
                    if ((Convert.ToInt64(part.Size) * hardDrive.Lbs) < 1048576000)
                        part.Size = (Convert.ToInt64(part.Size) * hardDrive.Lbs / 1024f / 1024f).ToString("#.##") + " MB";
                    else
                        part.Size = (Convert.ToInt64(part.Size) * hardDrive.Lbs / 1024f / 1024f / 1024f).ToString("#.##") +
                                    " GB";
                    part.UsedMb = part.UsedMb + " MB";
                    if (!string.IsNullOrEmpty(part.CustomSize))
                        part.CustomSize =
                            (Convert.ToInt64(part.CustomSize) * hardDrive.Lbs / 1024f / 1024f).ToString(
                                CultureInfo.InvariantCulture);
                  
                    part.VolumeSize = part.VolumeSize + " MB";
                    partitions.Add(part);

                 
                }
            }
            return partitions;
        }

        public static List<Models.ImageSchema.GridView.LogicalVolume> GetLogicalVolumesForGridView(Models.Image image, string selectedHd)
        {
            var imageSchema =
                JsonConvert.DeserializeObject<Models.ImageSchema.GridView.Schema>(FileOps.ReadImageSpecs(image.Name)
                    );

            var lvList = new List<Models.ImageSchema.GridView.LogicalVolume>();

            foreach (var partition in imageSchema.HardDrives[Convert.ToInt16(selectedHd)].Partitions)
            {
                if (partition.VolumeGroup.Name == null) continue;
                if (partition.VolumeGroup.LogicalVolumes == null) continue;
                var lbs = imageSchema.HardDrives[Convert.ToInt16(selectedHd)].Lbs;
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
                    if (!string.IsNullOrEmpty(lv.CustomSize))
                        lv.CustomSize =
                            (Convert.ToInt64(lv.CustomSize) * lbs / 1024f / 1024f).ToString(
                                CultureInfo.InvariantCulture);
                   
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
                var fltClientSize = new MinimumSize(img).HardDrive(hdNumber, 1) / 1024f / 1024f / 1024f;
                return Math.Abs(fltClientSize) < 0.1f ? "< 100M" : fltClientSize.ToString("#.##") + " GB";
            }
            catch
            {
                return "N/A";
            }
        }
    }
}

