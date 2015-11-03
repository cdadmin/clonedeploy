namespace Models.ClientPartition
{
    public class ClientPartition
    {
        public string FsId { get; set; }
        public string FsType { get; set; }
        public string Guid { get; set; }
        public bool IsBoot { get; set; }
        public string Number { get; set; }
        public bool SizeIsDynamic { get; set; }
        public long Size { get; set; }
        public long Start { get; set; }
        public string Type { get; set; }
        public string Uuid { get; set; }
    }

    public class ClientLogicalVolume
    {
        public string FsType { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public string Uuid { get; set; }
        public string Vg { get; set; }
        public bool SizeIsDynamic { get; set; }
    }

    public class ClientVolumeGroupHelper
    {
        public long AgreedPvSizeBlk { get; set; }
        public bool HasLv { get; set; }
        public long MinSizeBlk { get; set; }
        public string Name { get; set; }
        public string Pv { get; set; }
    }

    public class ExtendedPartitionHelper
    {
        public ExtendedPartitionHelper()
        {
            AgreedSizeBlk = 0;
            HasLogical = false;
            IsOnlySwap = false;
            LogicalCount = 0;
            MinSizeBlk = 0;
        }
        public long AgreedSizeBlk { get; set; }
        public bool HasLogical { get; set; }
        public bool IsOnlySwap { get; set; }
        public int LogicalCount { get; set; }
        public long MinSizeBlk { get; set; }
    }

    public class PartitionHelper
    {
        public bool IsDynamicSize { get; set; }
        public long MinSizeBlk { get; set; }
        public bool PartitionHasVolumeGroup { get; set; }
        public ClientVolumeGroupHelper VolumeGroupHelper { get; set; }
    }
}