using System.Collections.Generic;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class BaseAPI
    {
        protected readonly RestRequest _request;     
        protected readonly string _resource;
        protected readonly CustomApiCallDTO _cApiDto;

        public BaseAPI(string resource)
        {
            _request = new RestRequest();
            _resource = resource;
        }

        public BaseAPI(string resource, CustomApiCallDTO cApiDto)
        {
            _request = new RestRequest();
            _resource = resource;
            _cApiDto = cApiDto;
        }

        
    }
}


