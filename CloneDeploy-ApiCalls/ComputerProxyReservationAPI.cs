using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class ComputerProxyReservationAPI : BaseAPI
    {
        private readonly ApiRequest _apiRequest;

        public ComputerProxyReservationAPI(string resource) : base(resource)
        {
            _apiRequest = new ApiRequest();
        }

        public ActionResultDTO Post(ComputerProxyReservationEntity tObject)
        {
            Request.Method = Method.POST;
            Request.AddJsonBody(tObject);
            Request.Resource = string.Format("api/{0}/Post/", Resource);
            var response = _apiRequest.Execute<ActionResultDTO>(Request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }
    }
}