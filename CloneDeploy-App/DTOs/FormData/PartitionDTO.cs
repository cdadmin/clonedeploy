using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloneDeploy_App.DTOs.FormData
{
    public class PartitionDTO
    {
        public string imageProfileId { get; set; }
        public string hdToGet { get; set; }
        public string newHdSize { get; set; }
        public string clientHd { get; set; }
        public string taskType { get; set; }
        public string partitionPrefix { get; set; }
        public string lbs { get; set; }
    }
}