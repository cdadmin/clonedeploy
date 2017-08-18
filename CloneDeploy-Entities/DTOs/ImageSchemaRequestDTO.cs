namespace CloneDeploy_Entities.DTOs
{
    public class ImageSchemaRequestDTO
    {
        public ImageEntity image { get; set; }
        public ImageProfileWithImage imageProfile { get; set; }
        public string schemaType { get; set; }
        public string selectedHd { get; set; }
    }
}