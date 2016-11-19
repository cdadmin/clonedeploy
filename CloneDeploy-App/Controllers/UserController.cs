using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using CloneDeploy_App.Models;

namespace CloneDeploy_App.Controllers
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
                case "CallerOnly":
                    var objectId = Convert.ToInt32(actionContext.ControllerContext.RouteData.Values["id"]);
                    if (objectId == Convert.ToInt32(userId))
                        authorized = true;
                    break;
            }

            if (!authorized)
            {
                var response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden, new ActionResult() { Success = false, Message = "Forbidden" });
                throw new HttpResponseException(response);
            }
        }
    }

    public class UserController : ApiController
    {

        [UserAuth(Permission = "Administrator")]
        public IEnumerable<Models.CloneDeployUser> Get(string searchstring="")
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();

            return string.IsNullOrEmpty(searchstring)
                ? BLL.User.SearchUsers()
                : BLL.User.SearchUsers(searchstring);

        }

        [UserAuth(Permission = "CallerOnly")]
        public ActionResult GetForLogin(int id)
        {
            var actionResult = BLL.User.GetUserForLogin(id);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;

         
        }

     
    }
}
