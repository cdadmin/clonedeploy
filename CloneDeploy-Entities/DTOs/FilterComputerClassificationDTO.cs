using System.Collections.Generic;

namespace CloneDeploy_Entities.DTOs
{
    public class FilterComputerClassificationDTO
    {
        public int ComputerId { get; set; }
        public List<ImageWithDate> ListImages  { get; set; }

    }
}