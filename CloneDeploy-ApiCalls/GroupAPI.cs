using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    /// <summary>
    /// Summary description for User
    /// </summary>
    public class GroupAPI: GenericAPI<GroupEntity>
    {
        public GroupAPI(string resource):base(resource)
        {
		
        }

        public string GetMemberCount(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetMemberCount/{1}", _resource,id);
            return new ApiRequest().Execute<ApiStringResponseDTO>(_request).Value;
        }


        [HttpGet]
        [GroupAuth(Permission = "GroupUpdate")]
        public ApiBoolResponseDTO RemoveGroupMember(int id, int computerId)
        {
            return new ApiBoolResponseDTO() { Value = _groupServices.DeleteMembership(computerId, id) };
        }

        [HttpGet]
        [GroupAuth(Permission = "GroupUpdate")]
        public ApiBoolResponseDTO RemoveMunkiTemplate(int id)
        {
            return new ApiBoolResponseDTO() { Value = _groupServices.DeleteMunkiTemplates(id) };
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
            return new ApiStringResponseDTO() { Value = _groupServices.GetGroupMemberCount(id) };
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