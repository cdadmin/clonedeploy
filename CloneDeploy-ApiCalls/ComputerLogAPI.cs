using System.Collections.Generic;
using CloneDeploy_Entities;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class ComputerLogAPI:GenericAPI<ComputerLogEntity>
    {
        public ComputerLogAPI(string resource):base(resource)
        {
		
        }


        public IEnumerable<ComputerLogEntity> GetOnDemandLogs(int limit = 0)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetOnDemandLogs/", _resource);
            _request.AddParameter("limit", limit);
            return new ApiRequest().Execute<List<ComputerLogEntity>>(_request);
        }

      

      
    }
}