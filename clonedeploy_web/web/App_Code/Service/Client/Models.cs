using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Services.Client
{
    public class ImageList
    {
        public List<string> Images  { get; set; }
    }

    public class ImageProfileList
    {
        public string Count { get; set; }
        public string FirstProfileId { get; set; }
        public List<string> ImageProfiles { get; set; } 
        
    }

    public class CheckIn
    {
        public string Result { get; set; }
        public string Message { get; set; }
        public string TaskArguments { get; set; }
    }

    public class SMB
    {
        public string SharePath { get; set; }
        public string Domain { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class HdRequirement
    {
        public string IsValid { get; set; }
        public string Message { get; set; }
        public string MinimumSizeBytes { get; set; }
        public int SchemaHdNumber { get; set; }
    }

}
