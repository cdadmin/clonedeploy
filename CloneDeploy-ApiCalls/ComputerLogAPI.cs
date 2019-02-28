using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class ComputerLogAPI : BaseAPI
    {
        private readonly ApiRequest _apiRequest;

        public ComputerLogAPI(string resource) : base(resource)
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

        public ComputerLogEntity Get(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/Get/{1}", Resource, id);
            return _apiRequest.Execute<ComputerLogEntity>(Request);
        }

        public IEnumerable<ComputerLogEntity> GetOnDemandLogs(int limit = 0)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetOnDemandLogs/", Resource);
            Request.AddParameter("limit", limit);
            var result = _apiRequest.Execute<List<ComputerLogEntity>>(Request);
            if (result == null)
                return new List<ComputerLogEntity>();
            else
                return result;
        }

        public ActionResultDTO Post(ComputerLogEntity tObject)
        {
            Request.Method = Method.POST;
            Request.AddJsonBody(tObject);
            Request.Resource = string.Format("api/{0}/Post/", Resource);
            var response = _apiRequest.Execute<ActionResultDTO>(Request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }
    }
}