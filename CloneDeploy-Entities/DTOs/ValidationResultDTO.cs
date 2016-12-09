namespace CloneDeploy_Entities.DTOs
{
    public class ValidationResultDTO
    {
        public ValidationResultDTO()
        {
            Success = false;
        }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}
