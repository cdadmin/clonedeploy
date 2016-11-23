using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using CloneDeploy_App.BLL;
using CloneDeploy_App.Helpers;

namespace CloneDeploy_App.Controllers.Authorization
{
    public class ClientAuthAttribute : AuthorizeAttribute
    {
        public string Permission { get; set; }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var userToken = Utility.Decode(HttpContext.Current.Request.Headers["Authorization"], "Authorization");
            if (!new Logic().Authorize(userToken))
            {
                var response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
                throw new HttpResponseException(response);
            }
        }
    }
}