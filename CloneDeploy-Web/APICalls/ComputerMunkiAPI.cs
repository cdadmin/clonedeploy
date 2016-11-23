using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CloneDeploy_Web.APICalls
{
    public class ComputerMunkiAPI: GenericAPI<Models.ComputerMunki>
    {
        public ComputerMunkiAPI(string resource):base(resource)
        {
		
        }

        public new List<Models.ComputerMunki> Get(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/Get/{1}", _resource,id);
            return new ApiRequest().Execute<List<Models.ComputerMunki>>(_request);
        }
        public List<Models.ComputerMunki> GetTemplateComputers(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetTemplateComputers/{1}", _resource,id);
            return new ApiRequest().Execute<List<Models.ComputerMunki>>(_request);
        }

        public string GetEffectiveManifest(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetEffectiveManifest/{1}", _resource, id);
            return new ApiRequest().Execute<Models.ApiDTO>(_request).Value;
        }
    
    }
}