using System;
using System.Linq;
using Helpers;
using Models.ClientPartition;
using Models.ImageSchema;
using Newtonsoft.Json;

namespace BLL.ClientPartitioning
{
    /// <summary>
    ///     Calculates the minimum sizes required for various hard drives / partitions for the client imaging process  
    /// </summary>
    public class MinimumSize
    {
        private Models.Image Image { get; set; }
        private Models.ImageSchema.ImageSchema ImageSchema { get; set; }
        public MinimumSize(Models.Image image)
        {
            Image = image;
            ImageSchema = JsonConvert.DeserializeObject<Models.ImageSchema.ImageSchema>(FileOps.ReadImageSpecs(Image.Name)
                    );
        }
       
        /// <summary>
        ///     Calculates the minimum block size for an extended partition by determining the minimum size of all logical
        ///     partitions that fall under the extended.  Does not assume that any extended partitions actually exist.
        /// </summary>
        public ExtendedPartitionHelper ExtendedPartition(int hdNumberToGet)
        {
            var lbsByte = ImageSchema.HardDrives[hdNumberToGet].Lbs;
            var ep = new ExtendedPartitionHelper
            {
                MinSizeBlk = 0,
                IsOnlySwap = false,
                LogicalCount = 0
            };

            var hasExtendedPartition = false;
            var hasLogicalPartition = false;

            //Determine if any Extended or Logical Partitions are present.  Needed ahead of time correctly calculate sizes.
            //And calculate minimum needed extended partition size

            string logicalFsType = null;
            foreach (var part in ImageSchema.HardDrives[hdNumberToGet].Partitions.Where(part => part.Active))
            {
                if (part.Type.ToLower() == "extended")
                    hasExtendedPartition = true;
                if (part.Type.ToLower() != "logical") continue;
                ep.LogicalCount++;
                logicalFsType = part.FsType;
                hasLogicalPartition = true;
                ep.HasLogical = true;
            }

            if (logicalFsType != null && (ep.LogicalCount == 1 && logicalFsType.ToLower() == "swap"))
                ep.IsOnlySwap = true;

            if (hasExtendedPartition)
            {
                var partitionCounter = -1;
                foreach (var partition in ImageSchema.HardDrives[hdNumberToGet].Partitions)
                {
                    partitionCounter++;
                    if (!partition.Active)
                        continue;
                    if (hasLogicalPartition)
                    {
                        if (partition.Type.ToLower() != "logical") continue;

                        //Check if the logical partition is a physical volume for a volume group
                        if (partition.FsId.ToLower() == "8e" || partition.FsId.ToLower() == "8e00")
                        {
                            //Add to the minimum extended size to the minimum size of the volume group
                            var vg = VolumeGroup(hdNumberToGet, partitionCounter);
                            ep.MinSizeBlk += vg.MinSizeBlk;

                            if (vg.MinSizeBlk == 0)
                                ep.MinSizeBlk += partition.Size;
                        }
                        else
                        {
                            //Extended partition size overridden by user
                            if (!string.IsNullOrEmpty(partition.CustomSize))
                                ep.MinSizeBlk += Convert.ToInt64(partition.CustomSize);
                            else
                            {
                                //if logical partition is not resizable use the partition size created during upload
                                if (partition.VolumeSize == 0)
                                    ep.MinSizeBlk += partition.Size;
                                else
                                {
                                    //The resize value and used_mb value are calculated during upload by two different methods
                                    //Use the one that is bigger just in case.
                                    if (partition.VolumeSize > partition.UsedMb)
                                        ep.MinSizeBlk += partition.VolumeSize*1024*1024/lbsByte;
                                    else
                                        ep.MinSizeBlk += partition.UsedMb*1024*1024/lbsByte;
                                }
                            }
                        }
                    }
                    //If Hd has extended but no logical, use the extended to calc size
                    else
                    {
                        //In this case someone has defined an extended partition but has not created any logical
                        //This could just be for preperation of leaving room for more logical partition later
                        //This should be highly unlikely but should account for it anyway.  There is no way of knowing a minimum size required
                        //while still having the partition be resizable.  So will set minimum sized required to unless user overrides
                        if (partition.Type.ToLower() != "extended") continue;

                        //extended partition size set by user
                        if (!string.IsNullOrEmpty(partition.CustomSize))
                            ep.MinSizeBlk = Convert.ToInt64(partition.CustomSize);
                        else
                        //set arbitary minimum to 100MB
                            ep.MinSizeBlk = 100*1024*1024/lbsByte;

                        break; //Like the Highlander there can be only one extended partition
                    }
                }
            }
            //Logical paritions default to 1MB more than the previous block using fdisk. This needs to be added to extended size so logical parts will fit inside
            long epPadding = (((1048576/lbsByte)*ep.LogicalCount) + (1048576/lbsByte));
            ep.MinSizeBlk += epPadding;
            return ep;
        }

