namespace CloneDeploy_Entities.DTOs
{
    public class VersionDTO
    {
        public bool FirstRunCompleted { get; set; }
        public string DatabaseVersion { get; set; }
        public string TargetDbVersion { get; set; }
    }
}