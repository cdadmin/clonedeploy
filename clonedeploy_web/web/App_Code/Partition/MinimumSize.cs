using System;
using Models;
using Newtonsoft.Json;

namespace Partition
{
    public class MinimumSize
    {
        public Image Image { get; set; }

        /// <summary>
        ///     Calculates the minimum block size for an extended partition by determining the minimum size of all logical
        ///     partitions that fall under the extended.  Does not assume that any extended partitions actually exist.
        /// </summary>
        public ExtendedPartition ExtendedPartition(int hdNumberToGet)
        {
            var specs = GetImagePhysicalSpecs();

            var lbsByte = Convert.ToInt32(specs.Hd[hdNumberToGet].Lbs);
            var ep = new ExtendedPartition
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
            foreach (var part in specs.Hd[hdNumberToGet].Partition)
            {
                if (part.Active != "1")
                    continue;
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
                foreach (var partition in specs.Hd[hdNumberToGet].Partition)
                {
                    partitionCounter++;
                    if (partition.Active != "1")
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
                                ep.MinSizeBlk += Convert.ToInt64(partition.Size);
                        }
                        else
                        {
                            //Extended partition size overridden by user
                            if (!string.IsNullOrEmpty(partition.Size_Override))
                                ep.MinSizeBlk += Convert.ToInt64(partition.Size_Override);
                            else
                            {
                                //if logical partition is not resizable use the partition size created during upload
                                if (string.IsNullOrEmpty(partition.Resize))
                                    ep.MinSizeBlk += Convert.ToInt64(partition.Size);
                                else
                                {
                                    //The resize value and used_mb value are calculated during upload by two different methods
                                    //Use the one that is bigger just in case.
                                    if (Convert.ToInt64(partition.Resize) > Convert.ToInt64(partition.Used_Mb))
                                        ep.MinSizeBlk += (Convert.ToInt64(partition.Resize)*1024*1024)/lbsByte;
                                    else
                                        ep.MinSizeBlk += (Convert.ToInt64(partition.Used_Mb)*1024*1024)/lbsByte;
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
                        if (!string.IsNullOrEmpty(partition.Size_Override))
                            ep.MinSizeBlk = Convert.ToInt64(partition.Size_Override);
                        else
                        //set arbitary minimum to 100MB
                            ep.MinSizeBlk = (Convert.ToInt64("100")*1024*1024)/lbsByte;

                        break; //Like the Highlander there can be only one extended partition
                    }
                }
            }
            //Logical paritions default to 1MB more than the previous block using fdisk. This needs to be added to extended size so logical parts will fit inside
            long epPadding = (((1048576/lbsByte)*ep.LogicalCount) + (1048576/lbsByte));
            ep.MinSizeBlk += epPadding;
            return ep;
        }

        public ImagePhysicalSpecs GetImagePhysicalSpecs()
        {
            return
                JsonConvert.DeserializeObject<ImagePhysicalSpecs>(!string.IsNullOrEmpty(Image.ClientSizeCustom)
                    ? Image.ClientSizeCustom
                    : Image.ClientSize);
        }

        /// <summary>
        ///     Calculates the smallest size hard drive in Bytes that can be used to deploy the image to, based off the data usage.
        ///     The newHdSize parameter is arbitrary but is used to determine if the hard being deployed to is the same size that
        ///     the image was created from.
        /// </summary>
        public long Hd(int hdNumberToGet, string newHdSize)
        {
            var specs = GetImagePhysicalSpecs();

            long minHdSizeRequiredBlk = 0;
            var lbsByte = Convert.ToInt32(specs.Hd[hdNumberToGet].Lbs);

            //if hard drive is the same size as original, then no need to calculate.  It will fit.
            if (Convert.ToInt64(specs.Hd[hdNumberToGet].Size)*lbsByte == Convert.ToInt64(newHdSize))
                return Convert.ToInt64(newHdSize);

            var partitionCounter = -1;
            foreach (var part in specs.Hd[hdNumberToGet].Partition)
            {
                partitionCounter++;
                //Logical partitions are calculated via the extended
                if (part.Type.ToLower() == "logical") continue;

                var helper = Partition(hdNumberToGet, partitionCounter);

                minHdSizeRequiredBlk += helper.MinSizeBlk;
            }
            return minHdSizeRequiredBlk*lbsByte;
        }

        /// <summary>
        ///     Calculates the minimum block size required for a single logical volume, assuming the logical volume cannot have any
        ///     children.
        /// </summary>
        public PartitionHelper LogicalVolum(LvPhysicalSpecs lv, int lbsByte)
        {
            var helper = new PartitionHelper {MinSizeBlk = 0};

            if (!string.IsNullOrEmpty(lv.Size_Override))
            {
                helper.MinSizeBlk = Convert.ToInt64(lv.Size_Override);
                helper.IsResizable = false;
            }

            else if (string.IsNullOrEmpty(lv.Resize))
            {
                helper.MinSizeBlk = (Convert.ToInt64(lv.Size));
                helper.IsResizable = false;
            }

            else
            {
                helper.IsResizable = true;

                if (Convert.ToInt64(lv.Resize) > Convert.ToInt64(lv.Used_Mb))
                    helper.MinSizeBlk = (Convert.ToInt64(lv.Resize)*1024*1024)/lbsByte;
                else
                    helper.MinSizeBlk = (Convert.ToInt64(lv.Used_Mb)*1024*1024)/lbsByte;
            }

            return helper;
        }

        /// <summary>
        ///     Calculates the minimum block size required for a single partition, taking into account any children partitions.
        /// </summary>
        public PartitionHelper Partition(int hdNumberToGet, int partNumberToGet)
        {
            var specs = GetImagePhysicalSpecs();
            var helper = new PartitionHelper {MinSizeBlk = 0};
            var ep = ExtendedPartition(hdNumberToGet);
            var partition = specs.Hd[hdNumberToGet].Partition[partNumberToGet];
            helper.Vg = VolumeGroup(hdNumberToGet, partNumberToGet);
            var lbsByte = Convert.ToInt32(specs.Hd[hdNumberToGet].Lbs);


            //Look if any volume groups are present for this partition.  If so set the resize field to the minimum size
            //required for the volume group.  Volume groups are always treated as resizable even if none of the individual 
            //logical volumes are resizable
            if (helper.Vg.Pv != null)
            {
                helper.PartitionHasVolumeGroup = true;
                partition.Resize = (helper.Vg.MinSizeBlk*lbsByte/1024/1024).ToString();
            }


            //Use partition size that user has set for the partition, if it is set.
            if (!string.IsNullOrEmpty(partition.Size_Override))
            {
                helper.MinSizeBlk = Convert.ToInt64(partition.Size_Override);
                helper.IsResizable = false;
            }

            //If partition is not resizable.  Determine partition size.  Also if the partition is less than 2 gigs assume it is that
            // size for a reason, do not resize it even if it is marked as a resizable partition
            else if ((string.IsNullOrEmpty(partition.Resize) && partition.Type.ToLower() != "extended") ||
                     (partition.Type.ToLower() == "extended" && ep.IsOnlySwap) ||
                     Convert.ToInt64(partition.Size)*lbsByte <= 2097152000)
            {
                helper.MinSizeBlk = Convert.ToInt64(partition.Size);
                helper.IsResizable = false;
            }
            //If resizable determine what percent of drive partition was originally and match that to the new drive
            //while making sure the min size is still less than the resized size.
            else
            {
                helper.IsResizable = true;

                if (partition.Type.ToLower() == "extended")
                    helper.MinSizeBlk = ep.MinSizeBlk;
                else if (helper.Vg.Pv != null)
                {
                    helper.MinSizeBlk = helper.Vg.MinSizeBlk;
                }
                else
                {
                    //The resize value and used_mb value are calculated during upload by two different methods
                    //Use the one that is bigger just in case.
                    if (string.IsNullOrEmpty(partition.Used_Mb))
                        partition.Used_Mb = "0";
                    if (Convert.ToInt64(partition.Resize) > Convert.ToInt64(partition.Used_Mb))
                        helper.MinSizeBlk = (Convert.ToInt64(partition.Resize)*1024*1024)/lbsByte;
                    else
                        helper.MinSizeBlk = (Convert.ToInt64(partition.Used_Mb)*1024*1024)/lbsByte;
                }
            }

            return helper;
        }

        /// <summary>
        ///     Calculates the minimum block size required for a volume group by determine the size of each
        ///     logical volume within the volume group.  Does not assume that any volume group actually exists.
        /// </summary>
        public VolumeGroup VolumeGroup(int hdNumberToGet, int partNumberToGet)
        {
            var specs = GetImagePhysicalSpecs();
            var lbsByte = Convert.ToInt32(specs.Hd[hdNumberToGet].Lbs);

            var vg = new VolumeGroup
            {
                MinSizeBlk = 0,
                HasLv = false
            };

            if (specs.Hd[hdNumberToGet].Partition[partNumberToGet].FsId.ToLower() != "8e" &&
                specs.Hd[hdNumberToGet].Partition[partNumberToGet].FsId.ToLower() != "8e00") return vg;
            if (specs.Hd[hdNumberToGet].Partition[partNumberToGet].Active != "1")
                return vg;

            //if part.vg is null, most likely version 2.3.0 beta1 before lvm was added.
            if (specs.Hd[hdNumberToGet].Partition[partNumberToGet].Vg == null) return vg;
            //if vg.name is null partition was uploaded at physical partion level, by using the lvmResize=false flag
            if (specs.Hd[hdNumberToGet].Partition[partNumberToGet].Vg.Name == null) return vg;
            vg.Name = specs.Hd[hdNumberToGet].Partition[partNumberToGet].Vg.Name;
            foreach (var lv in specs.Hd[hdNumberToGet].Partition[partNumberToGet].Vg.Lv)
            {
                if (lv.Active != "1")
                    continue;
                vg.HasLv = true;
                vg.Pv = specs.Hd[hdNumberToGet].Partition[partNumberToGet].Vg.Pv;
                //Logical volume size overridden by user
                if (!string.IsNullOrEmpty(lv.Size_Override))
                    vg.MinSizeBlk += Convert.ToInt64(lv.Size_Override);
                else
                {
                    //If logical volume is not resizable use the actual size of the logical volume during upload
                    if (string.IsNullOrEmpty(lv.Resize))
                        vg.MinSizeBlk += Convert.ToInt64(lv.Size);
                    else
                    {
                        //The resize value and used_mb value are calculated during upload by two different methods
                        //Use the one that is bigger just in case.
                        if (Convert.ToInt64(lv.Resize) > Convert.ToInt64(lv.Used_Mb))
                            vg.MinSizeBlk += (Convert.ToInt64(lv.Resize)*1024*1024)/lbsByte;
                        else
                            vg.MinSizeBlk += (Convert.ToInt64(lv.Used_Mb)*1024*1024)/lbsByte;
                    }
                }
            }

            if (vg.HasLv) return vg;

            //Could Have VG Without LVs
            //Set arbitrary minimum size to 100mb
            vg.Pv = specs.Hd[hdNumberToGet].Partition[partNumberToGet].Vg.Pv;
            vg.MinSizeBlk = (Convert.ToInt64("100")*1024*1024)/lbsByte;
            return vg;
        }
    }
}