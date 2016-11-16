using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Results;
using Models;

namespace Service
{
    public class UserAuthAttribute : AuthorizeAttribute
    {
        public string Permission { get; set; }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;     
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                              .Select(c => c.Value).SingleOrDefault();

            if (userId == null) return;

           
            var authorized = false;
            switch (Permission)
            {
                case "Administrator":
                    if (new BLL.Auth(Convert.ToInt32(userId), Permission).IsAuthorized())
                        authorized = true;
                    break;            
            }

            if (!authorized)
            {
                var response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, new ActionResult() { Success = false, Message = "Unauthorized" });
                throw new HttpResponseException(response);
            }
        }
    }

    public class UserController : ApiController
    {
        [UserAuthAttribute(Permission = "Administrator")]
        public IEnumerable<Models.CloneDeployUser> Get(string searchstring="")
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();

            return string.IsNullOrEmpty(searchstring)
                ? BLL.User.SearchUsers()
                : BLL.User.SearchUsers(searchstring);

        }

        [UserAuthAttribute(Permission = "Administrator")]
        public IHttpActionResult GetByName(string name)
        {
            var user = BLL.User.GetUser(name);
            if (user == null)
                return NotFound();
            else
                return Ok(user);
        }

     
    }
}
