using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class WorkflowAPI : BaseAPI
    {
        private readonly ApiRequest _apiRequest;

        public WorkflowAPI(string resource) : base(resource)
        {
            _apiRequest = new ApiRequest();
        }

        public bool CancelAllImagingTasks()
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/CancelAllImagingTasks/", Resource);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }

        public bool CopyPxeBinaries()
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/CopyPxeBinaries/", Resource);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }

        public bool CreateClobberBootMenu(int profileId, bool promptComputerName)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/CreateClobberBootMenu/", Resource);
            Request.AddParameter("profileId", profileId);
            Request.AddParameter("promptComputerName", promptComputerName);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }

        public bool CreateDefaultBootMenu(BootMenuGenOptionsDTO defaultMenuOptions)
        {
            Request.Method = Method.POST;
            Request.Resource = string.Format("api/{0}/CreateDefaultBootMenu/", Resource);
            Request.AddJsonBody(defaultMenuOptions);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }

        public byte[] GenerateLinuxIsoConfig(IsoGenOptionsDTO isoOptions)
        {
            Request.Method = Method.POST;
            Request.Resource = string.Format("api/{0}/GenerateLinuxIsoConfig/", Resource);
            Request.AddJsonBody(isoOptions);
            return _apiRequest.ExecuteRaw(Request);
        }

        public AppleVendorDTO GetAppleVendorString(string ip)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetAppleVendorString/", Resource);
            Request.AddParameter("ip", ip);
            var response = _apiRequest.Execute<AppleVendorDTO>(Request);
            return response;
        }

        public string StartOnDemandMulticast(int profileId, string clientCount,int clusterId)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/StartOnDemandMulticast/", Resource);
            Request.AddParameter("profileId", profileId);
            Request.AddParameter("clientCount", clientCount);
            Request.AddParameter("clusterId", clusterId);
            var response = _apiRequest.Execute<ApiStringResponseDTO>(Request);
            return response != null ? response.Value : string.Empty;
        }
    }
}