namespace CloneDeploy_Entities.DTOs.ImageSchemaBE
{
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
}