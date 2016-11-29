using System.Collections.Generic;

namespace CloneDeploy_Entities.DTOs.ClientImaging
{
    public class ImageProfileList
    {
        public string Count { get; set; }
        public string FirstProfileId { get; set; }
        public List<string> ImageProfiles { get; set; }
    }
}