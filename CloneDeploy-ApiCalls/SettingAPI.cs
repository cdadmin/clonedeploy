using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class SettingAPI : GenericAPI<SettingEntity>
    {
        public SettingAPI(string resource):base(resource)
        {
		
        }

        public SettingAPI(string resource,CustomApiCallDTO cApiDto):base(resource,cApiDto)
        {

        }
    
        public SettingEntity GetSetting(string name)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetSetting/", _resource);
            _request.AddParameter("name", name);
            return _cApiDto != null ? new ApiRequest(_cApiDto.Token, _cApiDto.BaseUrl).Execute<SettingEntity>(_request) : new ApiRequest().Execute<SettingEntity>(_request);
        }


        public bool UpdateSettings(List<SettingEntity> listSettings)
        {
            _request.Method = Method.POST;
            _request.Resource = string.Format("api/{0}/UpdateSettings/", _resource);
            _request.AddJsonBody(listSettings);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;
            
        }

        public bool SendEmailTest()
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/SendEmailTest/", _resource);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;
        }

       

       
    }
}