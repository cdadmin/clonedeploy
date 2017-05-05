using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    /// <summary>
    ///     Summary description for User
    /// </summary>
    public class ServiceAccountAPI : BaseAPI
    {
        private readonly ApiRequest _apiRequest;

        public ServiceAccountAPI(string resource) : base(resource)
        {
            _apiRequest = new ApiRequest();
        }

        public ServiceAccountAPI(string resource, CustomApiCallDTO cApiDto)
            : base(resource)
        {
            _apiRequest = new ApiRequest(cApiDto.Token, cApiDto.BaseUrl);
        }

        public bool CancelAllImagingTasks()
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/CancelAllImagingTasks/", Resource);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }

        public bool DeleteTftpFile(string path)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/DeleteTftpFile/", Resource);
            Request.AddParameter("path", path);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }

        public int GetMulticastSenderArgs(MulticastArgsDTO multicastArgs)
        {
            Request.Method = Method.POST;
            Request.Resource = string.Format("api/{0}/GetMulticastSenderArgs/", Resource);
            Request.AddJsonBody(multicastArgs);
            var response = _apiRequest.Execute<ApiIntResponseDTO>(Request);
            return response != null ? response.Value : 0;
        }

        public ServerRoleDTO GetServerRoles()
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetServerRoles/", Resource);
            return _apiRequest.Execute<ServerRoleDTO>(Request);
        }

        public string GetTftpServer()
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetTftpServer/", Resource);
            var response = _apiRequest.Execute<ApiStringResponseDTO>(Request);
            return response != null ? response.Value : string.Empty;
        }

        public bool Test()
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/Test", Resource);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }


        public bool WriteTftpFile(TftpFileDTO tftpFile)
        {
            Request.Method = Method.POST;
            Request.Resource = string.Format("api/{0}/WriteTftpFile/", Resource);
            Request.AddJsonBody(tftpFile);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }
    }
}