using System.Collections.Generic;

namespace CloneDeploy_Entities.DTOs.ImageSchemaFE
{
    public class HardDrive
    {
        public bool Active { get; set; }
        public string Boot { get; set; }
        public string Destination { get; set; }
        public string Guid { get; set; }
        public short Lbs { get; set; }
        public string Name { get; set; }
        public List<Partition> Partitions { get; set; }
        public string Pbs { get; set; }
        public string Size { get; set; }
        public string Table { get; set; }
    }
}