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

namespace CloneDeploy_App.Controllers.Authorization
{
    public class TaskAuthAttribute : AuthorizeAttribute
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
                case "GroupSearch":
                case "GroupCreate":
                    if (new BLL.Auth(Convert.ToInt32(userId), Permission).IsAuthorized())
                        authorized = true;
                    break;
                case "ImageTaskDelete":
                    var objectId = Convert.ToInt32(actionContext.ControllerContext.RouteData.Values["id"]);
                    var activeImagingTask = BLL.ActiveImagingTask.GetTask(objectId);
                    var computer = BLL.Computer.GetComputer(activeImagingTask.ComputerId);
                    if (new BLL.Auth(Convert.ToInt32(userId), "ImageTaskDeploy").ComputerManagement(computer.Id))
                        authorized = true;
                    break;
            }

            if (!authorized)
            {
                var response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden, new ActionResultEntity() { Success = false, Message = "Forbidden" });
                throw new HttpResponseException(response);
            }
        }
    }
   
}