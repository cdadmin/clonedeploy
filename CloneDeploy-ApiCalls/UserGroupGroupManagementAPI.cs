using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class UserGroupGroupManagementAPI : BaseAPI
    {
        private readonly ApiRequest _apiRequest;

        public UserGroupGroupManagementAPI(string resource) : base(resource)
        {
            _apiRequest = new ApiRequest();
        }

        public ActionResultDTO Post(List<UserGroupGroupManagementEntity> listOfGroups)
        {
            Request.Method = Method.POST;
            Request.Resource = string.Format("api/{0}/Post/", Resource);
            Request.AddJsonBody(listOfGroups);
            return _apiRequest.Execute<ActionResultDTO>(Request);
        }
    }
}