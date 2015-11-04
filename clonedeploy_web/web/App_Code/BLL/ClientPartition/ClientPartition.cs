using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Helpers;
using Models.ClientPartition;
using Newtonsoft.Json;

namespace BLL.ClientPartitioning
{
    public class CustomComparer : IComparer<long>
    {
        public int Compare(long x, long y)
        {
            return x.CompareTo(y);
        }
    }

    public class ClientPartition
    {
        public ClientPartition(int hdToGet,string newHdSize, Models.ImageProfile imageProfile)
        {
            _hdToGet = hdToGet;
            _newHdSize = newHdSize;
            _imageProfile = imageProfile;
            PrimaryAndExtendedPartitions = new List<Models.ClientPartition.ClientPartition>();
            LogicalPartitions = new List<Models.ClientPartition.ClientPartition>();
            LogicalVolumes = new List<ClientLogicalVolume>();
            VolumeGroupHelpers = new List<ClientVolumeGroupHelper>();
            _imageSchema = new ClientPartitionHelper(_imageProfile).GetImageSchema();
        }

        private readonly Models.ImageSchema.ImageSchema _imageSchema;
        private string BootPart { get; set; }
        private int HdNumberToGet { get; set; }

        private readonly int _hdToGet;
        private readonly Models.ImageProfile _imageProfile;
        private string _newHdSize;

        private int LbsByte { get; set; }    
        private long NewHdBlk { get; set; }
       

        public int FirstPartitionStartSector { get; set; }
        public List<Models.ClientPartition.ClientPartition> PrimaryAndExtendedPartitions { get; set; }
        public List<Models.ClientPartition.ClientPartition> LogicalPartitions { get; set; }
        public List<ClientVolumeGroupHelper> VolumeGroupHelpers { get; set; }
        public List<Models.ClientPartition.ClientLogicalVolume> LogicalVolumes { get; set; }
        public ExtendedPartitionHelper ExtendedPartitionHelper { get; set; }
        public string DebugStatus { get; set; }
        /// <summary>
        ///     Generates the partitioning layout used for the client when restoring an image.
        /// </summary>
        public ClientPartition GenerateClientSchema()
        {
            var activeCounter = _hdToGet;
            HdNumberToGet = _hdToGet - 1;

            //Look for first active hd
            if (!_imageSchema.HardDrives[HdNumberToGet].Active)
            {
                while (activeCounter <= _imageSchema.HardDrives.Count())
                {
                    if (_imageSchema.HardDrives[activeCounter - 1].Active)
                    {
                        HdNumberToGet = activeCounter - 1;
                    }
                    activeCounter++;
                }
            }

            LbsByte = Convert.ToInt32(_imageSchema.HardDrives[HdNumberToGet].Lbs); //logical block size in bytes
            NewHdBlk = Convert.ToInt64(_newHdSize) / LbsByte; //size of client hard drive in block

            //Change the size of the hard drive being to deployed to to 99% of the drive.  Allow for math errors.
            //_newHdSize = (Convert.ToInt64(_newHdSize) * .99).ToString("#");


            //Find the Boot partition
            if (_imageSchema.HardDrives[HdNumberToGet].Boot.Length > 0)
                BootPart = _imageSchema.HardDrives[HdNumberToGet].Boot.Substring(_imageSchema.HardDrives[HdNumberToGet].Boot.Length - 1, 1);

            if (!PrimaryAndExtendedPartitionLayout())
                return this;

            if (ExtendedPartitionHelper.HasLogical)
                if (!LogicalPartitionLayout())
                    return null;

            if (VolumeGroupHelpers.Any())
                if (!LogicalVolumeLayout())
                    return null;


            //Order partitions based of block start
            PrimaryAndExtendedPartitions =
                PrimaryAndExtendedPartitions.OrderBy(part => part.Start, new CustomComparer()).ToList();
            LogicalPartitions = LogicalPartitions.OrderBy(part => part.Start, new CustomComparer()).ToList();

            return this;
            //new BLL.ClientPartitioning.PartitionScript().CreateOutput();
        }

