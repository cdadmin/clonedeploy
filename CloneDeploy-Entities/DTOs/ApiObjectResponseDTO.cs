namespace CloneDeploy_Entities.DTOs
{
    public class ApiObjectResponseDTO
    {
        public ApiObjectResponseDTO()
        {
            Success = false;
        }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public int Id { get; set; }
        public string ObjectJson { get; set; }
    }
}
