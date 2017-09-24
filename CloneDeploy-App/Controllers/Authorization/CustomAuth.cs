using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using CloneDeploy_Common;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;
using log4net;

namespace CloneDeploy_App.Controllers.Authorization
{
    public class CustomAuthAttribute : AuthorizeAttribute
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(CustomAuthAttribute));
        public string Permission { get; set; }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            _log.Debug(actionContext.Request.RequestUri);
            base.OnAuthorization(actionContext);
            var identity = (ClaimsPrincipal) Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                .Select(c => c.Value).SingleOrDefault();

            if (userId == null) return;

            var authorized = false;
            switch (Permission)
            {
                case AuthorizationStrings.SearchComputer:
                case AuthorizationStrings.CreateComputer:
                case AuthorizationStrings.SearchGroup:
                case AuthorizationStrings.CreateGroup:
                case AuthorizationStrings.CreateSmart:
                case AuthorizationStrings.SearchImage:
                case AuthorizationStrings.CreateImage:
                case AuthorizationStrings.SearchProfile:
                case AuthorizationStrings.CreateProfile:
                case AuthorizationStrings.ReadAdmin:
                case AuthorizationStrings.UpdateAdmin:
                case AuthorizationStrings.Administrator:
                case AuthorizationStrings.ReadGlobal:
                case AuthorizationStrings.UpdateGlobal:
                case AuthorizationStrings.CreateGlobal:
                case AuthorizationStrings.DeleteGlobal:
                case AuthorizationStrings.AllowOnd:
                case AuthorizationStrings.ServiceAccount:
                    if (new AuthorizationServices(Convert.ToInt32(userId), Permission).IsAuthorized())
                        authorized = true;
                    break;
                case AuthorizationStrings.DeleteComputer:
                case AuthorizationStrings.UpdateComputer:
                case AuthorizationStrings.ReadComputer:
                case AuthorizationStrings.ImageDeployTask:
                case AuthorizationStrings.ImageUploadTask:
                    var computerId = Convert.ToInt32(actionContext.ControllerContext.RouteData.Values["id"]);
                    if (new AuthorizationServices(Convert.ToInt32(userId), Permission).ComputerManagement(computerId))
                        authorized = true;
                    break;
                case AuthorizationStrings.DeleteGroup:
                case AuthorizationStrings.UpdateGroup:
                case AuthorizationStrings.ReadGroup:
                case AuthorizationStrings.UpdateSmart:
                case AuthorizationStrings.ImageMulticastTask:
                    var groupId = Convert.ToInt32(actionContext.ControllerContext.RouteData.Values["id"]);
                    if (new AuthorizationServices(Convert.ToInt32(userId), Permission).GroupManagement(groupId))
                        authorized = true;
                    break;
                case AuthorizationStrings.ImageTaskDeployGroup:
                    var groupId_a = Convert.ToInt32(actionContext.ControllerContext.RouteData.Values["id"]);
                    if (
                        new AuthorizationServices(Convert.ToInt32(userId), AuthorizationStrings.ImageDeployTask)
                            .GroupManagement(groupId_a))
                        authorized = true;
                    break;
                case AuthorizationStrings.DeleteImage:
                case AuthorizationStrings.UpdateImage:
                case AuthorizationStrings.ReadImage:
                case AuthorizationStrings.ApproveImage:
                    var imageId = Convert.ToInt32(actionContext.ControllerContext.RouteData.Values["id"]);
                    if (new AuthorizationServices(Convert.ToInt32(userId), Permission).ImageManagement(imageId))
                        authorized = true;
                    break;
                case AuthorizationStrings.DeleteProfile:
                case AuthorizationStrings.UpdateProfile:
                case AuthorizationStrings.ReadProfile:
                    var profileId = Convert.ToInt32(actionContext.ControllerContext.RouteData.Values["id"]);
                    var profile = new ImageProfileServices().ReadProfile(profileId);
                    if (profile == null)
                    {
                        authorized = true;
                        break;
                    }
                    var profileImageId = profile.ImageId;
                    if (
                        new AuthorizationServices(Convert.ToInt32(userId), Permission).ImageManagement(
                            profileImageId))
                        authorized = true;

                    break;
                case AuthorizationStrings.ImageDeleteTask:
                    var objectId = Convert.ToInt32(actionContext.ControllerContext.RouteData.Values["id"]);
                    var activeImagingTask = new ActiveImagingTaskServices().GetTask(objectId);
                    var computer = new ComputerServices().GetComputer(activeImagingTask.ComputerId);
                    if (
                        new AuthorizationServices(Convert.ToInt32(userId), AuthorizationStrings.ImageDeployTask)
                            .ComputerManagement(
                                computer.Id))
                        authorized = true;
                    break;
            }

            if (!authorized)
            {
                var response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden,
                    new ValidationResultDTO {Success = false, ErrorMessage = "Forbidden"});
                throw new HttpResponseException(response);
            }
        }
    }
}