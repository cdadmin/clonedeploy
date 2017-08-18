namespace CloneDeploy_Entities.DTOs.ClientPartition
{
    public class ClientPartition
    {
        public string FsId { get; set; }
        public string FsType { get; set; }
        public string Guid { get; set; }
        public bool IsBoot { get; set; }
        public string Number { get; set; }
        public long Size { get; set; }
        public bool SizeIsDynamic { get; set; }
        public long Start { get; set; }
        public string Type { get; set; }
        public string Uuid { get; set; }
    }
}