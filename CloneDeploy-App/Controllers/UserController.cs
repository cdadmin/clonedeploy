using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using CloneDeploy_App.BLL;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_App.DTOs;
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


        [UserAuth(Permission = "Administrator")]
        public IEnumerable<CloneDeployUserEntity> GetAll(string searchstring = "")
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();

            return string.IsNullOrEmpty(searchstring)
                ? _userServices.SearchUsers()
                : _userServices.SearchUsers(searchstring);

        }

        [UserAuth(Permission = "CallerOnly")]
        public ApiObjectResponseDTO GetForLogin(int id)
        {
            return _userServices.GetUserForLogin(id);
        }

        [UserAuth(Permission = "Administrator")]
        public ApiStringResponseDTO GetCount()
        {
            return new ApiStringResponseDTO() {Value = _userServices.TotalCount()};
        }

        [UserAuth(Permission = "Administrator")]
        public ApiStringResponseDTO GetAdminCount()
        {
            return new ApiStringResponseDTO() {Value = _userServices.GetAdminCount().ToString()};
        }

        [UserAuth(Permission = "Administrator")]
        public ActionResultDTO Post(CloneDeployUserEntity user)
        {
            return  _userServices.AddUser(user);
        }

        [UserAuth(Permission = "Administrator")]
        public ActionResultDTO Put(int id, CloneDeployUserEntity user)
        {
            user.Id = id;
            var result = _userServices.UpdateUser(user);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [UserAuth(Permission = "Administrator")]
        public ActionResultDTO Delete(int id)
        {
            var result = _userServices.DeleteUser(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [UserAuth(Permission = "Administrator")]
        public ApiBoolResponseDTO IsAdmin(int id)
        {
            return new ApiBoolResponseDTO() {Value = _userServices.IsAdmin(id)};
        }

        [UserAuth(Permission = "Administrator")]
        public CloneDeployUserEntity Get(int id)
        {
            var result = _userServices.GetUser(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [UserAuth(Permission = "Administrator")]
        public CloneDeployUserEntity GetByName(string username)
        {
            var result = _userServices.GetUser(username);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [UserAuth(Permission = "Administrator")]
        public IEnumerable<UserRightEntity> GetRights(int id)
        {
            return _userServices.GetUserRights(id);        
        }

        [UserAuth(Permission = "Administrator")]
        public ApiBoolResponseDTO DeleteRights(int id)
        {
            return new ApiBoolResponseDTO() {Value = _userServices.DeleteUserRights(id)};

        }

        [UserAuth(Permission = "Administrator")]
        public IEnumerable<UserImageManagementEntity> GetImageManagements(int id)
        {
            return _userServices.GetUserImageManagements(id);
        }

        [UserAuth(Permission = "Administrator")]
        public ApiBoolResponseDTO DeleteImageManagements(int id)
        {
            return new ApiBoolResponseDTO() {Value = _userServices.DeleteUserImageManagements(id)};

        }

        [UserAuth(Permission = "Administrator")]
        public IEnumerable<UserGroupManagementEntity> GetGroupManagements(int id)
        {
            return _userServices.GetUserGroupManagements(id);
        }

        [UserAuth(Permission = "Administrator")]
        public ApiBoolResponseDTO DeleteGroupManagements(int id)
        {
            return new ApiBoolResponseDTO() {Value = _userServices.DeleteUserGroupManagements(id)};
        }
   

     
    }
}
