using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloneDeploy_Entities.DTOs
{
    public class FileUploadDTO
    {
        public string Filename { get; set; }
        public Stream InputStream { get; set; }
        public int PartIndex { get; set; }
        public int TotalParts { get; set; }
        public string OriginalFilename { get; set; }
        public string PartUuid { get; set; }
        public ulong FileSize { get; set; }
        public string UploadMethod { get; set; }
        public string DestinationDirectory { get; set; }
        
    }
}
