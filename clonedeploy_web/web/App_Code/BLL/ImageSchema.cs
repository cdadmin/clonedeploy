using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using Helpers;
using Models.ImageSchema;
using Newtonsoft.Json;

namespace BLL
{

    public class ImageSchema
    {
        public static List<Models.ImageSchema.HardDrive> GetHardDrivesForGridView(Models.Image image)
        {
            var imageSchema =
                JsonConvert.DeserializeObject<Models.ImageSchema.ImageSchema>(FileOps.ReadImageSpecs(image.Name)
                    );
            if (imageSchema == null) return null;

            var hardDrives = new List<Models.ImageSchema.HardDrive>();

            foreach (var harddrive in imageSchema.HardDrives)
            {
                var logicalBlockSize = Convert.ToInt16(harddrive.Lbs);
                harddrive.Size = (harddrive.Size*logicalBlockSize/1000/1000/1000);
                hardDrives.Add(harddrive);
            }
            return hardDrives;
        }

        public static List<Models.ImageSchema.Partition> GetPartitionsForGridView(Models.Image image, string selectedHd)
        {
            var imageSchema =
                JsonConvert.DeserializeObject<Models.ImageSchema.ImageSchema>(FileOps.ReadImageSpecs(image.Name)
                    );

            var partitions = new List<Models.ImageSchema.Partition>();

            foreach (var hardDrive in imageSchema.HardDrives.Where(x => x.Name == selectedHd))
            {
                foreach (var part in hardDrive.Partitions)
                {
                    var logicalBlockSize = Convert.ToInt16(hardDrive.Lbs);
                    if ((Convert.ToInt64(part.Size)*logicalBlockSize) < 1048576000)
                        part.Size = part.Size*logicalBlockSize/1024/1024;
                    else
                        part.Size = part.Size*logicalBlockSize/1024/1024/1024;
                    part.UsedMb = part.UsedMb;

                    partitions.Add(part);
                }
            }
            return partitions;
        }

        public static List<Models.ImageSchema.LogicalVolume> GetLogicalVolumesForGridView(Models.Image image, string selectedHd)
        {
            var imageSchema =
                JsonConvert.DeserializeObject<Models.ImageSchema.ImageSchema>(FileOps.ReadImageSpecs(image.Name)
                    );

            var lvList = new List<Models.ImageSchema.LogicalVolume>();

            foreach (var partition in imageSchema.HardDrives[Convert.ToInt16(selectedHd)].Partitions)
            {
                if (partition.VolumeGroup.Name == null) continue;
                if (partition.VolumeGroup.LogicalVolumes == null) continue;
               
                foreach (var lv in partition.VolumeGroup.LogicalVolumes)
                {
                    //if (gvRow.Cells[1].Text != lv.VolumeGroup) continue;
                    var logicalBlockSize = Convert.ToInt64(imageSchema.HardDrives[Convert.ToInt32(selectedHd)].Lbs);
                    if ((Convert.ToInt64(lv.Size) * logicalBlockSize) < 1048576000)
                        lv.Size = lv.Size * logicalBlockSize / 1024 / 1024;
                    else
                        lv.Size =
                            lv.Size * logicalBlockSize / 1024 / 1024 / 1024;
                   
                    
                 


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
    }
}

