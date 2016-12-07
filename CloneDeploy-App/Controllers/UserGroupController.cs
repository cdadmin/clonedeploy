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
   
    public class UserGroupController : ApiController
    {
        private readonly UserGroupServices _userGroupServices;

        public UserGroupController()
        {
            _userGroupServices = new UserGroupServices();
        }

        [UserAuth(Permission = "Administrator")]
        public IEnumerable<CloneDeployUserGroupEntity> GetAll(string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? _userGroupServices.SearchUserGroups()
                : _userGroupServices.SearchUserGroups(searchstring);

        }

        [UserAuth(Permission = "Administrator")]
        public CloneDeployUserGroupEntity Get(int id)
        {
            var result = _userGroupServices.GetUserGroup(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

       

        [UserAuth(Permission = "Administrator")]
        public ApiStringResponseDTO GetCount()
        {
            return new ApiStringResponseDTO() {Value = _userGroupServices.TotalCount()};
        }

      

        [UserAuth(Permission = "Administrator")]
        public ActionResultDTO Post(CloneDeployUserGroupEntity userGroup)
        {
            return _userGroupServices.AddUserGroup(userGroup);
        }

        [UserAuth(Permission = "Administrator")]
        public ActionResultDTO Put(int id, CloneDeployUserGroupEntity userGroup)
        {
            userGroup.Id = id;
            var result = _userGroupServices.UpdateUser(userGroup);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [UserAuth(Permission = "Administrator")]
        public ActionResultDTO Delete(int id)
        {
            var result = _userGroupServices.DeleteUserGroup(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [UserAuth(Permission = "Administrator")]
        public ApiStringResponseDTO GetMemberCount(int id)
        {
            return new ApiStringResponseDTO() {Value = _userGroupServices.MemberCount(id)};
        }

        [UserAuth(Permission = "Administrator")]
        public IEnumerable<CloneDeployUserEntity> GetGroupMembers(int id, string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? _userGroupServices.GetGroupMembers(id)
                : _userGroupServices.GetGroupMembers(id,searchstring);

        }

        [UserAuth(Permission = "Administrator")]
        public ApiBoolResponseDTO UpdateMemberAcls(int id)
        {
            return new ApiBoolResponseDTO() {Value = _userGroupServices.UpdateAllGroupMembersAcls(id)};

        }

        [UserAuth(Permission = "Administrator")]
        public ApiBoolResponseDTO UpdateMemberGroups(int id)
        {
            return new ApiBoolResponseDTO() {Value = _userGroupServices.UpdateAllGroupMembersGroupMgmt(id)};

        }

        [UserAuth(Permission = "Administrator")]
        public ApiBoolResponseDTO UpdateMemberImages(int id)
        {
            return new ApiBoolResponseDTO() {Value = _userGroupServices.UpdateAllGroupMembersImageMgmt(id)};

        }

        [UserAuth(Permission = "Administrator")]
        public ApiBoolResponseDTO AddNewMember(CloneDeployUserGroupEntity userGroup, CloneDeployUserEntity user)
        {
            return new ApiBoolResponseDTO() {Value = _userGroupServices.AddNewGroupMember(userGroup, user)};
        }

        [UserAuth(Permission = "Administrator")]
        public IEnumerable<UserGroupRightEntity> GetRights(int id)
        {
            return _userGroupServices.GetUserGroupRights(id);
        }

        [UserAuth(Permission = "Administrator")]
        public ApiBoolResponseDTO DeleteRights(int id)
        {
            return new ApiBoolResponseDTO() {Value = _userGroupServices.DeleteUserGroupRights(id)};

        }

        [UserAuth(Permission = "Administrator")]
        public IEnumerable<UserGroupImageManagementEntity> GetImageManagements(int id)
        {
            return _userGroupServices.GetUserGroupImageManagements(id);
        }

        [UserAuth(Permission = "Administrator")]
        public ApiBoolResponseDTO DeleteImageManagements(int id)
        {
            return new ApiBoolResponseDTO() {Value = _userGroupServices.DeleteUserGroupImageManagements(id)};

        }

        [UserAuth(Permission = "Administrator")]
        public IEnumerable<UserGroupGroupManagementEntity> GetGroupManagements(int id)
        {
            return _userGroupServices.GetUserGroupGroupManagements(id);
        }

        [UserAuth(Permission = "Administrator")]
        public ApiBoolResponseDTO DeleteGroupManagements(int id)
        {
            return new ApiBoolResponseDTO() {Value = _userGroupServices.DeleteUserGroupGroupManagements(id)};

        }
     
    }
}
