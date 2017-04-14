using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class BuildingAPI : BaseAPI
    {
        public BuildingAPI(string resource):base(resource)
        {
		
        }

        public List<BuildingEntity> GetAll(int limit, string searchstring)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetAll", _resource);
            _request.AddParameter("limit", limit);
            _request.AddParameter("searchstring", searchstring);
            return new ApiRequest().Execute<List<BuildingEntity>>(_request);
        }


        public BuildingEntity Get(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/Get/{1}", _resource, id);
            return new ApiRequest().Execute<BuildingEntity>(_request);
        }

        public string GetCount()
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetCount", _resource);
            var responseData = new ApiRequest().Execute<ApiStringResponseDTO>(_request);
            return responseData != null ? responseData.Value : string.Empty;

        }

        public ActionResultDTO Put(int id, BuildingEntity tObject)
        {
            _request.Method = Method.PUT;
            _request.AddJsonBody(tObject);
            _request.Resource = string.Format("api/{0}/Put/{1}", _resource, id);
            var response = new ApiRequest().Execute<ActionResultDTO>(_request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }

        public ActionResultDTO Post(BuildingEntity tObject)
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
    
    }
}
