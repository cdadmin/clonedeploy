using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class UserGroupManagementAPI : GenericAPI<UserGroupManagementEntity>
    {
        public UserGroupManagementAPI(string resource):base(resource)
        {
		
        }
  
        public ActionResultDTO Post(List<UserGroupManagementEntity> listOfGroups)
        {
            _request.Method = Method.POST;
            _request.Resource = string.Format("api/{0}/Post/", _resource);
            _request.AddJsonBody(listOfGroups);
            return new ApiRequest().Execute<ActionResultDTO>(_request);
        }

        
    }
}