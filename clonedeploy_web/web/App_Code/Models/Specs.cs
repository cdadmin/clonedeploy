namespace Partition
{
    public class ImagePhysicalSpecs
    {
        public HdPhysicalSpecs[] Hd { get; set; }
        public string Image { get; set; }
    }

    public class HdPhysicalSpecs
    {
        public string Active { get; set; }
        public string Boot { get; set; }
        public string Guid { get; set; }
        public string Lbs { get; set; }
        public string Name { get; set; }
        public PartitionPhysicalSpecs[] Partition { get; set; }
        public string Pbs { get; set; }
        public string Size { get; set; }
        public string Table { get; set; }
    }

    public class PartitionPhysicalSpecs
    {
        public string Active { get; set; }
        public string End { get; set; }
        public string FsId { get; set; }
        public string FsType { get; set; }
        public string Guid { get; set; }
        public string Number { get; set; }
        public string Resize { get; set; }
        public string Size { get; set; }
        public string Size_Override { get; set; }
        public string Start { get; set; }
        public string Type { get; set; }
        public string Used_Mb { get; set; }
        public string Uuid { get; set; }
        public VgPhysicalSpecs Vg { get; set; }
    }

    public class VgPhysicalSpecs
    {
        public LvPhysicalSpecs[] Lv { get; set; }
        public string Name { get; set; }
        public string Pv { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string Uuid { get; set; }
    }

    public class LvPhysicalSpecs
    {
        public string Active { get; set; }
        public string FsType { get; set; }
        public string Name { get; set; }
        public string Resize { get; set; }
        public string Size { get; set; }
        public string Size_Override { get; set; }
        public string Type { get; set; }
        public string Used_Mb { get; set; }
        public string Uuid { get; set; }
        public string Vg { get; set; }
    }


    public class ClientPartition
    {
        public string FsId { get; set; }
        public string FsType { get; set; }
        public string Guid { get; set; }
        public bool IsBoot { get; set; }
        public string Number { get; set; }
        public bool PartitionWasResized { get; set; }
        public string Size { get; set; }
        public string Start { get; set; }
        public string Type { get; set; }
        public string Uuid { get; set; }
    }

    public class ClientLv
    {
        public string FsType { get; set; }
        public string Name { get; set; }
        public bool PartResized { get; set; }
        public string Size { get; set; }
        public string Uuid { get; set; }
        public string Vg { get; set; }
    }

    public class PartitionHelper
    {
        public bool IsResizable { get; set; }
        public long MinSizeBlk { get; set; }
        public bool PartitionHasVolumeGroup { get; set; }
        public VolumeGroup Vg { get; set; }
    }

    public class ExtendedPartition
    {
        public long AgreedSizeBlk { get; set; }
        public bool HasLogical { get; set; }
        public bool IsOnlySwap { get; set; }
        public int LogicalCount { get; set; }
        public long MinSizeBlk { get; set; }
    }

    public class VolumeGroup
    {
        public long AgreedPvSizeBlk { get; set; }
        public bool HasLv { get; set; }
        public long MinSizeBlk { get; set; }
        public string Name { get; set; }
        public string Pv { get; set; }
    }
}