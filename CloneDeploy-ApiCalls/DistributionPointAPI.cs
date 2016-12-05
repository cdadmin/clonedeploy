using CloneDeploy_Entities;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class DistributionPointAPI:GenericAPI<DistributionPointEntity>
    {
        public DistributionPointAPI(string resource):base(resource)
        {
		
        }
    
       

        public DistributionPointEntity GetPrimary()
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetPrimary/", _resource);
            return new ApiRequest().Execute<DistributionPointEntity>(_request);
        }

       
    }
}