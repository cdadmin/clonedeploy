using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class ClusterGroupDistributionPointAPI : BaseAPI
    {
        public ClusterGroupDistributionPointAPI(string resource):base(resource)
        {
		
        }
  
        public ActionResultDTO Post(List<ClusterGroupDistributionPointEntity> listOfDps)
        {
            _request.Method = Method.POST;
            _request.Resource = string.Format("api/{0}/Post/", _resource);
            _request.AddJsonBody(listOfDps);
            return new ApiRequest().Execute<ActionResultDTO>(_request);
        }

        
    }
}