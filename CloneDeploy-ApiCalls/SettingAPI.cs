using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class SettingAPI : BaseAPI
    {
        private readonly ApiRequest _apiRequest;

        public SettingAPI(string resource) : base(resource)
        {
            _apiRequest = new ApiRequest();
        }

        public SettingAPI(string resource, CustomApiCallDTO cApiDto) : base(resource)
        {
            _apiRequest = new ApiRequest(cApiDto.Token, cApiDto.BaseUrl);
        }

        public bool CheckExpiredToken()
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/CheckExpiredToken/", Resource);
            var result = _apiRequest.ExecuteExpired<ApiBoolResponseDTO>(Request);
            if (result == null) return false;
            return result;
        }

        public SettingEntity GetSetting(string name)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetSetting/", Resource);
            Request.AddParameter("name", name);
            return _apiRequest.Execute<SettingEntity>(Request);
        }

        public bool SendEmailTest()
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/SendEmailTest/", Resource);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }

        public bool UpdatePxeSettings(List<SettingEntity> listSettings)
        {
            Request.Method = Method.POST;
            Request.Resource = string.Format("api/{0}/UpdatePxeSettings/", Resource);
            Request.AddJsonBody(listSettings);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }

        public bool UpdateSettings(List<SettingEntity> listSettings)
        {
            Request.Method = Method.POST;
            Request.Resource = string.Format("api/{0}/UpdateSettings/", Resource);
            Request.AddJsonBody(listSettings);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }
    }
}