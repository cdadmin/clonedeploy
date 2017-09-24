using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;
using CloneDeploy_Services.Workflows;

namespace CloneDeploy_App.Controllers
{
    public class GroupController : ApiController
    {
        private readonly GroupServices _groupServices;
        private readonly int _userId;

        public GroupController()
        {
            _groupServices = new GroupServices();
            _userId = Convert.ToInt32(((ClaimsIdentity) User.Identity).Claims.Where(c => c.Type == "user_id")
                .Select(c => c.Value).SingleOrDefault());
        }

        [CustomAuth(Permission = "GroupDelete")]
        public ActionResultDTO Delete(int id)
        {
            var result = _groupServices.DeleteGroup(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [CustomAuth(Permission = AuthorizationStrings.DeleteGroup)]
        public ApiBoolResponseDTO DeleteImageClassifications(int id)
        {
            return new ApiBoolResponseDTO {Value = _groupServices.DeleteGroupImageClassifications(id)};
        }

        [CustomAuth(Permission = "GroupRead")]
        [HttpGet]
        public ApiBoolResponseDTO Export(string path)
        {
            _groupServices.ExportCsv(path);
            return new ApiBoolResponseDTO {Value = true};
        }

        [Authorize]
        public GroupEntity Get(int id)
        {
            var result = _groupServices.GetGroup(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [Authorize]
        public IEnumerable<GroupWithImage> Get(string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? _groupServices.SearchGroupsForUser(Convert.ToInt32(_userId))
                : _groupServices.SearchGroupsForUser(Convert.ToInt32(_userId), searchstring);
        }

        [Authorize]
        public ApiStringResponseDTO GetCount()
        {
            return new ApiStringResponseDTO {Value = _groupServices.GroupCountUser(Convert.ToInt32(_userId))};
        }

        [CustomAuth(Permission = "GroupRead")]
        public GroupBootMenuEntity GetCustomBootMenu(int id)
        {
            var result = _groupServices.GetGroupBootMenu(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [CustomAuth(Permission = "GroupRead")]
        public ApiStringResponseDTO GetEffectiveManifest(int id)
        {
            var effectiveManifest = new EffectiveMunkiTemplate().Group(id);
            return new ApiStringResponseDTO {Value = Encoding.UTF8.GetString(effectiveManifest.ToArray())};
        }

        [CustomAuth(Permission = "GroupRead")]
        public IEnumerable<ComputerWithImage> GetGroupMembers(int id, string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? _groupServices.GetGroupMembersWithImages(id)
                : _groupServices.GetGroupMembersWithImages(id, searchstring);
        }

        [HttpGet]
        [CustomAuth(Permission = "GroupRead")]
        public GroupPropertyEntity GetGroupProperties(int id)
        {
            var result = _groupServices.GetGroupProperty(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [CustomAuth(Permission = AuthorizationStrings.ReadGroup)]
        public IEnumerable<GroupImageClassificationEntity> GetImageClassifications(int id)
        {
            return _groupServices.GetGroupImageClassifications(id);
        }

        [Authorize]
        public ApiStringResponseDTO GetMemberCount(int id)
        {
            return new ApiStringResponseDTO {Value = _groupServices.GetGroupMemberCount(id)};
        }

        [CustomAuth(Permission = "GroupRead")]
        public IEnumerable<GroupMunkiEntity> GetMunkiTemplates(int id)
        {
            return _groupServices.GetGroupMunkiTemplates(id);
        }

        [CustomAuth(Permission = "GroupCreate")]
        [HttpPost]
        public ApiIntResponseDTO Import(ApiStringResponseDTO csvContents)
        {
            return new ApiIntResponseDTO {Value = _groupServices.ImportCsv(csvContents.Value, Convert.ToInt32(_userId))};
        }

        [CustomAuth(Permission = "GroupCreate")]
        public ActionResultDTO Post(GroupEntity group)
        {
            var result = _groupServices.AddGroup(group, Convert.ToInt32(_userId));
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [CustomAuth(Permission = "GroupUpdate")]
        public ActionResultDTO Put(int id, GroupEntity group)
        {
            group.Id = id;
            var result = _groupServices.UpdateGroup(group);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [Authorize]
        public ApiBoolResponseDTO ReCalcSmart()
        {
            _groupServices.UpdateAllSmartGroupsMembers();
            return new ApiBoolResponseDTO {Value = true};
        }

        [HttpGet]
        [CustomAuth(Permission = "GroupUpdate")]
        public ApiBoolResponseDTO RemoveGroupMember(int id, int computerId)
        {
            return new ApiBoolResponseDTO {Value = _groupServices.DeleteMembership(computerId, id)};
        }

        [HttpDelete]
        [CustomAuth(Permission = "GroupUpdate")]
        public ApiBoolResponseDTO RemoveMunkiTemplates(int id)
        {
            return new ApiBoolResponseDTO {Value = _groupServices.DeleteMunkiTemplates(id)};
        }

        [HttpGet]
        [CustomAuth(Permission = "ImageTaskDeployGroup")]
        public ApiIntResponseDTO StartGroupUnicast(int id)
        {
            return new ApiIntResponseDTO
            {
                Value = _groupServices.StartGroupUnicast(id, Convert.ToInt32(_userId), Request.GetClientIpAddress())
            };
        }

        [CustomAuth(Permission = "ImageTaskMulticast")]
        [HttpGet]
        public ApiStringResponseDTO StartMulticast(int id)
        {
            return new ApiStringResponseDTO
            {
                Value = new Multicast(id, Convert.ToInt32(_userId), Request.GetClientIpAddress()).Create()
            };
        }

        [HttpGet]
        [CustomAuth(Permission = "SmartUpdate")]
        public ActionResultDTO UpdateSmartMembership(int id)
        {
            return _groupServices.UpdateSmartMembership(id);
        }
    }
}