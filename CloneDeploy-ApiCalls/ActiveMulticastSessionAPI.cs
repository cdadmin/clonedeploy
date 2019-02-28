﻿using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class ActiveMulticastSessionAPI : BaseAPI
    {
        private readonly ApiRequest _apiRequest;

        public ActiveMulticastSessionAPI(string resource) : base(resource)
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

        public List<ActiveMulticastSessionEntity> Get(int limit, string searchstring)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/Get", Resource);
            Request.AddParameter("limit", limit);
            Request.AddParameter("searchstring", searchstring);
            var result = _apiRequest.Execute<List<ActiveMulticastSessionEntity>>(Request);
            if (result == null)
                return new List<ActiveMulticastSessionEntity>();
            else
                return result;
        }

        public IEnumerable<ComputerEntity> GetComputers(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetComputers/{1}", Resource, id);
            var result = _apiRequest.Execute<List<ComputerEntity>>(Request);
            if (result == null)
                return new List<ComputerEntity>();
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

        public IEnumerable<TaskWithComputer> GetMemberStatus(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetMemberStatus/{1}", Resource, id);
            var result = _apiRequest.Execute<List<TaskWithComputer>>(Request);
            if (result == null)
                return new List<TaskWithComputer>();
            else
                return result;
        }

        public IEnumerable<ActiveImagingTaskEntity> GetProgress(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetProgress/{1}", Resource, id);
            var result = _apiRequest.Execute<List<ActiveImagingTaskEntity>>(Request);
            if (result == null)
                return new List<ActiveImagingTaskEntity>();
            else
                return result;
        }
    }
}