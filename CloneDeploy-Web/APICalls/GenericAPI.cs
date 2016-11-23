using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using CloneDeploy_Web.Models;
using Helpers;
using RestSharp;

namespace CloneDeploy_Web.APICalls
{
    public class GenericAPI<T> : IGenericAPI<T> where T : new()
    {
        protected readonly RestRequest _request;     
        protected readonly string _resource;

        public GenericAPI(string resource)
        {
            _request = new RestRequest();
            _resource = resource;
        }

        public List<T> Get(int limit, string searchstring)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/Get",_resource);
            _request.AddParameter("limit", limit);
            _request.AddParameter("searchstring", searchstring);
            return new ApiRequest().Execute<List<T>>(_request);
        }

        public List<T> Get(string searchstring)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/Get", _resource);
            _request.AddParameter("searchstring", searchstring);
            return new ApiRequest().Execute<List<T>>(_request);
        }

        public T Get(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/Get/{1}",_resource, id);
            return new ApiRequest().Execute<T>(_request);
        }

        public ApiDTO GetCount()
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetCount", _resource);
            return new ApiRequest().Execute<ApiDTO>(_request);
        }

        public ActionResult Put(int id,T tObject)
        {
            _request.Method = Method.PUT;
            _request.AddJsonBody(tObject);
            _request.Resource = string.Format("api/{0}/Put/{1}",_resource, id);
            var response = new ApiRequest().Execute<ActionResult>(_request);
            if (response.ObjectId == 0)
                response.Success = false;
            return response;
        }

        public ActionResult Post(T tObject)
        {
            _request.Method = Method.POST;
            _request.AddJsonBody(tObject);
            _request.Resource = string.Format("api/{0}/Post/",_resource);
            var response = new ApiRequest().Execute<ActionResult>(_request);
            if (response.ObjectId == 0)
                response.Success = false;
            return response;
        }

        public ActionResult Delete(int id)
        {
            _request.Method = Method.DELETE;
            _request.Resource = string.Format("api/{0}/Delete/{1}",_resource, id);
            var response = new ApiRequest().Execute<ActionResult>(_request);
            if (response.ObjectId == 0)
                response.Success = false;
            return response;
        }
    }
}


