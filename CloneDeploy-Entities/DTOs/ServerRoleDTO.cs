namespace CloneDeploy_Entities.DTOs
{
    public class ServerRoleDTO
    {
        public string Identifier { get; set; }
        public string OperationMode { get; set; }
        public bool IsImageServer { get; set; }
        public bool IsTftpServer { get; set; }
        public bool IsMulticastServer { get; set; }
    }
}
