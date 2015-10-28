using System;
using System.Collections.Generic;
using System.Linq;
using BLL.ClientPartitioning;
using Helpers;
using Models;
using Models.ClientPartition;
using Models.ImageSchema;
using Newtonsoft.Json;
using ClientPartition = Models.ClientPartition.ClientPartition;

namespace Partition
{
    public class CustomComparer : IComparer<long>
    {
        public int Compare(long x, long y)
        {
            return x.CompareTo(y);
        }
    }

    public class ClientLayout
    {
        public ClientLayout()
        {
            PrimaryAndExtendedPartitions = new List<ClientPartition>();
            LogicalPartitions = new List<ClientPartition>();
            LogicalVolumes = new List<ClientLv>();
            VolumeGroups = new List<VolumeGroupHelper>();

            PartitionLayoutVerified = false;
            PartitionSectorStart = 0;
            ImageSchema = JsonConvert.DeserializeObject<Models.ImageSchema.ImageSchema>(FileOps.ReadImageSpecs(Image.Name)
                    );
        }
        private ImageSchema ImageSchema { get; set; }
        private string BootPart { get; set; }
        public string ClientHd { get; set; }
        private ExtendedPartitionHelper Ep { get; set; }
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
        public string TaskType { get; set; }
        private List<VolumeGroupHelper> VolumeGroups { get; set; }

       

        /// <summary>
        ///     Generates the partitioning layout used for the client when restoring an image.  The Flux Capacitor of CWDS.
        /// </summary>
        public void GeneratePartitionLayout()
        {
           

            var activeCounter = Convert.ToInt32(HdToGet);
            HdNumberToGet = Convert.ToInt32(HdToGet) - 1;

            //Look for first active hd
            if (!ImageSchema.HardDrives[HdNumberToGet].Active)
            {
                while (activeCounter <= ImageSchema.HardDrives.Count())
                {
                    if (ImageSchema.HardDrives[activeCounter - 1].Active)
                    {
                        HdNumberToGet = activeCounter - 1;
                    }
                    activeCounter++;
                }
            }

            LbsByte = Convert.ToInt32(ImageSchema.HardDrives[HdNumberToGet].Lbs); //logical block size in bytes
            NewHdBlk = Convert.ToInt64(NewHdSize)/LbsByte; //size of client hard drive in block

            //Change the size of the hard drive being to deployed to to 99% of the drive.  Allow for math errors.
            NewHdSize = (Convert.ToInt64(NewHdSize)*.99).ToString("#");


            //Find the Boot partition
            if (ImageSchema.HardDrives[HdNumberToGet].Boot.Length > 0)
                BootPart = ImageSchema.HardDrives[HdNumberToGet].Boot.Substring(ImageSchema.HardDrives[HdNumberToGet].Boot.Length - 1, 1);

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


            new BLL.ClientPartitioning.PartitionScript().CreateOutput();
        }

        

        public bool LvmPartitionLayout()
        {
            var minimumSize = new MinimumSize(Image);
            //Try to resize lv to fit inside newly created lvm
            foreach (var volumeGroup in VolumeGroups)
            {
                //Tell the volume group it has a size of the physical volume to work with * 99% to account for errors to allow alittle over
                volumeGroup.AgreedPvSizeBlk = Convert.ToInt64(volumeGroup.AgreedPvSizeBlk*.99);
                foreach (var partition in ImageSchema.HardDrives[HdNumberToGet].Partitions)
                {
                    //Find the partition this volume group belongs to
                    if (ImageSchema.HardDrives[HdNumberToGet].Name + partition.Number != volumeGroup.Pv) continue;
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

                            var clientPartitionLv = new ClientLv
                            {
                                Name = lv.Name,
                                Vg = lv.VolumeGroup,
                                Uuid = lv.Uuid,
                                FsType = lv.FsType
                            };


                            var logicalVolumeHelper = minimumSize.LogicalVolume(lv, LbsByte);
                            double percentOfPvForThisLv = (double) logicalVolumeHelper.MinSizeBlk/NewHdBlk;
                            var tmpClientPartitionSizeLvBlk = logicalVolumeHelper.MinSizeBlk;

                            if (logicalVolumeHelper.IsDynamicSize)
                            {
                                clientPartitionLv.SizeIsDynamic = true;
                                var percentOfOrigDrive = Convert.ToInt64(lv.Size)/
                                                         (double) (Convert.ToInt64(ImageSchema.HardDrives[HdNumberToGet].Size));
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
                                        lv.Size = lv.Size + (totalUnallocated/dynamicPartitionCount);
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