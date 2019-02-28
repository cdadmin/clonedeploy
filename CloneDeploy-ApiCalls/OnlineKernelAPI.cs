﻿using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class OnlineKernelAPI : BaseAPI
    {
        private readonly ApiRequest _apiRequest;

        public OnlineKernelAPI(string resource) : base(resource)
        {
            _apiRequest = new ApiRequest();
        }

        public bool Download(OnlineKernel tObject)
        {
            Request.Method = Method.POST;
            Request.AddJsonBody(tObject);
            Request.Resource = string.Format("api/{0}/Download/", Resource);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }

        public List<OnlineKernel> Get()
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/Get", Resource);

            var result = _apiRequest.Execute<List<OnlineKernel>>(Request);
            if (result == null)
                return new List<OnlineKernel>();
            else
                return result;
        }
    }
}