        /// <summary>
        ///     Calculates the smallest size hard drive in Bytes that can be used to deploy the image to, based off the data usage.
        ///     The newHdSize parameter is arbitrary but is used to determine if the hard being deployed to is the same size that
        ///     the image was created from.
        /// </summary>
        public long HardDrive(int hdNumberToGet, long newHdSize = 0)
        {
            //For CloneDeploy We refer to each hard drive as hd1, hd2, etc.  Starting at 1, the collections start at 0
            hdNumberToGet = hdNumberToGet - 1;

            long minHdSizeRequiredBlk = 0;
            var lbsByte = ImageSchema.HardDrives[hdNumberToGet].Lbs;

            //if hard drive is the same size as original, then no need to calculate.  It will fit.
            if (ImageSchema.HardDrives[hdNumberToGet].Size*lbsByte == newHdSize)
                return newHdSize;

            var partitionCounter = -1;
            foreach (var partition in ImageSchema.HardDrives[hdNumberToGet].Partitions)
            {
                partitionCounter++;
                //Logical partitions are calculated via the extended
                if (partition.Type.ToLower() == "logical") continue;
                minHdSizeRequiredBlk += this.Partition(hdNumberToGet, partitionCounter).MinSizeBlk;
            }
            return minHdSizeRequiredBlk*lbsByte;
        }

        /// <summary>
        ///     Calculates the minimum block size required for a single logical volume, assuming the logical volume cannot have any
        ///     children.
        /// </summary>
        public PartitionHelper LogicalVolum(LogicalVolume lv, int lbsByte)
        {
            var helper = new PartitionHelper {MinSizeBlk = 0};

            if (!string.IsNullOrEmpty(lv.CustomSize))
            {
                helper.MinSizeBlk = Convert.ToInt64(lv.CustomSize);
                helper.IsResizable = false;
            }

            else if (lv.VolumeSize == 0)
            {
                helper.MinSizeBlk = lv.Size;
                helper.IsResizable = false;
            }

            else
            {
                helper.IsResizable = true;

                if (lv.VolumeSize > lv.UsedMb)
                    helper.MinSizeBlk = lv.VolumeSize*1024*1024/lbsByte;
                else
                    helper.MinSizeBlk = lv.UsedMb*1024*1024/lbsByte;
            }

            return helper;
        }

        /// <summary>
        ///     Calculates the minimum block size required for a single partition, taking into account any children partitions.
        /// </summary>
        public PartitionHelper Partition(int hdNumberToGet, int partNumberToGet)
        {
            var partitionHelper = new PartitionHelper {MinSizeBlk = 0};
            var extendedPartitionHelper = ExtendedPartition(hdNumberToGet);
            var partition = ImageSchema.HardDrives[hdNumberToGet].Partitions[partNumberToGet];
            partitionHelper.VolumeGroupHelper = VolumeGroup(hdNumberToGet, partNumberToGet);
            var lbsByte = ImageSchema.HardDrives[hdNumberToGet].Lbs;


            //Look if any volume groups are present for this partition.  If so set the resize field to the minimum size
            //required for the volume group.  Volume groups are always treated as resizable even if none of the individual 
            //logical volumes are resizable
            if (partitionHelper.VolumeGroupHelper.Pv != null)
            {
                partitionHelper.PartitionHasVolumeGroup = true;
                partition.VolumeSize = (partitionHelper.VolumeGroupHelper.MinSizeBlk*lbsByte/1024/1024);
            }


            //Use partition size that user has set for the partition, if it is set.
            if (!string.IsNullOrEmpty(partition.CustomSize))
            {
                partitionHelper.MinSizeBlk = Convert.ToInt64(partition.CustomSize);
                partitionHelper.IsResizable = false;
            }

            //If partition is not resizable.  Determine partition size.  Also if the partition is less than 2 gigs assume it is that
            // size for a reason, do not resize it even if it is marked as a resizable partition
            else if ((partition.VolumeSize == 0 && partition.Type.ToLower() != "extended") ||
                     (partition.Type.ToLower() == "extended" && extendedPartitionHelper.IsOnlySwap) ||
                     partition.Size*lbsByte <= 2097152000)
            {
                partitionHelper.MinSizeBlk = partition.Size;
                partitionHelper.IsResizable = false;
            }
            //If resizable determine what percent of drive partition was originally and match that to the new drive
            //while making sure the min size is still less than the resized size.
            else
            {
                partitionHelper.IsResizable = true;

                if (partition.Type.ToLower() == "extended")
                    partitionHelper.MinSizeBlk = extendedPartitionHelper.MinSizeBlk;
                else if (partitionHelper.VolumeGroupHelper.Pv != null)
                {
                    partitionHelper.MinSizeBlk = partitionHelper.VolumeGroupHelper.MinSizeBlk;
                }
                else
                {
                    //The resize value and used_mb value are calculated during upload by two different methods
                    //Use the one that is bigger just in case.
                  
                    if (partition.VolumeSize > partition.UsedMb)
                        partitionHelper.MinSizeBlk = partition.VolumeSize*1024*1024/lbsByte;
                    else
                        partitionHelper.MinSizeBlk = (partition.UsedMb*1024*1024)/lbsByte;
                }
            }

            return partitionHelper;
        }

