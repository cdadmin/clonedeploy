using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class GroupMunkiAPI: GenericAPI<GroupMunkiEntity>
    {
        public GroupMunkiAPI(string resource):base(resource)
        {
		
        }

        public string GetEffectiveManifest(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetEffectiveManifest/{1}", _resource, id);
            return new ApiRequest().Execute<ApiStringResponseDTO>(_request).Value;
        }

       

       
    
    }
}