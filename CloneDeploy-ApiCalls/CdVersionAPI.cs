using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class CdVersionAPI : BaseAPI
    {
        public CdVersionAPI(string resource):base(resource)
        {
		
        }

        public CdVersionEntity Get(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/Get/{1}", _resource, id);
            return new ApiRequest().Execute<CdVersionEntity>(_request);
        }

       

        public ActionResultDTO Put(int id, CdVersionEntity tObject)
        {
            _request.Method = Method.PUT;
            _request.AddJsonBody(tObject);
            _request.Resource = string.Format("api/{0}/Put/{1}", _resource, id);
            var response = new ApiRequest().Execute<ActionResultDTO>(_request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }

        public bool IsFirstRunCompleted()
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/IsFirstRunCompleted", _resource);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;
        }

      
    }
}
