using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_App.DTOs;
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

        [UserAuth(Permission = "Administrator")]
        public ApiDTO GetCount()
        {
            var ApiDTO = new ApiDTO();
            ApiDTO.Value = BLL.User.TotalCount();
            return ApiDTO;
        }

        [UserAuth(Permission = "Administrator")]
        public ApiDTO GetAdminCount()
        {
            var ApiDTO = new ApiDTO();
            ApiDTO.Value = BLL.User.GetAdminCount().ToString();
            return ApiDTO;
        }

        [UserAuth(Permission = "Administrator")]
        public ActionResult Post(Models.CloneDeployUser user)
        {
            var actionResult = BLL.User.AddUser(user);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }

        [UserAuth(Permission = "Administrator")]
        public Models.ActionResult Put(int id, Models.CloneDeployUser user)
        {
            user.Id = id;
            var actionResult = BLL.User.UpdateUser(user);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }

        [UserAuth(Permission = "Administrator")]
        public ApiBoolDTO Delete(int id)
        {
            var apiBoolDto = new ApiBoolDTO();
            apiBoolDto.Value = BLL.User.DeleteUser(id);
            return apiBoolDto;
        }

        [UserAuth(Permission = "Administrator")]
        public ApiBoolDTO IsAdmin(int id)
        {
            var apiBoolDto = new ApiBoolDTO();
            apiBoolDto.Value = BLL.User.IsAdmin(id);
            return apiBoolDto;
        }

        [UserAuth(Permission = "Administrator")]
        public IHttpActionResult Get(int id)
        {
            var result = BLL.User.GetUser(id);
            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }

        [UserAuth(Permission = "Administrator")]
        public IHttpActionResult GetByName(string username)
        {
            var result = BLL.User.GetUser(username);
            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }

        [UserAuth(Permission = "Administrator")]
        public IEnumerable<Models.UserRight> GetRights(int id)
        {
            return BLL.UserRight.Get(id);        
        }

        [UserAuth(Permission = "Administrator")]
        public ApiBoolDTO DeleteRights(int id)
        {
            var apiBoolDto = new ApiBoolDTO();
            apiBoolDto.Value= BLL.UserRight.DeleteUserRights(id);
            return apiBoolDto;

        }

        [UserAuth(Permission = "Administrator")]
        public IEnumerable<Models.UserImageManagement> GetImageManagements(int id)
        {
            return BLL.UserImageManagement.Get(id);
        }

        [UserAuth(Permission = "Administrator")]
        public ApiBoolDTO DeleteImageManagements(int id)
        {
            var apiBoolDto = new ApiBoolDTO();
            apiBoolDto.Value = BLL.UserImageManagement.DeleteUserImageManagements(id);
            return apiBoolDto;

        }

        [UserAuth(Permission = "Administrator")]
        public IEnumerable<Models.UserGroupManagement> GetGroupManagements(int id)
        {
            return BLL.UserGroupManagement.Get(id);
        }

        [UserAuth(Permission = "Administrator")]
        public ApiBoolDTO DeleteGroupManagements(int id)
        {
            var apiBoolDto = new ApiBoolDTO();
            apiBoolDto.Value = BLL.UserGroupManagement.DeleteUserGroupManagements(id);
            return apiBoolDto;

        }
   

     
    }
}
