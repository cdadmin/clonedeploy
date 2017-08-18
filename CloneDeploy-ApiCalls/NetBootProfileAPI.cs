using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class NetBootProfileAPI : BaseAPI
    {
        private readonly ApiRequest _apiRequest;

        public NetBootProfileAPI(string resource) : base(resource)
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

        public ApiBoolResponseDTO DeleteProfileNbiEntries(int profileId)
        {
            Request.Method = Method.DELETE;
            Request.Resource = string.Format("api/{0}/DeleteProfileNbiEntries/{1}", Resource, profileId);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response;
        }

        public NetBootProfileEntity Get(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/Get/{1}", Resource, id);
            return _apiRequest.Execute<NetBootProfileEntity>(Request);
        }

        public List<NetBootProfileEntity> Get(string searchstring)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/Get", Resource);
            Request.AddParameter("searchstring", searchstring);
            return _apiRequest.Execute<List<NetBootProfileEntity>>(Request);
        }

        public string GetCount()
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetCount", Resource);
            var responseData = _apiRequest.Execute<ApiStringResponseDTO>(Request);
            return responseData != null ? responseData.Value : string.Empty;
        }

        public List<NbiEntryEntity> GetProfileNbiEntries(int profileId)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetProfileNbiEntries/{1}", Resource, profileId);
            return _apiRequest.Execute<List<NbiEntryEntity>>(Request);
        }

        public ActionResultDTO Post(NetBootProfileEntity tObject)
        {
            Request.Method = Method.POST;
            Request.AddJsonBody(tObject);
            Request.Resource = string.Format("api/{0}/Post/", Resource);
            var response = _apiRequest.Execute<ActionResultDTO>(Request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }

        public ActionResultDTO Put(int id, NetBootProfileEntity tObject)
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