using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class SiteAPI : BaseAPI
    {
        private readonly ApiRequest _apiRequest;

        public SiteAPI(string resource) : base(resource)
        {
            _apiRequest = new ApiRequest();
        }

        public ActionResultDTO Delete(int id)
        {
            Request.Method = Method.DELETE;
            Request.Resource = string.Format("api/{0}/Delete/{1}", Resource, id);
            var response = _apiRequest.Execute<ActionResultDTO>(Request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }

        public SiteEntity Get(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/Get/{1}", Resource, id);
            return _apiRequest.Execute<SiteEntity>(Request);
        }

        public List<SiteWithClusterGroup> Get(int limit, string searchstring)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/Get", Resource);
            Request.AddParameter("limit", limit);
            Request.AddParameter("searchstring", searchstring);
            var result = _apiRequest.Execute<List<SiteWithClusterGroup>>(Request);
            if (result == null)
                return new List<SiteWithClusterGroup>();
            else
                return result;
        }

        public string GetCount()
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetCount", Resource);
            var responseData = _apiRequest.Execute<ApiStringResponseDTO>(Request);
            return responseData != null ? responseData.Value : string.Empty;
        }

        public ActionResultDTO Post(SiteEntity tObject)
        {
            Request.Method = Method.POST;
            Request.AddJsonBody(tObject);
            Request.Resource = string.Format("api/{0}/Post/", Resource);
            var response = _apiRequest.Execute<ActionResultDTO>(Request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }

        public ActionResultDTO Put(int id, SiteEntity tObject)
        {
            Request.Method = Method.PUT;
            Request.AddJsonBody(tObject);
            Request.Resource = string.Format("api/{0}/Put/{1}", Resource, id);
            var response = _apiRequest.Execute<ActionResultDTO>(Request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }
    }
}