using System.Collections.Generic;

namespace CloneDeploy_Entities.DTOs.ClientImaging
{
    public class HardDriveSchema
    {
        public string IsValid { get; set; }
        public string Message { get; set; }
        public int SchemaHdNumber { get; set; }
        public int PhysicalPartitionCount { get; set; }
        public string PartitionType { get; set; }
        public string BootPartition { get; set; }
        public string Guid { get; set; }
        public string UsesLvm { get; set; }
        public List<PhysicalPartition> PhysicalPartitions { get; set; }
    }
}