using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloneDeploy_Entities.DTOs
{
   public class ImageShareDTO
    {
        public string Type { get; set; }
        public string Server { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public string ReadWriteUser { get; set; }
        public string ReadWritePassword { get; set; }
        public string ReadOnlyUser { get; set; }
        public string ReadOnlyPassword { get; set; }
        public string PhysicalPath { get; set; }
        public string QueueSize { get; set; }
    }
}
