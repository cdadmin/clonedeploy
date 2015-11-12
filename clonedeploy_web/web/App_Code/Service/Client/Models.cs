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

}
