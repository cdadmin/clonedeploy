using System.Collections.Generic;

namespace CloneDeploy_Entities.DTOs.ClientImaging
{
    public class VolumeGroup
    {
        public string Name { get; set; }
        public int LogicalVolumeCount { get; set; }
        public List<LogicalVolume> LogicalVolumes { get; set; }
    }
}