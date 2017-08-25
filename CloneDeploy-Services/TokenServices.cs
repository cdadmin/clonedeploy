using System.Web;
using CloneDeploy_ApiCalls;
using CloneDeploy_DataModel;

namespace CloneDeploy_Services
{
    public class TokenServices
    {
        private readonly UnitOfWork _uow;

        public string GetToken(string username, string password, string baseUrl)
        {
            HttpContext.Current.Response.Cookies.Add(new HttpCookie("cdBaseUrl")
            {
                Value = baseUrl,
                HttpOnly = true
            });
            //ApplicationServers._baseApiUrl = baseUrl;
            var result = new APICall().TokenApi.Get(username, password);
            return !string.IsNullOrEmpty(result.error_description) ? result.error_description : "bearer " + result.access_token;
        }

    
    }
}