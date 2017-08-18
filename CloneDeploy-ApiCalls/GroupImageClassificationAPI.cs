using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class GroupImageClassificationAPI : BaseAPI
    {
        private readonly ApiRequest _apiRequest;

        public GroupImageClassificationAPI(string resource) : base(resource)
        {
            _apiRequest = new ApiRequest();
        }

        public List<ImageWithDate> FilterClassifications(FilterGroupClassificationDTO filterGroupClassificationDto)
        {
            Request.Method = Method.POST;
            Request.Resource = string.Format("api/{0}/FilterClassifications/", Resource);
            Request.AddJsonBody(filterGroupClassificationDto);
            return _apiRequest.Execute<List<ImageWithDate>>(Request);
        }

        public ActionResultDTO Post(List<GroupImageClassificationEntity> listOfClassifications)
        {
            Request.Method = Method.POST;
            Request.Resource = string.Format("api/{0}/Post/", Resource);
            Request.AddJsonBody(listOfClassifications);
            return _apiRequest.Execute<ActionResultDTO>(Request);
        }
    }
}