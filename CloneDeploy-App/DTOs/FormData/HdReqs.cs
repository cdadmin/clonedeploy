using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloneDeploy_App.DTOs.FormData
{
    public class HdReqs
    {
        public string profileId { get; set; } 
        public string clientHdNumber { get; set; } 
        public string newHdSize { get; set; } 
        public string schemaHds { get; set; }
        public string clientLbs { get; set; }
    }
}