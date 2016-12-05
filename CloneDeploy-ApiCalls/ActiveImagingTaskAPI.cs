using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class ActiveImagingTaskAPI:GenericAPI<ActiveImagingTaskEntity>
    {
        public ActiveImagingTaskAPI(string resource):base(resource)
        {
		
        }

        public IEnumerable<ActiveImagingTaskEntity> GetUnicasts(string taskType)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetUnicasts/", _resource);
            _request.AddParameter("tasktype", taskType);
            return new ApiRequest().Execute<List<ActiveImagingTaskEntity>>(_request);

        }

        public IEnumerable<ActiveImagingTaskEntity> GetActiveTasks()
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetActiveTasks/", _resource);
            return new ApiRequest().Execute<List<ActiveImagingTaskEntity>>(_request);
        }

        public string GetActiveUnicastCount(string taskType)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetActiveUnicastCount/", _resource);
            _request.AddParameter("tasktype", taskType);
            return new ApiRequest().Execute<ApiStringResponseDTO>(_request).Value;
        }


        public string GetAllActiveCount()
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetAllActiveCount/", _resource);
            return new ApiRequest().Execute<ApiStringResponseDTO>(_request).Value;

        }

     
    }
}