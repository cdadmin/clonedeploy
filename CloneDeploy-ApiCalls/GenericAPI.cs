using System.Collections.Generic;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
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

        public List<T> GetAll(int limit, string searchstring)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetAll",_resource);
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

        public string GetCount()
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetCount", _resource);
            var responseData = new ApiRequest().Execute<ApiStringResponseDTO>(_request);
            return responseData != null ? responseData.Value : string.Empty;

        }

        public ActionResultDTO Put(int id,T tObject)
        {
            _request.Method = Method.PUT;
            _request.AddJsonBody(tObject);
            _request.Resource = string.Format("api/{0}/Put/{1}",_resource, id);
            var response = new ApiRequest().Execute<ActionResultDTO>(_request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }

        public ActionResultDTO Post(T tObject)
        {
            _request.Method = Method.POST;
            _request.AddJsonBody(tObject);
            _request.Resource = string.Format("api/{0}/Post/",_resource);
            var response = new ApiRequest().Execute<ActionResultDTO>(_request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }

        public ActionResultDTO Delete(int id)
        {
            _request.Method = Method.DELETE;
            _request.Resource = string.Format("api/{0}/Delete/{1}",_resource, id);
            var response = new ApiRequest().Execute<ActionResultDTO>(_request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }
    }
}


