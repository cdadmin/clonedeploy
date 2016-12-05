using System;
using System.Linq;

using CloneDeploy_Entities;
using CloneDeploy_Services;

namespace CloneDeploy_App.BLL.Workflows
{
    public class ClientPartitionScript
    {
        public CloneDeploy_Entities.DTOs.ImageSchemaBE.ImageSchema ImageSchema { get; set; }
        public string ClientHd { get; set; }
        public int HdNumberToGet { get; set; }
        public string NewHdSize { get; set; }
        public string TaskType { get; set; }
        public int profileId { get; set; }
        public string partitionPrefix { get; set; }
        public int clientBlockSize { get; set; }
        private ImageProfileEntity imageProfile;
        private CloneDeploy_App.BLL.DynamicClientPartition.ClientPartition clientSchema;
        public string GeneratePartitionScript()
        {
            imageProfile = new ImageProfileServices().ReadProfile(profileId);
            ImageSchema = new CloneDeploy_App.BLL.DynamicClientPartition.ClientPartitionHelper(imageProfile).GetImageSchema();


            clientSchema = new CloneDeploy_App.BLL.DynamicClientPartition.ClientPartition(HdNumberToGet, NewHdSize, imageProfile, partitionPrefix).GenerateClientSchema();
            if (clientSchema == null) return "failed";

            //Handle moving from / to hard drives with different sector sizes ie 512 / 4096
            var activeCounter = HdNumberToGet;          
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
            var LbsByte = Convert.ToInt32(ImageSchema.HardDrives[HdNumberToGet].Lbs); //logical block size in bytes
            if (LbsByte == 512 && clientBlockSize == 4096)
            {
                
                //fix calculations from 512 to 4096
                clientSchema.FirstPartitionStartSector = clientSchema.FirstPartitionStartSector/8;
                clientSchema.ExtendedPartitionHelper.AgreedSizeBlk = clientSchema.ExtendedPartitionHelper.AgreedSizeBlk/
                                                                     8;
                foreach (var partition in clientSchema.PrimaryAndExtendedPartitions)
                {
                    //efi partition on 4k drive cannot be smaller than this, and it is smaller on a 512 drive
                    if (partition.FsId.ToLower() == "ef00")
                        partition.Size = 66560;
                    else
                        partition.Size = partition.Size/8;
                    partition.Start = partition.Size/8;
                }
                                                                                                                                     
                foreach (var partition in clientSchema.LogicalPartitions)
                {
                    partition.Size = partition.Size / 8;
                    partition.Start = partition.Size / 8;
                }

                foreach (var lv in clientSchema.LogicalVolumes)
                {
                    lv.Size = lv.Size / 8;
                }

            }
            else if (LbsByte == 4096 && clientBlockSize == 512)
            {
                //fix calculations from 4096 to 512
                clientSchema.FirstPartitionStartSector = clientSchema.FirstPartitionStartSector * 8;
                clientSchema.ExtendedPartitionHelper.AgreedSizeBlk = clientSchema.ExtendedPartitionHelper.AgreedSizeBlk *
                                                                     8;
                foreach (var partition in clientSchema.PrimaryAndExtendedPartitions)
                {
                    partition.Size = partition.Size * 8;
                    partition.Start = partition.Size * 8;
                }

                foreach (var partition in clientSchema.LogicalPartitions)
                {
                    partition.Size = partition.Size * 8;
                    partition.Start = partition.Size * 8;
                }

                foreach (var lv in clientSchema.LogicalVolumes)
                {
                    lv.Size = lv.Size * 8;
                }
            }

            //otherwise both the original image block size and the destination hard block size are the same, no changes needed
            //End Handle moving from / to hard drives with different sector sizes


            if (imageProfile.Image.Environment == "linux" || string.IsNullOrEmpty(imageProfile.Image.Environment))
            {
                return LinuxLayout();
            }
            else if(imageProfile.Image.Environment == "macOS")
            {
                return OsxNbiLayout();
            }           
            else if (imageProfile.Image.Environment == "winpe")
            {
                return WinPELayout();
            }
            else
            {
                return "failed";
            }
        }

