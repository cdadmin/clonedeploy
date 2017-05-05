using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class UserImageManagementAPI : BaseAPI
    {
        private readonly ApiRequest _apiRequest;

        public UserImageManagementAPI(string resource) : base(resource)
        {
            _apiRequest = new ApiRequest();
        }


        public ActionResultDTO Post(List<UserImageManagementEntity> listOfImages)
        {
            Request.Method = Method.POST;
            Request.Resource = string.Format("api/{0}/Post/", Resource);
            Request.AddJsonBody(listOfImages);
            return _apiRequest.Execute<ActionResultDTO>(Request);
        }
    }
}