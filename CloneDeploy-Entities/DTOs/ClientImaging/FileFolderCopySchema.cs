using System.Collections.Generic;

namespace CloneDeploy_Entities.DTOs.ClientImaging
{
    public class FileFolderCopySchema
    {
        public string Count { get; set; }
        public List<FileFolderCopy> FilesAndFolders { get; set; }
    }
}