        /// <summary>
        ///     Calculates the minimum block size required for a volume group by determine the size of each
        ///     logical volume within the volume group.  Does not assume that any volume group actually exists.
        /// </summary>
        public VolumeGroupHelper VolumeGroup(int hdNumberToGet, int partNumberToGet)
        {
            var lbsByte = ImageSchema.HardDrives[hdNumberToGet].Lbs;

            var vg = new VolumeGroupHelper
            {
                MinSizeBlk = 0,
                HasLv = false
            };

            if (ImageSchema.HardDrives[hdNumberToGet].Partitions[partNumberToGet].FsId.ToLower() != "8e" &&
                ImageSchema.HardDrives[hdNumberToGet].Partitions[partNumberToGet].FsId.ToLower() != "8e00") return vg;
            if (!ImageSchema.HardDrives[hdNumberToGet].Partitions[partNumberToGet].Active)
                return vg;

            //if part.vg is null, most likely version 2.3.0 beta1 before lvm was added.
            if (ImageSchema.HardDrives[hdNumberToGet].Partitions[partNumberToGet].VolumeGroup == null) return vg;
            //if vg.name is null partition was uploaded at physical partion level, by using the lvmResize=false flag
            if (ImageSchema.HardDrives[hdNumberToGet].Partitions[partNumberToGet].VolumeGroup.Name == null) return vg;
            vg.Name = ImageSchema.HardDrives[hdNumberToGet].Partitions[partNumberToGet].VolumeGroup.Name;
            foreach (var lv in ImageSchema.HardDrives[hdNumberToGet].Partitions[partNumberToGet].VolumeGroup.LogicalVolumes)
            {
                if (!lv.Active)
                    continue;
                vg.HasLv = true;
                vg.Pv = ImageSchema.HardDrives[hdNumberToGet].Partitions[partNumberToGet].VolumeGroup.PhysicalVolume;
                //Logical volume size overridden by user
                if (!string.IsNullOrEmpty(lv.CustomSize))
                    vg.MinSizeBlk += Convert.ToInt64(lv.CustomSize);
                else
                {
                    //If logical volume is not resizable use the actual size of the logical volume during upload
                    if (lv.VolumeSize == 0)
                        vg.MinSizeBlk += lv.Size;
                    else
                    {
                        //The resize value and used_mb value are calculated during upload by two different methods
                        //Use the one that is bigger just in case.
                        if (lv.VolumeSize > lv.UsedMb)
                            vg.MinSizeBlk += lv.VolumeSize*1024*1024/lbsByte;
                        else
                            vg.MinSizeBlk += lv.UsedMb*1024*1024/lbsByte;
                    }
                }
            }

            if (vg.HasLv) return vg;

            //Could Have VG Without LVs
            //Set arbitrary minimum size to 100mb
            vg.Pv = ImageSchema.HardDrives[hdNumberToGet].Partitions[partNumberToGet].VolumeGroup.PhysicalVolume;
            vg.MinSizeBlk = 100*1024*1024/lbsByte;
            return vg;
        }
    }
}