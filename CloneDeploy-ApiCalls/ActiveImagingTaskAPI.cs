using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class ActiveImagingTaskAPI : BaseAPI
    {
        private readonly ApiRequest _apiRequest;

        public ActiveImagingTaskAPI(string resource) : base(resource)
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

        public ActionResultDTO DeleteOnDemand(int id)
        {
            Request.Method = Method.DELETE;
            Request.Resource = string.Format("api/{0}/DeleteOnDemand/{1}", Resource, id);
            var response = _apiRequest.Execute<ActionResultDTO>(Request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }

        public string GetActiveNotOwned()
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetActiveNotOwned/", Resource);
            var response = _apiRequest.Execute<ApiStringResponseDTO>(Request);
            return response != null ? response.Value : string.Empty;
        }

        public IEnumerable<TaskWithComputer> GetActiveTasks()
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetActiveTasks/", Resource);
            var result = _apiRequest.Execute<List<TaskWithComputer>>(Request);
            if (result == null)
                return new List<TaskWithComputer>();
            else
                return result;
        }

        public string GetActiveUnicastCount(string taskType = "")
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetActiveUnicastCount/", Resource);
            Request.AddParameter("tasktype", taskType);
            var response = _apiRequest.Execute<ApiStringResponseDTO>(Request);
            return response != null ? response.Value : string.Empty;
        }

        public string GetAllActiveCount()
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetAllActiveCount/", Resource);
            var response = _apiRequest.Execute<ApiStringResponseDTO>(Request);
            return response != null ? response.Value : string.Empty;
        }

        public IEnumerable<ActiveImagingTaskEntity> GetAllOnDemandUnregistered()
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetAllOnDemandUnregistered/", Resource);
            var result = _apiRequest.Execute<List<ActiveImagingTaskEntity>>(Request);
            if (result == null)
                return new List<ActiveImagingTaskEntity>();
            else
                return result;
        }

        public IEnumerable<TaskWithComputer> GetPermanentUnicasts()
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetPermanentUnicasts/", Resource);
            var result = _apiRequest.Execute<List<TaskWithComputer>>(Request);
            if (result == null)
                return new List<TaskWithComputer>();
            else
                return result;
        }

        public IEnumerable<TaskWithComputer> GetUnicasts()
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetUnicasts/", Resource);
            var result = _apiRequest.Execute<List<TaskWithComputer>>(Request);
            if (result == null)
                return new List<TaskWithComputer>();
            else
                return result;
        }

        public int OnDemandCount()
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/OnDemandCount/", Resource);
            var response = _apiRequest.Execute<ApiIntResponseDTO>(Request);
            if (response != null) return response.Value;
            return 0;
        }

        public void RecreatePermanentTasks()
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/RecreatePermanentTasks/", Resource);
            _apiRequest.Execute<ApiIntResponseDTO>(Request);
        }
    }
}