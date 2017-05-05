using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class GroupMembershipAPI : BaseAPI
    {
        private readonly ApiRequest _apiRequest;

        public GroupMembershipAPI(string resource) : base(resource)
        {
            _apiRequest = new ApiRequest();
        }

        public ActionResultDTO Post(List<GroupMembershipEntity> groupMemberships)
        {
            Request.Method = Method.POST;
            Request.Resource = string.Format("api/{0}/Post/", Resource);
            Request.AddJsonBody(groupMemberships);
            return _apiRequest.Execute<ActionResultDTO>(Request);
        }
    }
}