using System;
using System.Collections.Generic;
using System.Linq;
using Models.ClientPartition;

namespace BLL.ClientPartitioning
{
    /// <summary>
    /// Summary description for ClientPartition
    /// </summary>
    public class ClientPartition
    {
        private Models.ImageSchema.ImageSchema ImageSchema { get; set; }
        private string BootPart { get; set; }
        //public string ClientHd { get; set; }
        private ExtendedPartitionHelper Ep { get; set; }
        private int HdNumberToGet { get; set; }
        //public string HdToGet { get; set; }
        public Models.Image Image { get; set; }
        private int LbsByte { get; set; }
        //private List<Models.ClientPartition.ClientPartition> LogicalPartitions { get; set; }
        //private List<ClientLv> LogicalVolumes { get; set; }
        private long NewHdBlk { get; set; }
        //public string NewHdSize { get; set; }
        //public string PartitionLayoutText { get; set; }

        private int PartitionSectorStart { get; set; }
       
        //public string TaskType { get; set; }
        private List<VolumeGroupHelper> VolumeGroups { get; set; }

        public ClientPartition()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public List<Models.ClientPartition.ClientPartition> PrimaryAndExtendedPartitionLayout()
        {
            var minimumSize = new MinimumSize(Image);
            Ep = minimumSize.ExtendedPartition(HdNumberToGet);
            Ep.AgreedSizeBlk = 0;


            //Try to determine a layout for each primary or extended partition that will be able to fit logical partitions
            //or logical volumes in.  Also if the partition is logical and is the physical volume for a volume group determine 
            // a size that will work for all logical volumes
            double percentCounter = -1;

            var primaryAndExtendedPartitions = new List<Models.ClientPartition.ClientPartition>();
            var partitionLayoutVerified = false;
            while (!partitionLayoutVerified)
            {
                percentCounter++;
                var isError = false;
                double totalHdPercentage = 0;
                primaryAndExtendedPartitions.Clear();
                VolumeGroups.Clear();
                PartitionSectorStart = Convert.ToInt32(ImageSchema.HardDrives[HdNumberToGet].Partitions[0].Start);
                var partCounter = -1;

                foreach (var schemaPartition in ImageSchema.HardDrives[HdNumberToGet].Partitions)
                {
                    partCounter++;

                    //Determine what sector the first partition should start at
                    if (Convert.ToInt32(schemaPartition.Start) < PartitionSectorStart)
                        PartitionSectorStart = Convert.ToInt32(schemaPartition.Start);

                    if (!schemaPartition.Active)
                        continue;
                    if (schemaPartition.Type.ToLower() == "logical")
                        continue;

                    var clientPartition = new Models.ClientPartition.ClientPartition
                    {
                        IsBoot = BootPart == schemaPartition.Number,
                        Number = schemaPartition.Number,
                        Start = schemaPartition.Start,
                        Type = schemaPartition.Type,
                        FsId = schemaPartition.FsId,
                        Uuid = schemaPartition.Uuid,
                        Guid = schemaPartition.Guid,
                        FsType = schemaPartition.FsType
                    };


                    var partitionHelper = minimumSize.Partition(HdNumberToGet, partCounter);
                    var percentOfHdForThisPartition = (double)partitionHelper.MinSizeBlk / NewHdBlk;
                    long tmpClientPartitionSizeBlk = partitionHelper.MinSizeBlk;


                    if (partitionHelper.IsDynamicSize)
                    {
                        clientPartition.SizeIsDynamic = true;
                        var percentOfOrigDrive = Convert.ToInt64(schemaPartition.Size) /
                                                 (double)(Convert.ToInt64(ImageSchema.HardDrives[HdNumberToGet].Size));

                        //Change the resized partition size based off original percentage and percentCounter loop
                        //This is the active part of the loop that lowers the partition size based on each iteration
                        tmpClientPartitionSizeBlk = percentOfOrigDrive - (percentCounter / 100) <= 0
                            ? Convert.ToInt64(NewHdBlk * (percentOfOrigDrive))
                            : Convert.ToInt64((NewHdBlk * (percentOfOrigDrive - (percentCounter / 100))));

                        //Add the percent of this partition used to the total percent used to make sure we don't go over
                        //100% of the size of the new drive.

                        //Each logical partition requires and extra 1 mb added to the size of the extended partition.
                        if (clientPartition.Type.ToLower() == "extended")
                            percentOfHdForThisPartition = ((double)(tmpClientPartitionSizeBlk) +
                                                           (((1048576 / LbsByte) * Ep.LogicalCount) + (1048576 / LbsByte))) /
                                                          NewHdBlk;
                        else
                            percentOfHdForThisPartition = (double)(tmpClientPartitionSizeBlk) / NewHdBlk;
                    }


                    if (partitionHelper.MinSizeBlk > tmpClientPartitionSizeBlk)
                    {
                        isError = true;
                        break;
                    }

                    if (clientPartition.Type.ToLower() == "extended")
                        Ep.AgreedSizeBlk = tmpClientPartitionSizeBlk;

                    if (partitionHelper.PartitionHasVolumeGroup)
                    {
                        partitionHelper.VolumeGroupHelper.AgreedPvSizeBlk = tmpClientPartitionSizeBlk;
                        VolumeGroups.Add(partitionHelper.VolumeGroupHelper);
                    }

                    clientPartition.Size = tmpClientPartitionSizeBlk;
                    totalHdPercentage += percentOfHdForThisPartition;
                    primaryAndExtendedPartitions.Add(clientPartition);
                }


                //Could not determine a partition layout that works with this hard drive
                if (isError && percentCounter > 99)
                    return null;

                //This partition size doesn't work, continuation of break from earlier
                if (isError)
                    continue;

                if (totalHdPercentage <= 1)
                {
                    //if totalPercentage is < 1 then layout has been verified to work
                    partitionLayoutVerified = true;

                    //If totalPercentage is too far below 1 try to increase size of available resizable partitions
                    long totalAllocatedBlk = 0;
                    var dynamicPartitionCount = 0;
                    if (totalHdPercentage < .95)
                    {
                        foreach (var partition in primaryAndExtendedPartitions)
                        {
                            totalAllocatedBlk += Convert.ToInt64(partition.Size);
                            if (partition.SizeIsDynamic)
                                dynamicPartitionCount++;
                        }
                        var totalUnallocated = NewHdBlk - totalAllocatedBlk;
                        if (dynamicPartitionCount > 0)
                        {
                            foreach (
                                var partition in
                                    primaryAndExtendedPartitions.Where(partition => partition.SizeIsDynamic))
                            {
                                partition.Size = partition.Size + (totalUnallocated / dynamicPartitionCount);
                                if (partition.Type.ToLower() == "extended")
                                    Ep.AgreedSizeBlk = Convert.ToInt64(partition.Size);
                                for (var i = 0; i < VolumeGroups.Count(); i++)
                                    if (ImageSchema.HardDrives[HdNumberToGet].Name + partition.Number == VolumeGroups[i].Pv)
                                        VolumeGroups[i].AgreedPvSizeBlk = Convert.ToInt64(partition.Size);
                            }
                        }
                    }
                }

                //Theoretically should never hit this, but added to prevent infinite loop
                if (percentCounter > 100)
                    return null;
            }

            return primaryAndExtendedPartitions;
        }

