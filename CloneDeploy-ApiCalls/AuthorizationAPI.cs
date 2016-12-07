using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloneDeploy_App.DTOs;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class AuthorizationAPI
    {
        private readonly RestRequest _request;     
        private readonly string _resource;

        public AuthorizationAPI(string resource)
        {
            _request = new RestRequest();
            _resource = resource;
        }


        public bool IsAuthorized(string requiredRight)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/IsAuthorized/", _resource);
            _request.AddParameter("requiredRight", requiredRight);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;
        }


        public bool ComputerManagement(string requiredRight, int computerId)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/IsAuthorized/", _resource);
            _request.AddParameter("requiredRight", requiredRight);
            _request.AddParameter("computerId", computerId);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;
        }

        public bool GroupManagement(string requiredRight, int groupId)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/IsAuthorized/", _resource);
            _request.AddParameter("requiredRight", requiredRight);
            _request.AddParameter("groupId", groupId);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;
        }


        public bool ImageManagement(string requiredRight, int imageId)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/IsAuthorized/", _resource);
            _request.AddParameter("requiredRight", requiredRight);
            _request.AddParameter("imageId", imageId);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;
        }

      
    }
}
