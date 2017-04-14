using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class AuthorizationAPI : BaseAPI
    {
      
        public AuthorizationAPI(string resource):base (resource)
        {
           
        }

        public bool IsAuthorized(string requiredRight)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/IsAuthorized/", _resource);
            _request.AddParameter("requiredRight", requiredRight);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;
        }


        public bool ComputerManagement(string requiredRight, int computerId)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/IsAuthorized/", _resource);
            _request.AddParameter("requiredRight", requiredRight);
            _request.AddParameter("computerId", computerId);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;
        }

        public bool GroupManagement(string requiredRight, int groupId)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/IsAuthorized/", _resource);
            _request.AddParameter("requiredRight", requiredRight);
            _request.AddParameter("groupId", groupId);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;
        }


        public bool ImageManagement(string requiredRight, int imageId)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/IsAuthorized/", _resource);
            _request.AddParameter("requiredRight", requiredRight);
            _request.AddParameter("imageId", imageId);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;
        }

      
    }
}
