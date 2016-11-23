namespace CloneDeploy_Web.Models
{
    public class Token
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public string error_description { get; set; }
        public int user_id { get; set; }
       
      
    }
}