        private string WinPELayout()
        {
            string partitionScript = null;
            if (ImageSchema.HardDrives[HdNumberToGet].Table.ToLower() == "gpt")
            {
                foreach (var partition in clientSchema.PrimaryAndExtendedPartitions)
                {
                    if (partition.Type.ToLower() == "recovery")
                    {
                        partitionScript +=
                            "New-Partition " + ClientHd + " -GptType '{de94bba4-06d1-4d40-a16a-bfd50179d6ac}' -Size " + partition.Size * ImageSchema.HardDrives[HdNumberToGet].Lbs / 1024 / 1024 + "MB | Format-Volume -FileSystem NTFS -NewFileSystemLabel WindowsRE 2>&1 >> $clientLog\r\n";
                    }
                    else if (partition.Type.ToLower() == "system")
                    {
                        long partitionSize = 0;
                        if (clientBlockSize >= 4096 &&
                            (partition.Size*ImageSchema.HardDrives[HdNumberToGet].Lbs/1024/1024) < 260)
                            partitionSize = 260;
                        else
                        {
                            partitionSize = partition.Size*ImageSchema.HardDrives[HdNumberToGet].Lbs/1024/1024;
                        }
                        partitionScript +=
                            "New-Partition " + ClientHd + " -GptType '{c12a7328-f81f-11d2-ba4b-00a0c93ec93b}' -Size " + partitionSize + "MB 2>&1 >> $clientLog\r\n";// | Format-Volume -FileSystem FAT32 -NewFileSystemLabel System\r\n";
                    }
                    else if (partition.Type.ToLower() == "reserved")
                    {
                        partitionScript +=
                            "New-Partition " + ClientHd + " -GptType '{e3c9e316-0b5c-4db8-817d-f92df00215ae}' -Size " + partition.Size * ImageSchema.HardDrives[HdNumberToGet].Lbs / 1024 / 1024 + "MB 2>&1 >> $clientLog\r\n";
                    }
                    else if (partition.Type.ToLower() == "basic")
                    {
                        partitionScript +=
                           "New-Partition " + ClientHd + " -GptType '{ebd0a0a2-b9e5-4433-87c0-68b6b72699c7}' -Size " + partition.Size * ImageSchema.HardDrives[HdNumberToGet].Lbs / 1024 / 1024 + "MB | Format-Volume -FileSystem NTFS 2>&1 >> $clientLog\r\n";
                    }
                }   
            }
            else //mbr
            {
                foreach (var partition in clientSchema.PrimaryAndExtendedPartitions)
                {
                    var isActive = "";
                    if (partition.Number == ImageSchema.HardDrives[HdNumberToGet].Boot)
                        isActive = "-IsActive";
                    partitionScript +=
                          "New-Partition " + ClientHd + " -MbrType " + partition.Type + " -Size " + partition.Size * ImageSchema.HardDrives[HdNumberToGet].Lbs / 1024 / 1024 + "MB " + isActive + " | Format-Volume -FileSystem " + partition.FsType + " 2>&1 >> $clientLog\r\n";
                }
            }

            return partitionScript;
        }
        private string LinuxLayout()
        {
            string partitionScript = null;
            if (TaskType == "debug")
            {
                partitionScript = clientSchema.DebugStatus;
                if (clientSchema.PrimaryAndExtendedPartitions.Count == 0)
                    return partitionScript;
                try
                {
                    clientSchema.ExtendedPartitionHelper.AgreedSizeBlk =
                        clientSchema.ExtendedPartitionHelper.AgreedSizeBlk * clientBlockSize / 1024 / 1024;
                }
                catch
                {
                    // ignored
                }
                foreach (var p in clientSchema.PrimaryAndExtendedPartitions)
                    p.Size = p.Size * clientBlockSize / 1024 / 1024;
                foreach (var p in clientSchema.LogicalPartitions)
                    p.Size = p.Size * clientBlockSize / 1024 / 1024;
                foreach (var p in clientSchema.LogicalVolumes)
                    p.Size = p.Size * clientBlockSize / 1024 / 1024;
            }

            //Create Menu
            if (ImageSchema.HardDrives[HdNumberToGet].Table.ToLower() == "mbr")
            {
                var counter = 0;
                var partCount = clientSchema.PrimaryAndExtendedPartitions.Count;

                string partitionCommands;
                partitionCommands = "fdisk " + ClientHd + " &>>/tmp/clientlog.log <<FDISK\r\n";
                if (clientBlockSize == 512)
                {
                    if (Convert.ToInt32(clientSchema.PrimaryAndExtendedPartitions[0].Start) < 2048)
                        partitionCommands += "c\r\n";
                }
                else if (clientBlockSize == 4096)
                {
                    if (Convert.ToInt32(clientSchema.PrimaryAndExtendedPartitions[0].Start) < 256)
                        partitionCommands += "c\r\n";
                }

                foreach (var part in clientSchema.PrimaryAndExtendedPartitions)
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

                    partitionCommands += part.Number + "\r\n";

                    if (counter == 1)
                        partitionCommands += clientSchema.FirstPartitionStartSector + "\r\n";
                    else
                        partitionCommands += "\r\n";
                    if (part.Type == "extended")
                        partitionCommands += "+" +
                                             (Convert.ToInt64(clientSchema.ExtendedPartitionHelper.AgreedSizeBlk) - 1) +
                                             "\r\n";
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
                    if ((counter == 1 && part.IsBoot) || clientSchema.PrimaryAndExtendedPartitions.Count == 1)
                        partitionCommands += "a\r\n";
                    if (counter != 1 && part.IsBoot)
                    {
                        partitionCommands += "a\r\n";
                        partitionCommands += part.Number + "\r\n";
                    }
                    if ((counter != partCount || clientSchema.LogicalPartitions.Count != 0)) continue;
                    partitionCommands += "w\r\n";
                    partitionCommands += "FDISK\r\n";
                }


                var logicalCounter = 0;
                foreach (var logicalPart in clientSchema.LogicalPartitions)
                {
                    logicalCounter++;
                    partitionCommands += "n\r\n";

                    if (clientSchema.PrimaryAndExtendedPartitions.Count < 4)
                        partitionCommands += "l\r\n";


                    partitionCommands += "\r\n";

                    if (TaskType == "debug")
                        partitionCommands += "+" + (Convert.ToInt64(logicalPart.Size) - (logicalCounter*1)) + "\r\n";
                    else
                    {
                        if (clientBlockSize == 512)
                        partitionCommands += "+" + (Convert.ToInt64(logicalPart.Size) - (logicalCounter*2049)) + "\r\n";
                        else if(clientBlockSize == 4096)
                            partitionCommands += "+" + (Convert.ToInt64(logicalPart.Size) - (logicalCounter * 257)) + "\r\n";
                    }


                    partitionCommands += "t\r\n";

                    partitionCommands += logicalPart.Number + "\r\n";
                    partitionCommands += logicalPart.FsId + "\r\n";

                    if (logicalPart.IsBoot)
                    {
                        partitionCommands += "a\r\n";
                        partitionCommands += logicalPart.Number + "\r\n";
                    }
                    if (logicalCounter != clientSchema.LogicalPartitions.Count) continue;
                    partitionCommands += "w\r\n";
                    partitionCommands += "FDISK\r\n";
                }
                partitionScript += partitionCommands;
            }
            else
            {
                var counter = 0;
                var partCount = clientSchema.PrimaryAndExtendedPartitions.Count;

                var partitionCommands = "gdisk " + ClientHd + " &>>/tmp/clientlog.log <<GDISK\r\n";

                bool isApple = false;
                foreach (var part in clientSchema.PrimaryAndExtendedPartitions)
                {
                    if (part.FsType.Contains("hfs"))
                    {
                        isApple = true;
                        break;
                    }
                }
                //Not sure about this one for 4k native
                if (clientBlockSize == 512)
                {
                    if (clientSchema.FirstPartitionStartSector < 2048 && isApple) //osx cylinder boundary is 8
                    {
                        partitionCommands += "x\r\nl\r\n8\r\nm\r\n";
                    }
                }
                foreach (var part in clientSchema.PrimaryAndExtendedPartitions)
                {
                    counter++;

                    partitionCommands += "n\r\n";

                    partitionCommands += part.Number + "\r\n";
                    if (counter == 1)
                    {
                        if(isApple && clientBlockSize == 4096) //not sure about this one either
                            partitionCommands += "256" + "\r\n";
                        else
                            partitionCommands += clientSchema.FirstPartitionStartSector + "\r\n";

                    }
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
                partitionScript += partitionCommands;
            }


            foreach (var part in from part in ImageSchema.HardDrives[HdNumberToGet].Partitions
                                 where part.Active
                                 where part.VolumeGroup != null
                                 where part.VolumeGroup.LogicalVolumes != null
                                 select part)
            {
                partitionScript += "echo \"pvcreate -u " + part.Uuid + " --norestorefile -yf " +
                                   ClientHd + partitionPrefix +
                                   part.VolumeGroup.PhysicalVolume[part.VolumeGroup.PhysicalVolume.Length - 1] +
                                   "\" >>/tmp/lvmcommands \r\n";
                partitionScript += "echo \"vgcreate " + part.VolumeGroup.Name + " " + ClientHd + partitionPrefix +
                                   part.VolumeGroup.PhysicalVolume[part.VolumeGroup.PhysicalVolume.Length - 1] + " -yf" +
                                   "\" >>/tmp/lvmcommands \r\n";
                partitionScript += "echo \"" + part.VolumeGroup.Uuid + "\" >>/tmp/vg-" + part.VolumeGroup.Name +
                                   " \r\n";
                foreach (var lv in part.VolumeGroup.LogicalVolumes)
                {
                    foreach (var rlv in clientSchema.LogicalVolumes)
                    {
                        if (lv.Name != rlv.Name || lv.VolumeGroup != rlv.Vg) continue;
                        if (TaskType == "debug")
                        {
                            partitionScript += "echo \"lvcreate --yes -L " +
                                               rlv.Size + "mb -n " +
                                               rlv.Name + " " + rlv.Vg +
                                               "\" >>/tmp/lvmcommands \r\n";
                        }
                        else
                        {
                            if (clientBlockSize == 512)
                            {
                                partitionScript += "echo \"lvcreate --yes -L " +
                                                   ((Convert.ToInt64(rlv.Size) - 8192)) + "s -n " +
                                                   rlv.Name + " " + rlv.Vg +
                                                   "\" >>/tmp/lvmcommands \r\n";
                            }
                            else if(clientBlockSize == 4096)
                            {
                                partitionScript += "echo \"lvcreate --yes -L " +
                                                  ((Convert.ToInt64(rlv.Size) - 1024)) + "s -n " +
                                                  rlv.Name + " " + rlv.Vg +
                                                  "\" >>/tmp/lvmcommands \r\n";
                            }
                        }
                        partitionScript += "echo \"" + rlv.Uuid + "\" >>/tmp/" + rlv.Vg +
                                           "-" + rlv.Name + "\r\n";
                    }
                }
                partitionScript += "echo \"vgcfgbackup -f /tmp/lvm-" + part.VolumeGroup.Name +
                                   "\" >>/tmp/lvmcommands\r\n";
            }
            return partitionScript;
        }

