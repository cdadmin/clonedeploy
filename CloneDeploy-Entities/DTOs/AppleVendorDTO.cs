using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloneDeploy_Entities.DTOs
{
    public class AppleVendorDTO
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public string VendorString { get; set; }
        public string Instructions { get; set; }
    }
}
