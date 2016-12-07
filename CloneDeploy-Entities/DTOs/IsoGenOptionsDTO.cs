using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloneDeploy_Entities.DTOs
{
    public class IsoGenOptionsDTO
    {
        public string buildType { get; set; }
        public string kernel { get; set; }
        public string bootImage { get; set; }
        public string arguments { get; set; }
    }
}
