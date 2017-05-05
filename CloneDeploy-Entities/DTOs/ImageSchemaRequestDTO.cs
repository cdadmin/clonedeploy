namespace CloneDeploy_Entities.DTOs
{
    public class ImageSchemaRequestDTO
    {
        public ImageProfileWithImage imageProfile { get; set; }
        public string schemaType { get; set; }
        public ImageEntity image { get; set; }
        public string selectedHd { get; set; }
    }
}