using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloneDeploy_Entities.DTOs
{
    public class DpFreeSpaceDTO
    {
        public ulong freespace { get; set; }
        public ulong total  {get; set;}
        public int freePercent { get; set; }
        public int usedPercent { get; set; }
        public string dPPath { get; set; }
    }
}
