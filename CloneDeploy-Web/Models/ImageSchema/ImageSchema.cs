namespace Models.ImageSchema
{
    public class ImageSchema
    {
        public HardDrive[] HardDrives { get; set; }
    }

    public class HardDrive
    {
        public bool Active { get; set; }
        public string Boot { get; set; }
        public string Guid { get; set; }
        public short Lbs { get; set; }
        public string Name { get; set; }    
        public short Pbs { get; set; }
        public long Size { get; set; }
        public string Table { get; set; }
        public string Destination { get; set; }
        public Partition[] Partitions { get; set; }
    }

    public class Partition
    {
        public bool Active { get; set; }
        public long End { get; set; }
        public string FsId { get; set; }
        public string FsType { get; set; }
        public string Guid { get; set; }
        public string Number { get; set; }
        public long VolumeSize { get; set; }
        public long Size { get; set; }
        public string CustomSize { get; set; }
        public long Start { get; set; }
        public string Type { get; set; }
        public long UsedMb { get; set; }
        public string Uuid { get; set; }
        public bool ForceFixedSize { get; set; }
        public string CustomSizeUnit { get; set; }
        public string Prefix { get; set; }
        public string EfiBootLoader { get; set; }
        public VolumeGroup VolumeGroup { get; set; }
    }

    public class VolumeGroup
    {   
        public string Name { get; set; }
        public string PhysicalVolume { get; set; }
        public long Size { get; set; }
        public string Type { get; set; }
        public string Uuid { get; set; }
        public LogicalVolume[] LogicalVolumes { get; set; }
    }

    public class LogicalVolume
    {
        public bool Active { get; set; }
        public string FsType { get; set; }
        public string Name { get; set; }
        public long VolumeSize { get; set; }
        public long Size { get; set; }
        public string CustomSize { get; set; }
        public string Type { get; set; }
        public long UsedMb { get; set; }
        public string Uuid { get; set; }
        public string VolumeGroup { get; set; }
        public bool ForceFixedSize { get; set; }
        public string CustomSizeUnit { get; set; }
    }

    public class ImageFileInfo
    {
        public string FileName { get; set; }
        public string FileSize { get; set; }
    }
}