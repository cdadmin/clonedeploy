using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class ClusterGroupAPI : GenericAPI<ClusterGroupEntity>
    {
        public ClusterGroupAPI(string resource):base(resource)
        {
		
        }
  
        public IEnumerable<ClusterGroupServerEntity> GetClusterServers(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetClusterServers/{1}", _resource, id);
            return new ApiRequest().Execute<List<ClusterGroupServerEntity>>(_request);
        }

        
    }
}