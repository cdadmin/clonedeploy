namespace CloneDeploy_App.Models
{
    public class ActionResult
    {
        public ActionResult()
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