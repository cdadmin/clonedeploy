using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;


namespace CloneDeploy_App.Controllers
{
   

    public class UserController : ApiController
    {
        private readonly UserServices _userServices;

        public UserController()
        {
            _userServices = new UserServices();
        }


        [CustomAuth(Permission = "Administrator")]
        public IEnumerable<UserWithUserGroup> GetAll(string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? _userServices.SearchUsers()
                : _userServices.SearchUsers(searchstring);

        }

        [Authorize]
        public ApiObjectResponseDTO GetForLogin(string username)
        {
            var identity = (ClaimsPrincipal) Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                .Select(c => c.Value).SingleOrDefault();
            var user = _userServices.GetUser(username);
            if (user.Id == Convert.ToInt32(userId))
                return _userServices.GetUserForLogin(user.Id);

            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
        }

        [CustomAuth(Permission = "Administrator")]
        public ApiStringResponseDTO GetCount()
        {
            return new ApiStringResponseDTO() {Value = _userServices.TotalCount()};
        }

        [CustomAuth(Permission = "Administrator")]
        public ApiIntResponseDTO GetAdminCount()
        {
            return new ApiIntResponseDTO() {Value = _userServices.GetAdminCount()};
        }

        [CustomAuth(Permission = "Administrator")]
        public ActionResultDTO Post(CloneDeployUserEntity user)
        {
            return  _userServices.AddUser(user);
        }

        [CustomAuth(Permission = "Administrator")]
        public ActionResultDTO Put(int id, CloneDeployUserEntity user)
        {
            user.Id = id;
            var result = _userServices.UpdateUser(user);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [CustomAuth(Permission = "Administrator")]
        public ActionResultDTO Delete(int id)
        {
            var result = _userServices.DeleteUser(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [Authorize]
        [HttpGet]
        public ApiBoolResponseDTO IsAdmin(int id)
        {
            return new ApiBoolResponseDTO() {Value = _userServices.IsAdmin(id)};
        }

        [CustomAuth(Permission = "Administrator")]
        public CloneDeployUserEntity Get(int id)
        {
            var result = _userServices.GetUser(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [CustomAuth(Permission = "Administrator")]
        public CloneDeployUserEntity GetByName(string username)
        {
            var result = _userServices.GetUser(username);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [CustomAuth(Permission = "Administrator")]
        public IEnumerable<UserRightEntity> GetRights(int id)
        {
            return _userServices.GetUserRights(id);        
        }

        [CustomAuth(Permission = "Administrator")]
        public ApiBoolResponseDTO DeleteRights(int id)
        {
            return new ApiBoolResponseDTO() {Value = _userServices.DeleteUserRights(id)};

        }

        [CustomAuth(Permission = "Administrator")]
        public IEnumerable<UserImageManagementEntity> GetImageManagements(int id)
        {
            return _userServices.GetUserImageManagements(id);
        }

        [CustomAuth(Permission = "Administrator")]
        public ApiBoolResponseDTO DeleteImageManagements(int id)
        {
            return new ApiBoolResponseDTO() {Value = _userServices.DeleteUserImageManagements(id)};

        }

        [CustomAuth(Permission = "Administrator")]
        public IEnumerable<UserGroupManagementEntity> GetGroupManagements(int id)
        {
            return _userServices.GetUserGroupManagements(id);
        }

        [CustomAuth(Permission = "Administrator")]
        public ApiBoolResponseDTO DeleteGroupManagements(int id)
        {
            return new ApiBoolResponseDTO() {Value = _userServices.DeleteUserGroupManagements(id)};
        }
   

     
    }
}
