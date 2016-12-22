using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
   
    public class UserGroupAPI : GenericAPI<CloneDeployUserGroupEntity>
    {
        public UserGroupAPI(string resource):base(resource)
        {
		
        }

      
        public string GetMemberCount(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetMemberCount/{1}", _resource,id);
            return new ApiRequest().Execute<ApiStringResponseDTO>(_request).Value;
        }


        public IEnumerable<CloneDeployUserEntity> GetGroupMembers(int id, string searchstring = "")
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetGroupMembers/{1}", _resource,id);
            _request.AddParameter("searchstring", searchstring);
            return new ApiRequest().Execute<List<CloneDeployUserEntity>>(_request);

        }


        public bool UpdateMemberAcls(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/UpdateMemberAcls/{1}", _resource, id);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;

        }


        public bool UpdateMemberGroups(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/UpdateMemberGroups/{1}", _resource, id);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;

        }


        public bool UpdateMemberImages(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/UpdateMemberImages/{1}", _resource, id);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;

        }


        public bool AddNewMember(int id, int userId)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/AddNewMember/{1}", _resource,id);
            _request.AddParameter("userId", userId);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;
        }


        public IEnumerable<UserGroupRightEntity> GetRights(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetRights/{1}", _resource, id);
            return new ApiRequest().Execute<List<UserGroupRightEntity>>(_request);
        }


        public bool DeleteRights(int id)
        {
            _request.Method = Method.DELETE;
            _request.Resource = string.Format("api/{0}/DeleteRights/{1}", _resource, id);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;
        }


        public IEnumerable<UserGroupImageManagementEntity> GetImageManagements(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetImageManagements/{1}", _resource, id);
            return new ApiRequest().Execute<List<UserGroupImageManagementEntity>>(_request);
        }


        public bool DeleteImageManagements(int id)
        {
            _request.Method = Method.DELETE;
            _request.Resource = string.Format("api/{0}/DeleteImageManagements/{1}", _resource, id);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;

        }


        public IEnumerable<UserGroupGroupManagementEntity> GetGroupManagements(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetGroupManagements/{1}", _resource, id);
            return new ApiRequest().Execute<List<UserGroupGroupManagementEntity>>(_request);
        }


        public bool DeleteGroupManagements(int id)
        {
            _request.Method = Method.DELETE;
            _request.Resource = string.Format("api/{0}/DeleteGroupManagements/{1}", _resource, id);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;

        }
     
    }
}
