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
using CloneDeploy_Services;


namespace CloneDeploy_App.Controllers
{
    

    public class GroupController : ApiController
    {
        private readonly GroupServices _groupServices;

        public GroupController()
        {
            _groupServices = new GroupServices();
        }


        [GroupAuth(Permission = "GroupSearch")]
        public IEnumerable<GroupEntity> GetAll(string searchstring = "")
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();
            return string.IsNullOrEmpty(searchstring)
                ? _groupServices.SearchGroupsForUser(Convert.ToInt32(userId))
                : _groupServices.SearchGroupsForUser(Convert.ToInt32(userId), searchstring);

        }

        [GroupAuth(Permission = "GroupSearch")]
        public ApiStringResponseDTO GetCount()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();

            return new ApiStringResponseDTO() {Value = _groupServices.GroupCountUser(Convert.ToInt32(userId))};
        }

        [GroupAuth(Permission = "GroupRead")]
        public GroupEntity Get(int id)
        {
            var result = _groupServices.GetGroup(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [HttpGet]
        [GroupAuth(Permission = "GroupUpdate")]
        public ApiBoolResponseDTO RemoveGroupMember(int id, int computerId)
        {
            return new ApiBoolResponseDTO() {Value = _groupServices.DeleteMembership(computerId, id)};
        }

        [HttpGet]
        [GroupAuth(Permission = "GroupUpdate")]
        public ApiBoolResponseDTO RemoveMunkiTemplate(int id)
        {
            return new ApiBoolResponseDTO() {Value = _groupServices.DeleteMunkiTemplates(id)};
        }

        [HttpGet]
        [GroupAuth(Permission = "GroupRead")]
        public IEnumerable<GroupMunkiEntity> GetMunkiTemplates(int id)
        {
            return _groupServices.GetGroupMunkiTemplates(id);
        }

        [HttpGet]
        [GroupAuth(Permission = "GroupRead")]
        public GroupPropertyEntity GetGroupProperties(int id)
        {
            var result = _groupServices.GetGroupProperty(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [GroupAuth(Permission = "GroupRead")]
        public ApiStringResponseDTO GetMemberCount(int id)
        {
            return new ApiStringResponseDTO() {Value = _groupServices.GetGroupMemberCount(id)};
        }
       

        [GroupAuth(Permission = "GroupCreate")]
        public ActionResultDTO Post(GroupEntity group)
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();

            var result = _groupServices.AddGroup(group, Convert.ToInt32(userId));
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [GroupAuth(Permission = "GroupUpdate")]
        public ActionResultDTO Put(int id, GroupEntity group)
        {
            group.Id = id;
            var result = _groupServices.UpdateGroup(group);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [GroupAuth(Permission = "GroupDelete")]
        public ActionResultDTO Delete(int id)
        {
            var result = _groupServices.DeleteGroup(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [HttpPost]
        [GroupAuth(Permission = "GroupUpdate")]
        public ActionResultDTO UpdateSmartMembership(GroupEntity group)
        {
            return _groupServices.UpdateSmartMembership(group);
        }

        [HttpPost]
        [TaskAuth(Permission = "ImageTaskDeploy")]
        public ApiIntResponseDTO StartGroupUnicast(GroupEntity group)
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();

            return new ApiIntResponseDTO()
            {
                Value = _groupServices.StartGroupUnicast(group, Convert.ToInt32(userId))
            };

        }

        [GroupAuth(Permission = "GroupRead")]
        public IEnumerable<ComputerEntity> GetGroupMembers(int id, string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? _groupServices.GetGroupMembers(id)
                : _groupServices.GetGroupMembers(id, searchstring);

        }

        [GroupAuth(Permission = "GroupRead")]
        public GroupBootMenuEntity GetCustomBootMenu(int id)
        {
            var result = _groupServices.GetGroupBootMenu(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }
    }
}
