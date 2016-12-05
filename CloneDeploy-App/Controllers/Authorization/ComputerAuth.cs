using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;


namespace CloneDeploy_App.Controllers.Authorization
{
    public class ComputerAuthAttribute : AuthorizeAttribute
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
                case "ComputerSearch":
                case "ComputerCreate":
                    if (new BLL.AuthorizationServices(Convert.ToInt32(userId), Permission).IsAuthorized())
                        authorized = true;
                    break;
                case "ComputerDelete":
                case "ComputerUpdate":
                case "ComputerRead":
                    var objectId = Convert.ToInt32(actionContext.ControllerContext.RouteData.Values["id"]);
                    if (new BLL.AuthorizationServices(Convert.ToInt32(userId), Permission).ComputerManagement(objectId))
                        authorized = true;
                    break;
                case "NotRequired":
                    authorized = true;
                    break;
            }

            if (!authorized)
            {
                var response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden, new ValidationResultDTO() { Success = false, ErrorMessage = "Forbidden" });
                throw new HttpResponseException(response);
            }
        }
    }
}