        private List<Models.ClientPartition.ClientPartition> LogicalPartitionLayout()
        {
            var minimumSize = new MinimumSize(Image);
            // Try to resize logical to fit inside newly created extended
            double percentCounter = 0;
            var logicalPartLayoutVerified = false;

            while (!logicalPartLayoutVerified)
            {
                var isError = false;
                LogicalPartitions.Clear();
                double totalExtendedPercentage = 0;

                var partCounter = -1;
                foreach (var part in ImageSchema.HardDrives[HdNumberToGet].Partitions)
                {
                    partCounter++;
                    if (part.Type.ToLower() != "logical")
                        continue;

                    var clientPartition = new Models.ClientPartition.ClientPartition
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

                    var logicalPartitionHelper = minimumSize.Partition(HdNumberToGet, partCounter);

                    var percentOfExtendedForThisPartition = (double)logicalPartitionHelper.MinSizeBlk / Ep.AgreedSizeBlk;
                    var tmpClientPartitionSizeBlk = logicalPartitionHelper.MinSizeBlk;

                    if (logicalPartitionHelper.IsDynamicSize)
                    {
                        clientPartition.SizeIsDynamic = true;
                        var percentOfOrigDrive = Convert.ToInt64(part.Size) /
                                                 (double)(Convert.ToInt64(ImageSchema.HardDrives[HdNumberToGet].Size));

                        tmpClientPartitionSizeBlk = percentOfOrigDrive - (percentCounter / 100) <= 0
                            ? Convert.ToInt64(Ep.AgreedSizeBlk * percentOfOrigDrive)
                            : Convert.ToInt64(Ep.AgreedSizeBlk * (percentOfOrigDrive - (percentCounter / 100)));

                        percentOfExtendedForThisPartition = (double)(tmpClientPartitionSizeBlk) /
                                                            Ep.AgreedSizeBlk;
                    }


                    if (logicalPartitionHelper.MinSizeBlk > tmpClientPartitionSizeBlk)
                    {
                        isError = true;
                        break;
                    }

                    totalExtendedPercentage += percentOfExtendedForThisPartition;
                    clientPartition.Size = tmpClientPartitionSizeBlk;

                    LogicalPartitions.Add(clientPartition);

                    if (logicalPartitionHelper.PartitionHasVolumeGroup)
                    {
                        logicalPartitionHelper.VolumeGroupHelper.AgreedPvSizeBlk = tmpClientPartitionSizeBlk;
                        VolumeGroups.Add(logicalPartitionHelper.VolumeGroupHelper);
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
                    var dynamicPartitionCount = 0;
                    //If totalPercentage is too far below 1 try to increase size of available resizable partitions
                    if (totalExtendedPercentage < .95)
                    {
                        foreach (var partition in LogicalPartitions)
                        {
                            totalAllocatedBlk += Convert.ToInt64(partition.Size);
                            if (partition.SizeIsDynamic)
                                dynamicPartitionCount++;
                        }
                        var totalUnallocated = Ep.AgreedSizeBlk - totalAllocatedBlk;
                        if (dynamicPartitionCount > 0)
                        {
                            foreach (
                                var partition in LogicalPartitions.Where(partition => partition.SizeIsDynamic))
                            {
                                partition.Size =
                                    partition.Size + (totalUnallocated / dynamicPartitionCount);
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
    }
}