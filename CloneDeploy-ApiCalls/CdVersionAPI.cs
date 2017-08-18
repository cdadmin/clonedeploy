using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class CdVersionAPI : BaseAPI
    {
        private readonly ApiRequest _apiRequest;

        public CdVersionAPI(string resource) : base(resource)
        {
            _apiRequest = new ApiRequest();
        }

        public CdVersionEntity Get(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/Get/{1}", Resource, id);
            return _apiRequest.Execute<CdVersionEntity>(Request);
        }

        public bool IsFirstRunCompleted()
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/IsFirstRunCompleted", Resource);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }

        public ActionResultDTO Put(int id, CdVersionEntity tObject)
        {
            Request.Method = Method.PUT;
            Request.AddJsonBody(tObject);
            Request.Resource = string.Format("api/{0}/Put/{1}", Resource, id);
            var response = _apiRequest.Execute<ActionResultDTO>(Request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }
    }
}