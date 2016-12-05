using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class UserRightAPI : GenericAPI<UserRightEntity>
    {
        public UserRightAPI(string resource):base(resource)
        {
		
        }
   

        public ActionResultDTO Post(List<UserRightEntity> listOfRights)
        {
            _request.Method = Method.POST;
            _request.Resource = string.Format("api/{0}/Post/", _resource);
            _request.AddJsonBody(listOfRights);
            return new ApiRequest().Execute<ActionResultDTO>(_request);
        }

        
    }
}