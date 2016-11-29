namespace CloneDeploy_Entities.DTOs.ImageSchemaBE
{
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
}