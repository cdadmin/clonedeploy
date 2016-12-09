using System.Collections.Generic;
using System.IO;

using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
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

        public string ReadFileText(string path)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/ReadFileText/", _resource);
            _request.AddParameter("path", path);
            var response = new ApiRequest().Execute<ApiStringResponseDTO>(_request).Value;

            return response;

        }

        public bool SetUnixPermissions(string path)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/SetUnixPermissions/", _resource);
            _request.AddParameter("path", path);
            var response = new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;

            return response;

        }

        public List<string> GetKernels()
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetKernels/", _resource);
            var response = new ApiRequest().Execute<List<string>>(_request);

            return response;

        }

        public List<string> GetBootImages()
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetBootImages/", _resource);
            var response = new ApiRequest().Execute<List<string>>(_request);

            return response;

        }

        public List<string> GetLogs()
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetLogs/", _resource);
            var response = new ApiRequest().Execute<List<string>>(_request);

            return response;

        }

        public List<string> GetScripts(string type)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetScripts/", _resource);
            _request.AddParameter("type", type);
            var response = new ApiRequest().Execute<List<string>>(_request);

            return response;

        }

        public List<string> GetThinImages()
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetThinImages/", _resource);
            var response = new ApiRequest().Execute<List<string>>(_request);

            return response;

        }


        public MunkiPackageInfoEntity GetPlist(string file)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetPlist/", _resource);
            _request.AddParameter("file", file);
            var response = new ApiRequest().Execute<MunkiPackageInfoEntity>(_request);

            return response;
        }


        public List<FileInfo> GetMunkiResources(string resourceType)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetMunkiResources/", _resource);
            _request.AddParameter("resourceType", resourceType);
            var response = new ApiRequest().Execute<List<FileInfo>>(_request);

            return response;
        }
    }
}