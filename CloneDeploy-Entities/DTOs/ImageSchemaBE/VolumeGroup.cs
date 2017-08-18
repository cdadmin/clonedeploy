namespace CloneDeploy_Entities.DTOs.ImageSchemaBE
{
    public class VolumeGroup
    {
        public LogicalVolume[] LogicalVolumes { get; set; }
        public string Name { get; set; }
        public string PhysicalVolume { get; set; }
        public long Size { get; set; }
        public string Type { get; set; }
        public string Uuid { get; set; }
    }
}