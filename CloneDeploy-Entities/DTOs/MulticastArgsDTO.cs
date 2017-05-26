namespace CloneDeploy_Entities.DTOs
{
    public class MulticastArgsDTO
    {
        public string ImageName { get; set; }
        public string Environment { get; set; }
        public string Port { get; set; }
        public string ExtraArgs { get; set; }
        public string clientCount { get; set; }
        public CloneDeploy_Entities.DTOs.ImageSchemaBE.ImageSchema schema { get; set; }
        public string groupName { get; set; }
    }
}
