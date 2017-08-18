using System.Collections.Generic;

namespace CloneDeploy_Entities.DTOs.ClientImaging
{
    public class VolumeGroup
    {
        public int LogicalVolumeCount { get; set; }
        public List<LogicalVolume> LogicalVolumes { get; set; }
        public string Name { get; set; }
    }
}