        private bool PrimaryAndExtendedPartitionLayout()
        {

            //Try to determine a layout for each primary or extended partition that will be able to fit logical partitions
            //or logical volumes in.  Also if the partition is logical and is the physical volume for a volume group determine 
            // a size that will work for all logical volumes
            ExtendedPartitionHelper = new ClientPartitionHelper(_imageProfile).ExtendedPartition(HdNumberToGet);


            double percentCounter = -1;
            var partitionLayoutVerified = false;
            while (!partitionLayoutVerified)
            {
                percentCounter++;
                var isError = false;
                double totalHdPercentage = 0;
                PrimaryAndExtendedPartitions.Clear();
                VolumeGroupHelpers.Clear();
                FirstPartitionStartSector = Convert.ToInt32(_imageSchema.HardDrives[HdNumberToGet].Partitions[0].Start);
                var partCounter = -1;

                foreach (var schemaPartition in _imageSchema.HardDrives[HdNumberToGet].Partitions)
                {
                    partCounter++;

                    //Determine what sector the first partition should start at
                    if (Convert.ToInt32(schemaPartition.Start) < FirstPartitionStartSector)
                        FirstPartitionStartSector = Convert.ToInt32(schemaPartition.Start);

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


                    var partitionHelper = new ClientPartitionHelper(_imageProfile).Partition(HdNumberToGet, partCounter);
                    var percentOfHdForThisPartition = (double)partitionHelper.MinSizeBlk / NewHdBlk;
                    long tmpClientPartitionSizeBlk = partitionHelper.MinSizeBlk;


                    if (partitionHelper.IsDynamicSize)
                    {
                        clientPartition.SizeIsDynamic = true;
                        var percentOfOrigDrive = Convert.ToInt64(schemaPartition.Size) /
                                                 (double)(Convert.ToInt64(_imageSchema.HardDrives[HdNumberToGet].Size));

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
                                                           (((1048576 / LbsByte) * ExtendedPartitionHelper.LogicalCount) + (1048576 / LbsByte))) /
                                                          NewHdBlk;
                        else
                            percentOfHdForThisPartition = (double)(tmpClientPartitionSizeBlk) / NewHdBlk;
                    }

                    DebugStatus += tmpClientPartitionSizeBlk + "\r\n";
                    if (partitionHelper.MinSizeBlk > tmpClientPartitionSizeBlk)
                    {
                        isError = true;
                        break;
                    }

                    if (clientPartition.Type.ToLower() == "extended")
                        ExtendedPartitionHelper.AgreedSizeBlk = tmpClientPartitionSizeBlk;

                    if (partitionHelper.PartitionHasVolumeGroup)
                    {
                        partitionHelper.VolumeGroupHelper.AgreedPvSizeBlk = tmpClientPartitionSizeBlk;
                        VolumeGroupHelpers.Add(partitionHelper.VolumeGroupHelper);
                    }

                    clientPartition.Size = tmpClientPartitionSizeBlk;
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
                    partitionLayoutVerified = true;

                    //If totalPercentage is too far below 1 try to increase size of available resizable partitions
                    long totalAllocatedBlk = 0;
                    var dynamicPartitionCount = 0;
                    if (totalHdPercentage < .98)
                    {
                        foreach (var partition in PrimaryAndExtendedPartitions)
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
                                    PrimaryAndExtendedPartitions.Where(partition => partition.SizeIsDynamic))
                            {
                                partition.Size = partition.Size + (totalUnallocated / dynamicPartitionCount);
                                if (partition.Type.ToLower() == "extended")
                                    ExtendedPartitionHelper.AgreedSizeBlk = Convert.ToInt64(partition.Size);
                                for (var i = 0; i < VolumeGroupHelpers.Count(); i++)
                                    if (_imageSchema.HardDrives[HdNumberToGet].Name + partition.Number == VolumeGroupHelpers[i].Pv)
                                        VolumeGroupHelpers[i].AgreedPvSizeBlk = Convert.ToInt64(partition.Size);
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

        private bool LogicalPartitionLayout()
        {
            // Try to resize logical to fit inside newly created extended
            double percentCounter = 0;

            var logicalPartLayoutVerified = false;
            while (!logicalPartLayoutVerified)
            {
                var isError = false;
                LogicalPartitions.Clear();
                double totalExtendedPercentage = 0;

                var partCounter = -1;
                foreach (var part in _imageSchema.HardDrives[HdNumberToGet].Partitions)
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

                    var logicalPartitionHelper = new ClientPartitionHelper(_imageProfile).Partition(HdNumberToGet, partCounter);

                    var percentOfExtendedForThisPartition = (double)logicalPartitionHelper.MinSizeBlk / ExtendedPartitionHelper.AgreedSizeBlk;
                    var tmpClientPartitionSizeBlk = logicalPartitionHelper.MinSizeBlk;

                    if (logicalPartitionHelper.IsDynamicSize)
                    {
                        clientPartition.SizeIsDynamic = true;
                        var percentOfOrigDrive = Convert.ToInt64(part.Size) /
                                                 (double)(Convert.ToInt64(_imageSchema.HardDrives[HdNumberToGet].Size));

                        tmpClientPartitionSizeBlk = percentOfOrigDrive - (percentCounter / 100) <= 0
                            ? Convert.ToInt64(ExtendedPartitionHelper.AgreedSizeBlk * percentOfOrigDrive)
                            : Convert.ToInt64(ExtendedPartitionHelper.AgreedSizeBlk * (percentOfOrigDrive - (percentCounter / 100)));

                        percentOfExtendedForThisPartition = (double)(tmpClientPartitionSizeBlk) /
                                                            ExtendedPartitionHelper.AgreedSizeBlk;
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
                        VolumeGroupHelpers.Add(logicalPartitionHelper.VolumeGroupHelper);
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
                        var totalUnallocated = ExtendedPartitionHelper.AgreedSizeBlk - totalAllocatedBlk;
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
                    return false;
                }
            }

            return true;
        }

        private bool LogicalVolumeLayout()
        {
            //Try to resize lv to fit inside newly created lvm
            foreach (var volumeGroup in VolumeGroupHelpers)
            {
                //Tell the volume group it has a size of the physical volume to work with * 99% to account for errors to allow alittle over
                volumeGroup.AgreedPvSizeBlk = Convert.ToInt64(volumeGroup.AgreedPvSizeBlk * .99);
                foreach (var partition in _imageSchema.HardDrives[HdNumberToGet].Partitions)
                {
                    //Find the partition this volume group belongs to
                    if (_imageSchema.HardDrives[HdNumberToGet].Name + partition.Number != volumeGroup.Pv) continue;
                    var singleLvVerified = false;

                    double percentCounter = -1;

                  
                    while (!singleLvVerified)
                    {
                        percentCounter++;
                        double totalPvPercentage = 0;
                        LogicalVolumes.Clear();
                        if (!partition.Active)
                            continue;

                        var isError = false;
                        foreach (var lv in partition.VolumeGroup.LogicalVolumes)
                        {
                            if (!lv.Active)
                                continue;

                            var clientPartitionLv = new ClientLogicalVolume
                            {
                                Name = lv.Name,
                                Vg = lv.VolumeGroup,
                                Uuid = lv.Uuid,
                                FsType = lv.FsType
                            };


                            var logicalVolumeHelper = new ClientPartitionHelper(_imageProfile).LogicalVolume(lv, LbsByte);
                            double percentOfPvForThisLv = (double)logicalVolumeHelper.MinSizeBlk / NewHdBlk;
                            var tmpClientPartitionSizeLvBlk = logicalVolumeHelper.MinSizeBlk;

                            if (logicalVolumeHelper.IsDynamicSize)
                            {
                                clientPartitionLv.SizeIsDynamic = true;
                                var percentOfOrigDrive = Convert.ToInt64(lv.Size) /
                                                         (double)(Convert.ToInt64(_imageSchema.HardDrives[HdNumberToGet].Size));
                                if (percentOfOrigDrive - (percentCounter / 100) <= 0)
                                    tmpClientPartitionSizeLvBlk =
                                        Convert.ToInt64(volumeGroup.AgreedPvSizeBlk * percentOfOrigDrive);
                                else
                                    tmpClientPartitionSizeLvBlk =
                                        Convert.ToInt64(volumeGroup.AgreedPvSizeBlk *
                                                        (percentOfOrigDrive - (percentCounter / 100)));

                                percentOfPvForThisLv = (double)(tmpClientPartitionSizeLvBlk) /
                                                       volumeGroup.AgreedPvSizeBlk;
                            }


                            if (logicalVolumeHelper.MinSizeBlk > tmpClientPartitionSizeLvBlk)
                            {
                                isError = true;
                                break;
                            }


                            clientPartitionLv.Size = tmpClientPartitionSizeLvBlk;
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
                            var dynamicPartitionCount = 0;
                            //If totalPercentage is too far below 1 try to increase size of available resizable partitions
                            if (totalPvPercentage < .95)
                            {
                                foreach (var lv in LogicalVolumes)
                                {
                                    totalAllocatedBlk += Convert.ToInt64(lv.Size);
                                    if (lv.SizeIsDynamic)
                                        dynamicPartitionCount++;
                                }
                                var totalUnallocated = volumeGroup.AgreedPvSizeBlk - totalAllocatedBlk;
                                if (dynamicPartitionCount > 0)
                                {
                                    foreach (var lv in LogicalVolumes.Where(lv => lv.SizeIsDynamic))
                                    {
                                        lv.Size = lv.Size + (totalUnallocated / dynamicPartitionCount);
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
    }
}