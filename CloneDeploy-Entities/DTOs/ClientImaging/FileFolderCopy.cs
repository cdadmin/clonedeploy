namespace CloneDeploy_Entities.DTOs.ClientImaging
{
    public class FileFolderCopy
    {
        public string SourcePath { get; set; }
        public string DestinationFolder { get; set; }
        public string DestinationPartition { get; set; }
        public string FolderCopyType { get; set; }
    }
}