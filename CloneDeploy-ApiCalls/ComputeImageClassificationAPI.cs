﻿using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class ComputerImageClassificationAPI : BaseAPI
    {
        private readonly ApiRequest _apiRequest;

        public ComputerImageClassificationAPI(string resource) : base(resource)
        {
            _apiRequest = new ApiRequest();
        }

        public List<ImageWithDate> FilterClassifications(FilterComputerClassificationDTO filterComputerClassificationDto)
        {
            Request.Method = Method.POST;
            Request.Resource = string.Format("api/{0}/FilterClassifications/", Resource);
            Request.AddJsonBody(filterComputerClassificationDto);
            return _apiRequest.Execute<List<ImageWithDate>>(Request);
        }

        public ActionResultDTO Post(List<ComputerImageClassificationEntity> listOfClassifications)
        {
            Request.Method = Method.POST;
            Request.Resource = string.Format("api/{0}/Post/", Resource);
            Request.AddJsonBody(listOfClassifications);
            return _apiRequest.Execute<ActionResultDTO>(Request);
        }
    }
}