using CloneDeploy_App.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class FilesystemAPI
    {
        private readonly RestRequest _request;     
        private readonly string _resource;

        public FilesystemAPI(string resource)
        {
            _request = new RestRequest();
            _resource = resource;
        }

        public bool BootSdiExists()
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/BootSdiExists/", _resource);
            var response = new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;

            return response;

        }
    }
}