using CloneDeploy_Entities.DTOs.ImageSchemaBE;

namespace CloneDeploy_Entities.DTOs
{
    public class MulticastArgsDTO
    {
        public string clientCount { get; set; }
        public string Environment { get; set; }
        public string ExtraArgs { get; set; }
        public string groupName { get; set; }
        public string ImageName { get; set; }
        public string Port { get; set; }
        public ImageSchema schema { get; set; }
    }
}