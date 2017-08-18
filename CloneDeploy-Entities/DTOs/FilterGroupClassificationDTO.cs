using System.Collections.Generic;

namespace CloneDeploy_Entities.DTOs
{
    public class FilterGroupClassificationDTO
    {
        public int GroupId { get; set; }
        public List<ImageWithDate> ListImages { get; set; }
    }
}