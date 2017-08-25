using System.Web.Http;
using CloneDeploy_Services;

namespace CloneDeploy_App.Controllers
{
    public class TokenController : ApiController
    {       
        public string Get(string username, string password, string baseUrl)
        {
            return new TokenServices().GetToken(username, password,baseUrl);
        }  
    }
}