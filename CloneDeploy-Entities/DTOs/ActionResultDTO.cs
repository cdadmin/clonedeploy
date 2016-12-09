namespace CloneDeploy_Entities.DTOs
{
    public class ActionResultDTO
    {
        public ActionResultDTO()
        {
            Success = false;
        }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public int Id { get; set; }
    }
}
