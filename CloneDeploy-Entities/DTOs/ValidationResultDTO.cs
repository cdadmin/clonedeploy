namespace CloneDeploy_Entities.DTOs
{
    public class ValidationResultDTO
    {
        public ValidationResultDTO()
        {
            Success = false;
        }

        public string ErrorMessage { get; set; }
        public bool Success { get; set; }
    }
}