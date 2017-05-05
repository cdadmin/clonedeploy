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


namespace CloneDeploy_App.Controllers.Authorization
{
    public class CustomAuthAttribute : AuthorizeAttribute
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
                case "GroupSearch":
                case "GroupCreate":
                case "SmartCreate":
                case "ImageSearch":
                case "ImageCreate":
                case "ProfileSearch":
                case "ProfileCreate":
                case "AdminRead":
                case "AdminUpdate":
                case "Administrator":
                case "GlobalRead":
                case "GlobalUpdate":
                case "GlobalCreate":
                case "GlobalDelete":
                case "AllowOnd":
                case "ServiceAccount":
                    if (new AuthorizationServices(Convert.ToInt32(userId), Permission).IsAuthorized())
                        authorized = true;
                    break;
                case "ComputerDelete":
                case "ComputerUpdate":
                case "ComputerRead":
                case "ImageTaskDeploy":
                case "ImageTaskUpload":
                    var computerId = Convert.ToInt32(actionContext.ControllerContext.RouteData.Values["id"]);
                    if (new AuthorizationServices(Convert.ToInt32(userId), Permission).ComputerManagement(computerId))
                        authorized = true;
                    break;
                case "GroupDelete":
                case "GroupUpdate":
                case "GroupRead":
                case "SmartUpdate":
                case "ImageTaskMulticast":
                    var groupId = Convert.ToInt32(actionContext.ControllerContext.RouteData.Values["id"]);
                    if (new AuthorizationServices(Convert.ToInt32(userId), Permission).GroupManagement(groupId))
                        authorized = true;
                    break;
                case "ImageTaskDeployGroup":
                    var groupId_a = Convert.ToInt32(actionContext.ControllerContext.RouteData.Values["id"]);
                    if (new AuthorizationServices(Convert.ToInt32(userId), "ImageTaskDeploy").GroupManagement(groupId_a))
                        authorized = true;
                    break;
                case "ImageDelete":
                case "ImageUpdate":
                case "ImageRead":
                case "ApproveImage":
                    var imageId = Convert.ToInt32(actionContext.ControllerContext.RouteData.Values["id"]);
                    if (new AuthorizationServices(Convert.ToInt32(userId), Permission).ImageManagement(imageId))
                        authorized = true;
                    break;
                case "ProfileDelete":
                case "ProfileUpdate":
                case "ProfileRead":
                    var profileId = Convert.ToInt32(actionContext.ControllerContext.RouteData.Values["id"]);
                    var profile = new ImageProfileServices().ReadProfile(profileId);
                    if (profile == null)
                    {
                        authorized = true;
                        break;
                    }
                    else
                    {
                        var profileImageId = profile.ImageId;
                        if (
                            new AuthorizationServices(Convert.ToInt32(userId), Permission).ImageManagement(
                                profileImageId))
                            authorized = true;

                        break;
                    }
                case "ImageTaskDelete":
                    var objectId = Convert.ToInt32(actionContext.ControllerContext.RouteData.Values["id"]);
                    var activeImagingTask = new ActiveImagingTaskServices().GetTask(objectId);
                    var computer = new ComputerServices().GetComputer(activeImagingTask.ComputerId);
                    if (new AuthorizationServices(Convert.ToInt32(userId), "ImageTaskDeploy").ComputerManagement(computer.Id))
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