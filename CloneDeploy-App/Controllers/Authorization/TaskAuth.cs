using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;
using log4net;

namespace CloneDeploy_App.Controllers.Authorization
{
    public class TaskAuthAttribute : AuthorizeAttribute
    {
        public string Permission { get; set; }
        private readonly ILog log = LogManager.GetLogger("ApplicationLog");

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
                    if (new AuthorizationServices(Convert.ToInt32(userId), Permission).IsAuthorized())
                        authorized = true;
                    break;
                case "ImageTaskDelete": 
                    var objectId = Convert.ToInt32(actionContext.ControllerContext.RouteData.Values["id"]);
                    var activeImagingTask = new ActiveImagingTaskServices().GetTask(objectId);
                    var computer = new ComputerServices().GetComputer(activeImagingTask.ComputerId);
                    if (new AuthorizationServices(Convert.ToInt32(userId), "ImageTaskDeploy").ComputerManagement(computer.Id))
                        authorized = true;
                    break;
                case "ImageTaskDeploy":
                case "ImageTaskUpload":

                    var computerId = Convert.ToInt32(actionContext.ControllerContext.RouteData.Values["id"]);
                    if (new AuthorizationServices(Convert.ToInt32(userId), "ImageTaskDeploy").ComputerManagement(computerId))
                        authorized = true;
                    break;
            }

            if (!authorized)
            {
                log.Debug("Forbidden API Request");
                var response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden, new ValidationResultDTO() { Success = false, ErrorMessage = "Forbidden" });
                throw new HttpResponseException(response);
            }
        }
    }
   
}