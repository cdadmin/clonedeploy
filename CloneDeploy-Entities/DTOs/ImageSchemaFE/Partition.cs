namespace CloneDeploy_Entities.DTOs.ImageSchemaFE
{
    public class Partition
    {
        public bool Active { get; set; }
        public long End { get; set; }
        public string FsId { get; set; }
        public string FsType { get; set; }
        public string Guid { get; set; }
        public string Number { get; set; }
        public string VolumeSize { get; set; }
        public string Size { get; set; }
        public string CustomSize { get; set; }
        public long Start { get; set; }
        public string Type { get; set; }
        public string UsedMb { get; set; }
        public string Uuid { get; set; }
        public string CustomSizeUnit { get; set; }
        public bool ForceFixedSize { get; set; }
        public string Prefix { get; set; }
        public string EfiBootLoader { get; set; }
        public VolumeGroup VolumeGroup { get; set; }
    }
}