using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class ComputerLogAPI : BaseAPI
    {
        public ComputerLogAPI(string resource):base(resource)
        {
		
        }

        public ComputerLogEntity Get(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/Get/{1}", _resource, id);
            return new ApiRequest().Execute<ComputerLogEntity>(_request);
        }

        public ActionResultDTO Post(ComputerLogEntity tObject)
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

        public IEnumerable<ComputerLogEntity> GetOnDemandLogs(int limit = 0)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetOnDemandLogs/", _resource);
            _request.AddParameter("limit", limit);
            return new ApiRequest().Execute<List<ComputerLogEntity>>(_request);
        }

        


      
    }
}