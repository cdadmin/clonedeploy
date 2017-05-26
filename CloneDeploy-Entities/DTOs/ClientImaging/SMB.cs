namespace CloneDeploy_Entities.DTOs.ClientImaging
{
    public class SMB
    {
        public string SharePath { get; set; }
        public string Domain { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public string IsPrimary { get; set; }
    }
}