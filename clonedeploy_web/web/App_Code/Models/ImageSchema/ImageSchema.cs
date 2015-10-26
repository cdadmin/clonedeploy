using System;

namespace Models.ImageSchema
{
    public class ImageSchema
    {
        public HardDrive[] HardDrives { get; set; }
        public string ImageName { get; set; }
    }

    public class HardDrive
    {
        public bool Active { get; set; }
        public string Boot { get; set; }
        public string Guid { get; set; }
        public short Lbs { get; set; }
        public string Name { get; set; }
        public Partition[] Partitions { get; set; }
        public short Pbs { get; set; }
        public long Size { get; set; }
        public string Table { get; set; }
    }

    public class Partition
    {
        public bool Active { get; set; }
        public long End { get; set; }
        public string FsId { get; set; }
        public string FsType { get; set; }
        public string Guid { get; set; }
        public string Number { get; set; }
        public long? VolumeSize { get; set; }
        public long Size { get; set; }
        public string CustomSize { get; set; }
        public long Start { get; set; }
        public string Type { get; set; }
        public long? UsedMb { get; set; }
        public string Uuid { get; set; }
        public VolumeGroup VolumeGroup { get; set; }
    }

    public class VolumeGroup
    {
        public LogicalVolume[] LogicalVolumes { get; set; }
        public string Name { get; set; }
        public string PhysicalVolume { get; set; }
        public long Size { get; set; }
        public string Type { get; set; }
        public string Uuid { get; set; }
    }

    public class LogicalVolume
    {
        public bool Active { get; set; }
        public string FsType { get; set; }
        public string Name { get; set; }
        public long? VolumeSize { get; set; }
        public long Size { get; set; }
        public string CustomSize { get; set; }
        public string Type { get; set; }
        public long? UsedMb { get; set; }
        public string Uuid { get; set; }
        public string VolumeGroup { get; set; }
    }


    public class ClientPartition
    {
        public string FsId { get; set; }
        public string FsType { get; set; }
        public string Guid { get; set; }
        public bool IsBoot { get; set; }
        public string Number { get; set; }
        public bool PartitionWasResized { get; set; }
        public long Size { get; set; }
        public long Start { get; set; }
        public string Type { get; set; }
        public string Uuid { get; set; }
    }

    public class ClientLv
    {
        public string FsType { get; set; }
        public string Name { get; set; }
        public bool PartResized { get; set; }
        public long Size { get; set; }
        public string Uuid { get; set; }
        public string Vg { get; set; }
    }

    public class PartitionHelper
    {
        public bool IsResizable { get; set; }
        public long? MinSizeBlk { get; set; }
        public bool PartitionHasVolumeGroup { get; set; }
        public VolumeGroupHelper Vg { get; set; }
    }

    public class ExtendedPartitionHelper
    {
        public long AgreedSizeBlk { get; set; }
        public bool HasLogical { get; set; }
        public bool IsOnlySwap { get; set; }
        public int LogicalCount { get; set; }
        public long? MinSizeBlk { get; set; }
    }

    public class VolumeGroupHelper
    {
        public long AgreedPvSizeBlk { get; set; }
        public bool HasLv { get; set; }
        public long? MinSizeBlk { get; set; }
        public string Name { get; set; }
        public string Pv { get; set; }
    }

    public class ImageFileInfo
    {
        public string FileName { get; set; }
        public string FileSize { get; set; }
    }
}