        private string OsxNbiLayout()
        {
            string partitionScript = "echo \'diskutil partitionDisk " + ClientHd + " ";
            if (TaskType == "debug")
            {
            
                if (clientSchema.PrimaryAndExtendedPartitions.Count == 0)
                    return partitionScript;
                try
                {
                    clientSchema.ExtendedPartitionHelper.AgreedSizeBlk =
                        clientSchema.ExtendedPartitionHelper.AgreedSizeBlk * clientBlockSize / 1024 / 1024;
                }
                catch
                {
                    // ignored
                }
                foreach (var p in clientSchema.PrimaryAndExtendedPartitions)
                    p.Size = p.Size * clientBlockSize / 1024 / 1024;
                foreach (var p in clientSchema.LogicalPartitions)
                    p.Size = p.Size * clientBlockSize / 1024 / 1024;
                foreach (var p in clientSchema.LogicalVolumes)
                    p.Size = p.Size * clientBlockSize / 1024 / 1024;
            }

            
            var neededPartitionCount = clientSchema.PrimaryAndExtendedPartitions.Count;
            
            foreach (var partition in clientSchema.PrimaryAndExtendedPartitions)
            {
                if (partition.Type.ToLower() == "efi" || partition.Type.ToLower() == "boot os x")
                    neededPartitionCount -= 1;        
            }

            //Add 1 for the free space partition
            neededPartitionCount += 1;
            partitionScript += neededPartitionCount + " ";
            
            foreach (var partition in clientSchema.PrimaryAndExtendedPartitions)
            {
                if (partition.Type.ToLower() == "efi" || partition.Type.ToLower() == "boot os x")
                    continue; //osx automatically creates the efi partition and boot partition if needed

                partitionScript += "\"" + partition.FsType + "\"" + " " + "\"" + partition.Type + "\"" + " " +
                                  partition.Size + "DBS ";
            }

            partitionScript += "\"" + "Free Space" + "\"" + " " + "\"" + "" + "\"" + " " + "R" + " 2>>/tmp/clientlog.log\' > /tmp/createPartitions\n";
                                  
            foreach (var part in from part in ImageSchema.HardDrives[HdNumberToGet].Partitions
                                 where part.Active
                                 where part.VolumeGroup != null
                                 where part.VolumeGroup.LogicalVolumes != null
                                 select part)
            {
                foreach (var lv in part.VolumeGroup.LogicalVolumes)
                {
                    foreach (var rlv in clientSchema.LogicalVolumes)
                    {
                        if (lv.Name != rlv.Name || lv.VolumeGroup != rlv.Vg) continue;

                        partitionScript += "echo \"" + part.VolumeGroup.Name + ":" + ClientHd + partitionPrefix +
                                           part.VolumeGroup.PhysicalVolume[part.VolumeGroup.PhysicalVolume.Length - 1] +
                                           ":" + part.VolumeGroup.Uuid + ":" + rlv.Name +
                                           ":" + rlv.Size * clientBlockSize + ":" + rlv.FsType + ":" + rlv.Uuid + "\" >> /tmp/corestorage\n";
                    }
                }
            }
         
            return partitionScript;
        }
    }



}