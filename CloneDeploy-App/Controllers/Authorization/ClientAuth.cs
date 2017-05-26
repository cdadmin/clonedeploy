using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using CloneDeploy_Services;

namespace CloneDeploy_App.Controllers.Authorization
{
    public class ClientAuthAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var userToken = StringManipulationServices.Decode(HttpContext.Current.Request.Headers["Authorization"], "Authorization");
            if (!new ClientImagingServices().Authorize(userToken))
            {
                var response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
                throw new HttpResponseException(response);
            }
        }
    }
}