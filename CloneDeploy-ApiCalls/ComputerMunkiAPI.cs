using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class ComputerMunkiAPI: GenericAPI<ComputerMunkiEntity>
    {
        public ComputerMunkiAPI(string resource):base(resource)
        {
		
        }

        public new List<ComputerMunkiEntity> Get(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/Get/{1}", _resource,id);
            return new ApiRequest().Execute<List<ComputerMunkiEntity>>(_request);
        }
        public List<ComputerMunkiEntity> GetTemplateComputers(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetTemplateComputers/{1}", _resource,id);
            return new ApiRequest().Execute<List<ComputerMunkiEntity>>(_request);
        }

        public string GetEffectiveManifest(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetEffectiveManifest/{1}", _resource, id);
            return new ApiRequest().Execute<ApiStringResponseDTO>(_request).Value;
        }

       

       
    
    }
}