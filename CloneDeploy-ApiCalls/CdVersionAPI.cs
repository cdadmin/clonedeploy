using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class CdVersionAPI:GenericAPI<CdVersionEntity>
    {
        public CdVersionAPI(string resource):base(resource)
        {
		
        }

        public bool IsFirstRunCompleted()
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/IsFirstRunCompleted", _resource);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;
        }

      
    }
}
