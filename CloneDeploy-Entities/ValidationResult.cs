namespace CloneDeploy_Entities
{
    public class ActionResultEntity
    {
        public ActionResultEntity()
        {
            Success = true;
            Message = string.Empty;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public int ObjectId { get; set; }
        public string Object { get; set; }
    }
}