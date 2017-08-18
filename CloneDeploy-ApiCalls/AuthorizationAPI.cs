using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class AuthorizationAPI : BaseAPI
    {
        private readonly ApiRequest _apiRequest;

        public AuthorizationAPI(string resource) : base(resource)
        {
            _apiRequest = new ApiRequest();
        }

        public bool ComputerManagement(string requiredRight, int computerId)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/IsAuthorized/", Resource);
            Request.AddParameter("requiredRight", requiredRight);
            Request.AddParameter("computerId", computerId);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }

        public bool GroupManagement(string requiredRight, int groupId)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/IsAuthorized/", Resource);
            Request.AddParameter("requiredRight", requiredRight);
            Request.AddParameter("groupId", groupId);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }

        public bool ImageManagement(string requiredRight, int imageId)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/IsAuthorized/", Resource);
            Request.AddParameter("requiredRight", requiredRight);
            Request.AddParameter("imageId", imageId);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }

        public bool IsAuthorized(string requiredRight)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/IsAuthorized/", Resource);
            Request.AddParameter("requiredRight", requiredRight);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }
    }
}