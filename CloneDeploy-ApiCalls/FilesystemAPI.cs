using System.Collections.Generic;
using System.IO;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class FilesystemAPI : BaseAPI
    {
        private readonly ApiRequest _apiRequest;

        public FilesystemAPI(string resource) : base(resource)
        {
            _apiRequest = new ApiRequest();
        }

        public bool BootSdiExists()
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/BootSdiExists/", Resource);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);

            return response != null && response.Value;
        }

        public bool EditDefaultBootMenu(CoreScriptDTO menu)
        {
            Request.Method = Method.POST;
            Request.Resource = string.Format("api/{0}/EditDefaultBootMenu/", Resource);
            Request.AddJsonBody(menu);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);

            return response != null && response.Value;
        }

        public List<string> GetBootImages()
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetBootImages/", Resource);
            var result = _apiRequest.Execute<List<string>>(Request);

            if (result == null)
                return new List<string>();
            else
                return result;
        }

        public string GetDefaultBootFilePath(string type)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetDefaultBootFilePath/", Resource);
            Request.AddParameter("type", type);
            var response = _apiRequest.Execute<ApiStringResponseDTO>(Request);

            return response != null ? response.Value : string.Empty;
        }

        public DpFreeSpaceDTO GetDpFreeSpace()
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetDpFreeSpace/", Resource);
            var response = _apiRequest.Execute<DpFreeSpaceDTO>(Request);

            return response;
        }

        public List<string> GetKernels()
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetKernels/", Resource);
            var result = _apiRequest.Execute<List<string>>(Request);

            if (result == null)
                return new List<string>();
            else
                return result;
        }

        public IEnumerable<string> GetLogContents(string name, int limit)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetLogContents/", Resource);
            Request.AddParameter("name", name);
            Request.AddParameter("limit", limit);
            var result = _apiRequest.Execute<List<string>>(Request);

            if (result == null)
                return new List<string>();
            else
                return result;
        }

        public List<string> GetLogs()
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetLogs/", Resource);
            var result = _apiRequest.Execute<List<string>>(Request);

            if (result == null)
                return new List<string>();
            else
                return result;
        }

        public List<string> GetScripts(string type)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetScripts/", Resource);
            Request.AddParameter("type", type);
            var result = _apiRequest.Execute<List<string>>(Request);

            if (result == null)
                return new List<string>();
            else
                return result;
        }

        public string GetServerPaths(string type, string subType)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetServerPaths/", Resource);
            Request.AddParameter("type", type);
            Request.AddParameter("subType", subType);
            var response = _apiRequest.Execute<ApiStringResponseDTO>(Request);

            return response != null ? response.Value : string.Empty;
        }

        public string ReadFileText(string path)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/ReadFileText/", Resource);
            Request.AddParameter("path", path);
            var response = _apiRequest.Execute<ApiStringResponseDTO>(Request).Value;

            return response;
        }

        public bool SetUnixPermissions(string path)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/SetUnixPermissions/", Resource);
            Request.AddParameter("path", path);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);

            return response != null && response.Value;
        }

        public bool WriteCoreScript(CoreScriptDTO script)
        {
            Request.Method = Method.POST;
            Request.Resource = string.Format("api/{0}/WriteCoreScript/", Resource);
            Request.AddJsonBody(script);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);

            return response != null && response.Value;
        }
    }
}