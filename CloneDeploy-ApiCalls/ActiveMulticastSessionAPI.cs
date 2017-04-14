using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class ActiveMulticastSessionAPI : BaseAPI
    {
        public ActiveMulticastSessionAPI(string resource):base(resource)
        {
		
        }

        public List<ActiveMulticastSessionEntity> GetAll(int limit, string searchstring)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetAll", _resource);
            _request.AddParameter("limit", limit);
            _request.AddParameter("searchstring", searchstring);
            return new ApiRequest().Execute<List<ActiveMulticastSessionEntity>>(_request);
        }

        public string GetCount()
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetCount", _resource);
            var responseData = new ApiRequest().Execute<ApiStringResponseDTO>(_request);
            return responseData != null ? responseData.Value : string.Empty;

        }
        public IEnumerable<ActiveImagingTaskEntity> GetMemberStatus(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetUnicasts/{1}", _resource,id);
            return new ApiRequest().Execute<List<ActiveImagingTaskEntity>>(_request);
        }

        public IEnumerable<ComputerEntity> GetComputers(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetComputers/{1}", _resource,id);
            return new ApiRequest().Execute<List<ComputerEntity>>(_request);
        }


        public IEnumerable<ActiveImagingTaskEntity> GetProgress(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetUnicasts/{1}", _resource,id);
            return new ApiRequest().Execute<List<ActiveImagingTaskEntity>>(_request);
           
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