namespace CloneDeploy_Entities.DTOs
{
    public class ActionResultDTO
    {
        public ActionResultDTO()
        {
            Success = false;
        }

        public string ErrorMessage { get; set; }
        public int Id { get; set; }
        public bool Success { get; set; }
    }
}