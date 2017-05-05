using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class ClusterGroupDistributionPointAPI : BaseAPI
    {
        private readonly ApiRequest _apiRequest;

        public ClusterGroupDistributionPointAPI(string resource) : base(resource)
        {
            _apiRequest = new ApiRequest();
        }

        public ActionResultDTO Post(List<ClusterGroupDistributionPointEntity> listOfDps)
        {
            Request.Method = Method.POST;
            Request.Resource = string.Format("api/{0}/Post/", Resource);
            Request.AddJsonBody(listOfDps);
            return _apiRequest.Execute<ActionResultDTO>(Request);
        }
    }
}