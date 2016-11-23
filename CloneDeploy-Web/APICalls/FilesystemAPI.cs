using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestSharp;

namespace CloneDeploy_Web.APICalls
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

        public Models.ApiBoolDTO BootSdiExists()
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/BootSdiExists/", _resource);
            var response = new ApiRequest().Execute<Models.ApiBoolDTO>(_request);

            return response;

        }
    }
}