using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class ClusterGroupServerAPI : BaseAPI
    {
        private readonly ApiRequest _apiRequest;

        public ClusterGroupServerAPI(string resource) : base(resource)
        {
            _apiRequest = new ApiRequest();
        }

        public ActionResultDTO Post(List<ClusterGroupServerEntity> listOfServers)
        {
            Request.Method = Method.POST;
            Request.Resource = string.Format("api/{0}/Post/", Resource);
            Request.AddJsonBody(listOfServers);
            return _apiRequest.Execute<ActionResultDTO>(Request);
        }
    }
}