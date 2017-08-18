using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
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

        [CustomAuth(Permission = "Administrator")]
        [HttpGet]
        public ApiBoolResponseDTO AddNewMember(int id, int userId)
        {
            return new ApiBoolResponseDTO {Value = _userGroupServices.AddNewGroupMember(id, userId)};
        }

        [CustomAuth(Permission = "Administrator")]
        public ActionResultDTO Delete(int id)
        {
            var result = _userGroupServices.DeleteUserGroup(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [CustomAuth(Permission = "Administrator")]
        public ApiBoolResponseDTO DeleteGroupManagements(int id)
        {
            return new ApiBoolResponseDTO {Value = _userGroupServices.DeleteUserGroupGroupManagements(id)};
        }

        [CustomAuth(Permission = "Administrator")]
        public ApiBoolResponseDTO DeleteImageManagements(int id)
        {
            return new ApiBoolResponseDTO {Value = _userGroupServices.DeleteUserGroupImageManagements(id)};
        }

        [CustomAuth(Permission = "Administrator")]
        public ApiBoolResponseDTO DeleteRights(int id)
        {
            return new ApiBoolResponseDTO {Value = _userGroupServices.DeleteUserGroupRights(id)};
        }

        [CustomAuth(Permission = "Administrator")]
        public CloneDeployUserGroupEntity Get(int id)
        {
            var result = _userGroupServices.GetUserGroup(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [CustomAuth(Permission = "Administrator")]
        public IEnumerable<CloneDeployUserGroupEntity> Get(string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? _userGroupServices.SearchUserGroups()
                : _userGroupServices.SearchUserGroups(searchstring);
        }

        [CustomAuth(Permission = "Administrator")]
        public ApiStringResponseDTO GetCount()
        {
            return new ApiStringResponseDTO {Value = _userGroupServices.TotalCount()};
        }

        [CustomAuth(Permission = "Administrator")]
        public IEnumerable<UserGroupGroupManagementEntity> GetGroupManagements(int id)
        {
            return _userGroupServices.GetUserGroupGroupManagements(id);
        }

        [CustomAuth(Permission = "Administrator")]
        public IEnumerable<CloneDeployUserEntity> GetGroupMembers(int id, string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? _userGroupServices.GetGroupMembers(id)
                : _userGroupServices.GetGroupMembers(id, searchstring);
        }

        [CustomAuth(Permission = "Administrator")]
        public IEnumerable<UserGroupImageManagementEntity> GetImageManagements(int id)
        {
            return _userGroupServices.GetUserGroupImageManagements(id);
        }

        [CustomAuth(Permission = "Administrator")]
        public ApiStringResponseDTO GetMemberCount(int id)
        {
            return new ApiStringResponseDTO {Value = _userGroupServices.MemberCount(id)};
        }

        [CustomAuth(Permission = "Administrator")]
        public IEnumerable<UserGroupRightEntity> GetRights(int id)
        {
            return _userGroupServices.GetUserGroupRights(id);
        }

        [CustomAuth(Permission = "Administrator")]
        public ActionResultDTO Post(CloneDeployUserGroupEntity userGroup)
        {
            return _userGroupServices.AddUserGroup(userGroup);
        }

        [CustomAuth(Permission = "Administrator")]
        public ActionResultDTO Put(int id, CloneDeployUserGroupEntity userGroup)
        {
            userGroup.Id = id;
            var result = _userGroupServices.UpdateUserGroup(userGroup);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [HttpGet]
        [CustomAuth(Permission = "Administrator")]
        public ApiBoolResponseDTO ToggleGroupManagement(int id, int value)
        {
            return new ApiBoolResponseDTO {Value = _userGroupServices.ToggleGroupManagement(id, value)};
        }

        [HttpGet]
        [CustomAuth(Permission = "Administrator")]
        public ApiBoolResponseDTO ToggleImageManagement(int id, int value)
        {
            return new ApiBoolResponseDTO {Value = _userGroupServices.ToggleImageManagement(id, value)};
        }

        [CustomAuth(Permission = "Administrator")]
        [HttpGet]
        public ApiBoolResponseDTO UpdateMemberAcls(int id)
        {
            return new ApiBoolResponseDTO {Value = _userGroupServices.UpdateAllGroupMembersAcls(id)};
        }

        [CustomAuth(Permission = "Administrator")]
        [HttpGet]
        public ApiBoolResponseDTO UpdateMemberGroups(int id)
        {
            return new ApiBoolResponseDTO {Value = _userGroupServices.UpdateAllGroupMembersGroupMgmt(id)};
        }

        [CustomAuth(Permission = "Administrator")]
        [HttpGet]
        public ApiBoolResponseDTO UpdateMemberImages(int id)
        {
            return new ApiBoolResponseDTO {Value = _userGroupServices.UpdateAllGroupMembersImageMgmt(id)};
        }
    }
}