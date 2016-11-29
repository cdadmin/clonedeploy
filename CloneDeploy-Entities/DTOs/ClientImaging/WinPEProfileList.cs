using System.Collections.Generic;

namespace CloneDeploy_Entities.DTOs.ClientImaging
{
    public class WinPEProfileList
    {
        public string Count { get; set; }
        public string FirstProfileId { get; set; }
        public List<WinPEProfile> ImageProfiles { get; set; }
    }
}