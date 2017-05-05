using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class UserGroupImageManagementAPI : BaseAPI
    {
        private readonly ApiRequest _apiRequest;


        public UserGroupImageManagementAPI(string resource) : base(resource)
        {
            _apiRequest = new ApiRequest();
        }


        public ActionResultDTO Post(List<UserGroupImageManagementEntity> listOfImages)
        {
            Request.Method = Method.POST;
            Request.Resource = string.Format("api/{0}/Post/", Resource);
            Request.AddJsonBody(listOfImages);
            return _apiRequest.Execute<ActionResultDTO>(Request);
        }
    }
}