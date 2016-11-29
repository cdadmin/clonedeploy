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


namespace CloneDeploy_App.Controllers
{
   
    public class UserGroupController : ApiController
    {

        [UserAuth(Permission = "Administrator")]
        public IEnumerable<CloneDeployUserGroupEntity> Get(string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? BLL.UserGroup.SearchUserGroups()
                : BLL.UserGroup.SearchUserGroups(searchstring);

        }

        [UserAuth(Permission = "Administrator")]
        public IHttpActionResult Get(int id)
        {
            var result = BLL.UserGroup.GetUserGroup(id);
            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }

       

        [UserAuth(Permission = "Administrator")]
        public ApiDTO GetCount()
        {
            var ApiDTO = new ApiDTO();
            ApiDTO.Value = BLL.UserGroup.TotalCount();
            return ApiDTO;
        }

      

        [UserAuth(Permission = "Administrator")]
        public ActionResultEntity Post(CloneDeployUserGroupEntity userGroup)
        {
            var actionResult = BLL.UserGroup.AddUserGroup(userGroup);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }

        [UserAuth(Permission = "Administrator")]
        public ActionResultEntity Put(int id, CloneDeployUserGroupEntity userGroup)
        {
            userGroup.Id = id;
            var actionResult = BLL.UserGroup.UpdateUser(userGroup);
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
            apiBoolDto.Value = BLL.UserGroup.DeleteUserGroup(id);
            return apiBoolDto;
        }

        [UserAuth(Permission = "Administrator")]
        public ApiDTO GetMemberCount(int id)
        {
            var apiDto = new ApiDTO();
            apiDto.Value = BLL.UserGroup.MemberCount(id);
            return apiDto;
        }

        [UserAuth(Permission = "Administrator")]
        public IEnumerable<CloneDeployUserEntity> GetGroupMembers(int id, string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? BLL.UserGroup.GetGroupMembers(id)
                : BLL.UserGroup.GetGroupMembers(id,searchstring);

        }

        [UserAuth(Permission = "Administrator")]
        public ApiDTO UpdateMemberAcls(CloneDeployUserGroupEntity userGroup)
        {
            var apiDto = new ApiDTO();
            BLL.UserGroup.UpdateAllGroupMembersAcls(userGroup);
            return apiDto;
        }

        [UserAuth(Permission = "Administrator")]
        public ApiDTO UpdateMemberGroups(CloneDeployUserGroupEntity userGroup)
        {
            var apiDto = new ApiDTO();
            BLL.UserGroup.UpdateAllGroupMembersGroupMgmt(userGroup);
            return apiDto;
        }

        [UserAuth(Permission = "Administrator")]
        public ApiDTO UpdateMemberImages(CloneDeployUserGroupEntity userGroup)
        {
            var apiDto = new ApiDTO();
            BLL.UserGroup.UpdateAllGroupMembersImageMgmt(userGroup);
            return apiDto;
        }

        [UserAuth(Permission = "Administrator")]
        public ApiDTO AddNewMember(CloneDeployUserGroupEntity userGroup, CloneDeployUserEntity user)
        {
            var apiDto = new ApiDTO();
            BLL.UserGroup.AddNewGroupMember(userGroup,user);
            return apiDto;
        }

        [UserAuth(Permission = "Administrator")]
        public IEnumerable<UserGroupRightEntity> GetRights(int id)
        {
            return BLL.UserGroupRight.Get(id);
        }

        [UserAuth(Permission = "Administrator")]
        public ApiBoolDTO DeleteRights(int id)
        {
            var apiBoolDto = new ApiBoolDTO();
            apiBoolDto.Value = BLL.UserGroupRight.DeleteUserGroupRights(id);
            return apiBoolDto;

        }

        [UserAuth(Permission = "Administrator")]
        public IEnumerable<UserGroupImageManagementEntity> GetImageManagements(int id)
        {
            return BLL.UserGroupImageManagement.Get(id);
        }

        [UserAuth(Permission = "Administrator")]
        public ApiBoolDTO DeleteImageManagements(int id)
        {
            var apiBoolDto = new ApiBoolDTO();
            apiBoolDto.Value = BLL.UserGroupImageManagement.DeleteUserGroupImageManagements(id);
            return apiBoolDto;

        }

        [UserAuth(Permission = "Administrator")]
        public IEnumerable<UserGroupGroupManagementEntity> GetGroupManagements(int id)
        {
            return BLL.UserGroupGroupManagement.Get(id);
        }

        [UserAuth(Permission = "Administrator")]
        public ApiBoolDTO DeleteGroupManagements(int id)
        {
            var apiBoolDto = new ApiBoolDTO();
            apiBoolDto.Value = BLL.UserGroupGroupManagement.DeleteUserGroupGroupManagements(id);
            return apiBoolDto;

        }
     
    }
}
