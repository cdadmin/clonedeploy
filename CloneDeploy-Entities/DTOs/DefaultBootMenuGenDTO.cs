using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloneDeploy_Entities.DTOs
{
    public class BootMenuGenOptionsDTO
    {
        public string AddPwd { get; set; }
        public string BootImage { get; set; }
        public string DebugPwd { get; set; }
        public string DiagPwd { get; set; }
        public string GrubPassword { get; set; }
        public string GrubUserName { get; set; }
        public string Kernel { get; set; }
        public string OndPwd { get; set; }
        public string Type { get; set; }
    }
}
