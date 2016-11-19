using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloneDeploy_App.DTOs.FormData
{
    public class OriginalLVM
    {
        public string profileId { get; set; } 
        public string clientHd { get; set; } 
        public string hdToGet { get; set; }
        public string partitionPrefix { get; set; }
    }
}