namespace CloneDeploy_Entities.DTOs
{
    public class ApiObjectResponseDTO
    {
        public ApiObjectResponseDTO()
        {
            Success = false;
        }

        public string ErrorMessage { get; set; }
        public int Id { get; set; }
        public string ObjectJson { get; set; }
        public bool Success { get; set; }
    }
}