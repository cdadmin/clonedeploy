using System;
using System.Collections.Generic;
using System.Linq;
using Models;

namespace Partition
{
    public class CustomComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            return long.Parse(x).CompareTo(long.Parse(y));
        }
    }

    public class ClientLayout
    {
        public ClientLayout()
        {
            PrimaryAndExtendedPartitions = new List<ClientPartition>();
            LogicalPartitions = new List<ClientPartition>();
            LogicalVolumes = new List<ClientLv>();
            VolumeGroups = new List<VolumeGroup>();

            PartitionLayoutVerified = false;
            PartitionSectorStart = 0;
        }

        private string BootPart { get; set; }
        public string ClientHd { get; set; }
        private ExtendedPartition Ep { get; set; }
        private int HdNumberToGet { get; set; }
        public string HdToGet { get; set; }
        public Image Image { get; set; }
        private int LbsByte { get; set; }
        private List<ClientPartition> LogicalPartitions { get; set; }
        private List<ClientLv> LogicalVolumes { get; set; }
        private long NewHdBlk { get; set; }
        public string NewHdSize { get; set; }
        public string PartitionLayoutText { get; set; }
        private bool PartitionLayoutVerified { get; set; }
        private int PartitionSectorStart { get; set; }
        private List<ClientPartition> PrimaryAndExtendedPartitions { get; set; }
        private ImagePhysicalSpecs Specs { get; set; }
        public string TaskType { get; set; }
        private List<VolumeGroup> VolumeGroups { get; set; }

        private void CreateOutput()
        {
            if (TaskType == "debug")
            {
                try
                {
                    Ep.AgreedSizeBlk = Ep.AgreedSizeBlk*512/1024/1024;
                }
                catch
                {
                    // ignored
                }
                foreach (var p in PrimaryAndExtendedPartitions)
                    p.Size = (Convert.ToInt64(p.Size)*512/1024/1024).ToString();
                foreach (var p in LogicalPartitions)
                    p.Size = (Convert.ToInt64(p.Size)*512/1024/1024).ToString();
                foreach (var p in LogicalVolumes)
                    p.Size = (Convert.ToInt64(p.Size)*512/1024/1024).ToString();
            }

            //Create Menu
            if (Specs.Hd[HdNumberToGet].Table.ToLower() == "mbr")
            {
                var counter = 0;
                var partCount = PrimaryAndExtendedPartitions.Count;

                string partitionCommands;
                if (Convert.ToInt32(PrimaryAndExtendedPartitions[0].Start) < 2048)
                    partitionCommands = "fdisk -c=dos " + ClientHd + " &>>/tmp/clientlog.log <<FDISK\r\n";
                else
                    partitionCommands = "fdisk " + ClientHd + " &>>/tmp/clientlog.log <<FDISK\r\n";

                foreach (var part in PrimaryAndExtendedPartitions)
                {
                    counter++;
                    partitionCommands += "n\r\n";
                    switch (part.Type)
                    {
                        case "primary":
                            partitionCommands += "p\r\n";
                            break;
                        case "extended":
                            partitionCommands += "e\r\n";
                            break;
                    }
                    //if (PrimaryAndExtendedPartitions.Count == 1)
                    //{
                        //partitionCommands += "1" + "\r\n";
                    //}
                    //else
                    //{
                        partitionCommands += part.Number + "\r\n";   
                    //}
                    
                    if (counter == 1)
                        partitionCommands += PartitionSectorStart + "\r\n";
                    else
                        partitionCommands += "\r\n";
                    if (part.Type == "extended")
                        partitionCommands += "+" + (Convert.ToInt64(Ep.AgreedSizeBlk) - 1) + "\r\n";
                    else //FDISK seems to include the starting sector in size so we need to subtract 1
                        partitionCommands += "+" + (Convert.ToInt64(part.Size) - 1) + "\r\n";

                    partitionCommands += "t\r\n";
                    if (counter == 1)
                        partitionCommands += part.FsId + "\r\n";
                    else
                    {
                        partitionCommands += part.Number + "\r\n";
                        partitionCommands += part.FsId + "\r\n";
                    }
                    if ((counter == 1 && part.IsBoot) || PrimaryAndExtendedPartitions.Count == 1) 
                        partitionCommands += "a\r\n";
                    if (counter != 1 && part.IsBoot)
                    {
                        partitionCommands += "a\r\n";
                        partitionCommands += part.Number + "\r\n";
                    }
                    if ((counter != partCount || LogicalPartitions.Count != 0)) continue;
                    partitionCommands += "w\r\n";
                    partitionCommands += "FDISK\r\n";
                }


                var logicalCounter = 0;
                foreach (var logicalPart in LogicalPartitions)
                {
                    logicalCounter++;
                    partitionCommands += "n\r\n";

                    if (PrimaryAndExtendedPartitions.Count < 4)
                        partitionCommands += "l\r\n";


                    partitionCommands += "\r\n";

                    if (TaskType == "debug")
                        partitionCommands += "+" + (Convert.ToInt64(logicalPart.Size) - (logicalCounter*1)) + "\r\n";
                    else
                        partitionCommands += "+" + (Convert.ToInt64(logicalPart.Size) - (logicalCounter*2049)) + "\r\n";


                    partitionCommands += "t\r\n";

                    partitionCommands += logicalPart.Number + "\r\n";
                    partitionCommands += logicalPart.FsId + "\r\n";

                    if (logicalPart.IsBoot)
                    {
                        partitionCommands += "a\r\n";
                        partitionCommands += logicalPart.Number + "\r\n";
                    }
                    if (logicalCounter != LogicalPartitions.Count) continue;
                    partitionCommands += "w\r\n";
                    partitionCommands += "FDISK\r\n";
                }
                PartitionLayoutText += partitionCommands;
            }
            else
            {
                var counter = 0;
                var partCount = PrimaryAndExtendedPartitions.Count;

                var partitionCommands = "gdisk " + ClientHd + " &>>/tmp/clientlog.log <<GDISK\r\n";

                bool isApple = false;
                foreach (var part in PrimaryAndExtendedPartitions)
                {
                    if (part.FsType.Contains("hfs"))
                    {
                        isApple = true;
                        break;
                    }
                }
                if (PartitionSectorStart < 2048 && isApple) //osx cylinder boundary is 8
                {
                    partitionCommands += "x\r\nl\r\n8\r\nm\r\n";
                }
                foreach (var part in PrimaryAndExtendedPartitions)
                {
                    counter++;
                   
                    partitionCommands += "n\r\n";

                    partitionCommands += part.Number + "\r\n";
                    if (counter == 1)
                        partitionCommands += PartitionSectorStart + "\r\n";
                    else
                        partitionCommands += "\r\n";
                    //GDISK seems to NOT include the starting sector in size so don't subtract 1 like in FDISK
                    partitionCommands += "+" + Convert.ToInt64(part.Size) + "\r\n";


                    partitionCommands += part.FsId + "\r\n";


                    if ((counter != partCount)) continue;
                    partitionCommands += "w\r\n";
                    partitionCommands += "y\r\n";
                    partitionCommands += "GDISK\r\n";
                }
                PartitionLayoutText += partitionCommands;
            }


            foreach (var part in from part in Specs.Hd[HdNumberToGet].Partition
                where part.Active == "1"
                where part.Vg != null
                where part.Vg.Lv != null
                select part)
            {
                PartitionLayoutText += "echo \"pvcreate -u " + part.Uuid + " --norestorefile -yf " +
                                       ClientHd + part.Vg.Pv[part.Vg.Pv.Length - 1] +
                                       "\" >>/tmp/lvmcommands \r\n";
                PartitionLayoutText += "echo \"vgcreate " + part.Vg.Name + " " + ClientHd +
                                       part.Vg.Pv[part.Vg.Pv.Length - 1] + " -yf" +
                                       "\" >>/tmp/lvmcommands \r\n";
                PartitionLayoutText += "echo \"" + part.Vg.Uuid + "\" >>/tmp/vg-" + part.Vg.Name +
                                       " \r\n";
                foreach (var lv in part.Vg.Lv)
                {
                    foreach (var rlv in LogicalVolumes)
                    {
                        if (lv.Name != rlv.Name || lv.Vg != rlv.Vg) continue;
                        if (TaskType == "debug")
                        {
                            PartitionLayoutText += "echo \"lvcreate -L " +
                                                   rlv.Size + "mb -n " +
                                                   rlv.Name + " " + rlv.Vg +
                                                   "\" >>/tmp/lvmcommands \r\n";
                        }
                        else
                        {
                            PartitionLayoutText += "echo \"lvcreate -L " +
                                                   ((Convert.ToInt64(rlv.Size) - 8192)) + "s -n " +
                                                   rlv.Name + " " + rlv.Vg +
                                                   "\" >>/tmp/lvmcommands \r\n";
                        }
                        PartitionLayoutText += "echo \"" + rlv.Uuid + "\" >>/tmp/" + rlv.Vg +
                                               "-" + rlv.Name + "\r\n";
                    }
                }
                PartitionLayoutText += "echo \"vgcfgbackup -f /tmp/lvm-" + part.Vg.Name +
                                       "\" >>/tmp/lvmcommands\r\n";
            }

            //If mbr / gpt is hybrid, set the boot flag
            /*if (!string.IsNullOrEmpty(BootPart))
            {
                PartitionLayoutText += "fdisk " + ClientHd + " &>>/tmp/clientlog.log <<FDISK\r\n";
                PartitionLayoutText += "x\r\nM\r\nr\r\na\r\n";
                PartitionLayoutText += BootPart + "\r\n";
                PartitionLayoutText += "w\r\nq\r\n";
                PartitionLayoutText += "FDISK\r\n";
            }*/
        }

        /// <summary>
        ///     Generates the partitioning layout used for the client when restoring an image.  The Flux Capacitor of CWDS.
        /// </summary>
        public void GeneratePartitionLayout()
        {
            var minimumSize = new MinimumSize {Image = Image};
            Specs = minimumSize.GetImagePhysicalSpecs();

            var activeCounter = Convert.ToInt32(HdToGet);
            HdNumberToGet = Convert.ToInt32(HdToGet) - 1;

            //Look for first active hd
            if (Specs.Hd[HdNumberToGet].Active != "1")
            {
                while (activeCounter <= Specs.Hd.Count())
                {
                    if (Specs.Hd[activeCounter - 1].Active == "1")
                    {
                        HdNumberToGet = activeCounter - 1;
                    }
                    activeCounter++;
                }
            }

            LbsByte = Convert.ToInt32(Specs.Hd[HdNumberToGet].Lbs); //logical block size in bytes
            NewHdBlk = Convert.ToInt64(NewHdSize)/LbsByte; //size of client hard drive in block

            //Change the size of the hard drive being to deployed to to 99% of the drive.  Allow for math errors.
            NewHdSize = (Convert.ToInt64(NewHdSize)*.99).ToString("#");


            //Find the Boot partition
            if (Specs.Hd[HdNumberToGet].Boot.Length > 0)
                BootPart = Specs.Hd[HdNumberToGet].Boot.Substring(Specs.Hd[HdNumberToGet].Boot.Length - 1, 1);

            if (!PrimaryAndExtendedPartitionLayout())
                return;

            if (Ep.HasLogical)
                if (!LogicalPartitionLayout())
                    return;

            if (VolumeGroups.Any())
                if (!LvmPartitionLayout())
                    return;


            //Order partitions based of block start
            PrimaryAndExtendedPartitions =
                PrimaryAndExtendedPartitions.OrderBy(part => part.Start, new CustomComparer()).ToList();
            LogicalPartitions = LogicalPartitions.OrderBy(part => part.Start, new CustomComparer()).ToList();


            CreateOutput();
        }

        private bool LogicalPartitionLayout()
        {
            var minimumSize = new MinimumSize {Image = Image};
            // Try to resize logical to fit inside newly created extended
            double percentCounter = 0;
            var logicalPartLayoutVerified = false;

            while (!logicalPartLayoutVerified)
            {
                var isError = false;
                LogicalPartitions.Clear();
                double totalExtendedPercentage = 0;

                var partCounter = -1;
                foreach (var part in Specs.Hd[HdNumberToGet].Partition)
                {
                    partCounter++;
                    if (part.Type.ToLower() != "logical")
                        continue;

                    var clientPartition = new ClientPartition
                    {
                        IsBoot = BootPart == part.Number,
                        Number = part.Number,
                        Start = part.Start,
                        Type = part.Type,
                        FsId = part.FsId,
                        Uuid = part.Uuid,
                        Guid = part.Guid,
                        FsType = part.FsType
                    };

                    var helper = minimumSize.Partition(HdNumberToGet, partCounter);

                    var percentOfExtendedForThisPartition = (double) helper.MinSizeBlk/Ep.AgreedSizeBlk;
                    var tmpClientPartitionSizeBlk = helper.MinSizeBlk;

                    if (helper.IsResizable)
                    {
                        var percentOfOrigDrive = Convert.ToInt64(part.Size)/
                                                 (double) (Convert.ToInt64(Specs.Hd[HdNumberToGet].Size));

                        tmpClientPartitionSizeBlk = percentOfOrigDrive - (percentCounter/100) <= 0
                            ? Convert.ToInt64(Ep.AgreedSizeBlk*percentOfOrigDrive)
                            : Convert.ToInt64(Ep.AgreedSizeBlk*(percentOfOrigDrive - (percentCounter/100)));

                        percentOfExtendedForThisPartition = (double) (tmpClientPartitionSizeBlk)/
                                                            Ep.AgreedSizeBlk;
                    }


                    if (helper.MinSizeBlk > tmpClientPartitionSizeBlk)
                    {
                        isError = true;
                        break;
                    }

                    totalExtendedPercentage += percentOfExtendedForThisPartition;
                    clientPartition.Size = tmpClientPartitionSizeBlk.ToString();

                    LogicalPartitions.Add(clientPartition);

                    if (helper.PartitionHasVolumeGroup)
                    {
                        helper.Vg.AgreedPvSizeBlk = tmpClientPartitionSizeBlk;
                        VolumeGroups.Add(helper.Vg);
                    }
                }

                //Could not determine a partition layout that works with this hard drive
                if (isError && percentCounter > 99)
                    return false;

                //This partition size doesn't work, continuation of break from earlier
                if (isError)
                    continue;

                percentCounter++;
                if (totalExtendedPercentage <= 1)
                {
                    long totalAllocatedBlk = 0;
                    var resizablePartsCount = 0;
                    //If totalPercentage is too far below 1 try to increase size of available resizable partitions
                    if (totalExtendedPercentage < .95)
                    {
                        foreach (var partition in LogicalPartitions)
                        {
                            totalAllocatedBlk += Convert.ToInt64(partition.Size);
                            if (partition.PartitionWasResized)
                                resizablePartsCount++;
                        }
                        var totalUnallocated = Ep.AgreedSizeBlk - totalAllocatedBlk;
                        if (resizablePartsCount > 0)
                        {
                            foreach (
                                var partition in LogicalPartitions.Where(partition => partition.PartitionWasResized))
                            {
                                partition.Size =
                                    (Convert.ToInt64(partition.Size) + (totalUnallocated/resizablePartsCount))
                                        .ToString();
                            }
                        }
                    }
                    logicalPartLayoutVerified = true;
                }

                //Theoretically should never hit this, but added to prevent infinite loop
                if (percentCounter > 100)
                {
                    PartitionLayoutText = "Error";
                    break;
                }
            }

            return true;
        }

        public bool LvmPartitionLayout()
        {
            var minimumSize = new MinimumSize {Image = Image};
            //Try to resize lv to fit inside newly created lvm
            foreach (var volumeGroup in VolumeGroups)
            {
                //Tell the volume group it has a size of the physical volume to work with * 99% to account for errors to allow alittle over
                volumeGroup.AgreedPvSizeBlk = Convert.ToInt64(volumeGroup.AgreedPvSizeBlk*.99);
                foreach (var partition in Specs.Hd[HdNumberToGet].Partition)
                {
                    //Find the partition this volume group belongs to
                    if (Specs.Hd[HdNumberToGet].Name + partition.Number != volumeGroup.Pv) continue;
                    var singleLvVerified = false;

                    double percentCounter = -1;

                    while (!singleLvVerified)
                    {
                        percentCounter++;
                        double totalPvPercentage = 0;
                        LogicalVolumes.Clear();
                        if (partition.Active != "1")
                            continue;

                        var isError = false;
                        foreach (var lv in partition.Vg.Lv)
                        {
                            if (lv.Active != "1")
                                continue;

                            var clientPartitionLv = new ClientLv
                            {
                                Name = lv.Name,
                                Vg = lv.Vg,
                                Uuid = lv.Uuid,
                                FsType = lv.FsType
                            };


                            var helper = minimumSize.LogicalVolum(lv, LbsByte);
                            var percentOfPvForThisLv = (double) helper.MinSizeBlk/NewHdBlk;
                            var tmpClientPartitionSizeLvBlk = helper.MinSizeBlk;

                            if (helper.IsResizable)
                            {
                                var percentOfOrigDrive = Convert.ToInt64(lv.Size)/
                                                         (double) (Convert.ToInt64(Specs.Hd[HdNumberToGet].Size));
                                if (percentOfOrigDrive - (percentCounter/100) <= 0)
                                    tmpClientPartitionSizeLvBlk =
                                        Convert.ToInt64(volumeGroup.AgreedPvSizeBlk*percentOfOrigDrive);
                                else
                                    tmpClientPartitionSizeLvBlk =
                                        Convert.ToInt64(volumeGroup.AgreedPvSizeBlk*
                                                        (percentOfOrigDrive - (percentCounter/100)));

                                percentOfPvForThisLv = (double) (tmpClientPartitionSizeLvBlk)/
                                                       volumeGroup.AgreedPvSizeBlk;
                            }


                            if (helper.MinSizeBlk > tmpClientPartitionSizeLvBlk)
                            {
                                isError = true;
                                break;
                            }


                            clientPartitionLv.Size = tmpClientPartitionSizeLvBlk.ToString("#");
                            totalPvPercentage += percentOfPvForThisLv;
                            LogicalVolumes.Add(clientPartitionLv);
                        }

                        //Could not determine a partition layout that works with this hard drive
                        if (isError && percentCounter > 99)
                            return false;

                        //This partition size doesn't work, continuation of break from earlier
                        if (isError)
                            continue;


                        if (totalPvPercentage <= 1)
                        {
                            long totalAllocatedBlk = 0;
                            var resizablePartsCount = 0;
                            //If totalPercentage is too far below 1 try to increase size of available resizable partitions
                            if (totalPvPercentage < .95)
                            {
                                foreach (var lv in LogicalVolumes)
                                {
                                    totalAllocatedBlk += Convert.ToInt64(lv.Size);
                                    if (lv.PartResized)
                                        resizablePartsCount++;
                                }
                                var totalUnallocated = volumeGroup.AgreedPvSizeBlk - totalAllocatedBlk;
                                if (resizablePartsCount > 0)
                                {
                                    foreach (var lv in LogicalVolumes.Where(lv => lv.PartResized))
                                    {
                                        lv.Size =
                                            (Convert.ToInt64(lv.Size) +
                                             (totalUnallocated/resizablePartsCount)).ToString();
                                    }
                                }
                            }
                            singleLvVerified = true;
                        }

                        //Theoretically should never hit this, but added to prevent infinite loop
                        if (percentCounter > 100)
                            return false;
                    }
                }
            }
            return true;
        }

        public bool PrimaryAndExtendedPartitionLayout()
        {
            var minimumSize = new MinimumSize {Image = Image};
            Ep = minimumSize.ExtendedPartition(HdNumberToGet);
            Ep.AgreedSizeBlk = 0;


            //Try to determine a layout for each primary or extended partition that will be able to fit logical partitions
            //or logical volumes in.  Also if the partition is logical and is the physical volume for a volume group determine 
            // a size that will work for all logical volumes
            double percentCounter = -1;
            while (!PartitionLayoutVerified)
            {
                percentCounter++;
                var isError = false;
                double totalHdPercentage = 0;
                PrimaryAndExtendedPartitions.Clear();
                LogicalPartitions.Clear();
                VolumeGroups.Clear();
                PartitionSectorStart = Convert.ToInt32(Specs.Hd[HdNumberToGet].Partition[0].Start);
                var partCounter = -1;

                foreach (var originalPartition in Specs.Hd[HdNumberToGet].Partition)
                {
                    partCounter++;

                    //Determine what sector the first partition should start at
                    if (Convert.ToInt32(originalPartition.Start) < PartitionSectorStart)
                        PartitionSectorStart = Convert.ToInt32(originalPartition.Start);

                    if (originalPartition.Active != "1")
                        continue;
                    if (originalPartition.Type.ToLower() == "logical")
                        continue;

                    var clientPartition = new ClientPartition
                    {
                        IsBoot = BootPart == originalPartition.Number,
                        Number = originalPartition.Number,
                        Start = originalPartition.Start,
                        Type = originalPartition.Type,
                        FsId = originalPartition.FsId,
                        Uuid = originalPartition.Uuid,
                        Guid = originalPartition.Guid,
                        FsType = originalPartition.FsType
                    };


                    var helper = minimumSize.Partition(HdNumberToGet, partCounter);
                    var percentOfHdForThisPartition = (double) helper.MinSizeBlk/NewHdBlk;
                    var tmpClientPartitionSizeBlk = helper.MinSizeBlk;


                    if (helper.IsResizable)
                    {
                        var percentOfOrigDrive = Convert.ToInt64(originalPartition.Size)/
                                                 (double) (Convert.ToInt64(Specs.Hd[HdNumberToGet].Size));

                        //Change the resized partition size based off original percentage and percentCounter loop
                        //This is the active part of the loop that lowers the partition size based on each iteration
                        tmpClientPartitionSizeBlk = percentOfOrigDrive - (percentCounter/100) <= 0
                            ? Convert.ToInt64(NewHdBlk*(percentOfOrigDrive))
                            : Convert.ToInt64((NewHdBlk*(percentOfOrigDrive - (percentCounter/100))));

                        //Add the percent of this partition used to the total percent used to make sure we don't go over
                        //100% of the size of the new drive.

                        //Each logical partition requires and extra 1 mb added to the size of the extended partition.
                        if (clientPartition.Type.ToLower() == "extended")
                            percentOfHdForThisPartition = ((double) (tmpClientPartitionSizeBlk) +
                                                           (((1048576/LbsByte)*Ep.LogicalCount) + (1048576/LbsByte)))/
                                                          NewHdBlk;
                        else
                            percentOfHdForThisPartition = (double) (tmpClientPartitionSizeBlk)/NewHdBlk;
                    }


                    if (helper.MinSizeBlk > tmpClientPartitionSizeBlk)
                    {
                        isError = true;
                        break;
                    }

                    if (clientPartition.Type.ToLower() == "extended")
                        Ep.AgreedSizeBlk = tmpClientPartitionSizeBlk;

                    if (helper.PartitionHasVolumeGroup)
                    {
                        helper.Vg.AgreedPvSizeBlk = tmpClientPartitionSizeBlk;
                        VolumeGroups.Add(helper.Vg);
                    }

                    clientPartition.Size = tmpClientPartitionSizeBlk.ToString("#");
                    totalHdPercentage += percentOfHdForThisPartition;
                    PrimaryAndExtendedPartitions.Add(clientPartition);
                }


                //Could not determine a partition layout that works with this hard drive
                if (isError && percentCounter > 99)
                    return false;

                //This partition size doesn't work, continuation of break from earlier
                if (isError)
                    continue;

                if (totalHdPercentage <= 1)
                {
                    //if totalPercentage is < 1 then layout has been verified to work
                    PartitionLayoutVerified = true;

                    //If totalPercentage is too far below 1 try to increase size of available resizable partitions
                    long totalAllocatedBlk = 0;
                    var resizablePartsCount = 0;
                    if (totalHdPercentage < .95)
                    {
                        foreach (var partition in PrimaryAndExtendedPartitions)
                        {
                            totalAllocatedBlk += Convert.ToInt64(partition.Size);
                            if (partition.PartitionWasResized)
                                resizablePartsCount++;
                        }
                        var totalUnallocated = NewHdBlk - totalAllocatedBlk;
                        if (resizablePartsCount > 0)
                        {
                            foreach (
                                var partition in
                                    PrimaryAndExtendedPartitions.Where(partition => partition.PartitionWasResized))
                            {
                                partition.Size =
                                    (Convert.ToInt64(partition.Size) + (totalUnallocated/resizablePartsCount))
                                        .ToString();
                                if (partition.Type.ToLower() == "extended")
                                    Ep.AgreedSizeBlk = Convert.ToInt64(partition.Size);
                                for (var i = 0; i < VolumeGroups.Count(); i++)
                                    if (Specs.Hd[HdNumberToGet].Name + partition.Number == VolumeGroups[i].Pv)
                                        VolumeGroups[i].AgreedPvSizeBlk = Convert.ToInt64(partition.Size);
                            }
                        }
                    }
                }

                //Theoretically should never hit this, but added to prevent infinite loop
                if (percentCounter > 100)
                    return false;
            }

            return true;
        }
    }
}