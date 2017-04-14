using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class SecondaryServerAPI : BaseAPI
    {
        public SecondaryServerAPI(string resource):base(resource)
        {
		
        }

        public List<SecondaryServerEntity> GetAll(int limit, string searchstring)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetAll", _resource);
            _request.AddParameter("limit", limit);
            _request.AddParameter("searchstring", searchstring);
            return new ApiRequest().Execute<List<SecondaryServerEntity>>(_request);
        }

      

        public SecondaryServerEntity Get(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/Get/{1}", _resource, id);
            return new ApiRequest().Execute<SecondaryServerEntity>(_request);
        }

    

        public ActionResultDTO Put(int id, SecondaryServerEntity tObject)
        {
            _request.Method = Method.PUT;
            _request.AddJsonBody(tObject);
            _request.Resource = string.Format("api/{0}/Put/{1}", _resource, id);
            var response = new ApiRequest().Execute<ActionResultDTO>(_request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }

        public ActionResultDTO Post(SecondaryServerEntity tObject)
        {
            _request.Method = Method.POST;
            _request.AddJsonBody(tObject);
            _request.Resource = string.Format("api/{0}/Post/", _resource);
            var response = new ApiRequest().Execute<ActionResultDTO>(_request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }

        public ActionResultDTO Delete(int id)
        {
            _request.Method = Method.DELETE;
            _request.Resource = string.Format("api/{0}/Delete/{1}", _resource, id);
            var response = new ApiRequest().Execute<ActionResultDTO>(_request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }

        public string GetServerOS()
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetServerOS", _resource);
            var response = new ApiRequest().Execute<ApiStringResponseDTO>(_request);
            return response != null ? response.Value : null;
        }

       

    

        
    }
}