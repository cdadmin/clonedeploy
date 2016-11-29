using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web.Http;
using CloneDeploy_App.BLL;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_App.DTOs;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;


namespace CloneDeploy_App.Controllers
{
    

    public class GroupController : ApiController
    {
        [GroupAuth(Permission = "GroupSearch")]
        public IEnumerable<GroupEntity> Get(string searchstring = "")
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();
            return string.IsNullOrEmpty(searchstring)
                ? BLL.Group.SearchGroupsForUser(Convert.ToInt32(userId))
                : BLL.Group.SearchGroupsForUser(Convert.ToInt32(userId), searchstring);

        }

        [GroupAuth(Permission = "GroupSearch")]
        public ApiDTO GetCount()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();

            return BLL.Group.GroupCountUser(Convert.ToInt32(userId));
        }

        [GroupAuth(Permission = "GroupRead")]
        public IHttpActionResult Get(int id)
        {
            var group = BLL.Group.GetGroup(id);
            if (group == null)
                return Content(HttpStatusCode.NotFound, new ActionResultEntity());
            else
                return Ok(group);
        }

        [HttpGet]
        [GroupAuth(Permission = "GroupUpdate")]
        public IHttpActionResult RemoveGroupMember(int id, int computerId)
        {
            var result = BLL.GroupMembership.DeleteMembership(computerId, id);
            if (!result)
                return NotFound();
            else
                return Ok();
        }

        [HttpGet]
        [GroupAuth(Permission = "GroupUpdate")]
        public IHttpActionResult RemoveMunkiTemplate(int id)
        {
            var result = BLL.GroupMunki.DeleteMunkiTemplates(id);
            if (!result)
                return NotFound();
            else
                return Ok();
        }

        [HttpGet]
        [GroupAuth(Permission = "GroupRead")]
        public List<GroupMunkiEntity> GetMunkiTemplates(int id)
        {
            return BLL.GroupMunki.Get(id);
        }

        [HttpGet]
        [GroupAuth(Permission = "GroupRead")]
        public GroupPropertyEntity GetGroupProperties(int id)
        {
            return BLL.GroupProperty.GetGroupProperty(id);
        }

        [GroupAuth(Permission = "GroupRead")]
        public IHttpActionResult GetMemberCount(int id)
        {
            var group = BLL.GroupMembership.GetGroupMemberCount(id);
            if (group == null)
                return Content(HttpStatusCode.NotFound, new ActionResultEntity());
            else
                return Ok(group);
        }
       

        [GroupAuth(Permission = "GroupCreate")]
        public ActionResultEntity Post(GroupEntity group)
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();

            var actionResult = BLL.Group.AddGroup(group, Convert.ToInt32(userId));
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }

        [GroupAuth(Permission = "GroupUpdate")]
        public ActionResultEntity Put(int id, GroupEntity group)
        {
            group.Id = id;
            var actionResult = BLL.Group.UpdateGroup(group);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }

        [GroupAuth(Permission = "GroupDelete")]
        public ActionResultEntity Delete(int id)
        {
            var actionResult = BLL.Group.DeleteGroup(id);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }

        [HttpPost]
        [GroupAuth(Permission = "GroupUpdate")]
        public ApiBoolDTO UpdateSmartMembership(GroupEntity group)
        {
            var apiBoolDto = new ApiBoolDTO();
            apiBoolDto.Value = BLL.Group.UpdateSmartMembership(group);
           return apiBoolDto;
        }

        [HttpPost]
        [TaskAuth(Permission = "ImageTaskDeploy")]
        public ApiDTO StartGroupUnicast(GroupEntity group)
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();
            var apiDto = new ApiDTO();
            apiDto.Value = BLL.Group.StartGroupUnicast(group, Convert.ToInt32(userId)).ToString();
            return apiDto;
        }

        [GroupAuth(Permission = "GroupRead")]
        public IEnumerable<ComputerEntity> GetGroupMembers(int id, string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? BLL.Group.GetGroupMembers(id)
                : BLL.Group.GetGroupMembers(id, searchstring);

        }
    }
}
