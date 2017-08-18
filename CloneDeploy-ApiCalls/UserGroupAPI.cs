using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class UserGroupAPI : BaseAPI
    {
        private readonly ApiRequest _apiRequest;

        public UserGroupAPI(string resource) : base(resource)
        {
            _apiRequest = new ApiRequest();
        }

        public bool AddNewMember(int id, int userId)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/AddNewMember/{1}", Resource, id);
            Request.AddParameter("userId", userId);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }

        public ActionResultDTO Delete(int id)
        {
            Request.Method = Method.DELETE;
            Request.Resource = string.Format("api/{0}/Delete/{1}", Resource, id);
            var response = _apiRequest.Execute<ActionResultDTO>(Request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }

        public bool DeleteGroupManagements(int id)
        {
            Request.Method = Method.DELETE;
            Request.Resource = string.Format("api/{0}/DeleteGroupManagements/{1}", Resource, id);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }

        public bool DeleteImageManagements(int id)
        {
            Request.Method = Method.DELETE;
            Request.Resource = string.Format("api/{0}/DeleteImageManagements/{1}", Resource, id);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }

        public bool DeleteRights(int id)
        {
            Request.Method = Method.DELETE;
            Request.Resource = string.Format("api/{0}/DeleteRights/{1}", Resource, id);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }

        public CloneDeployUserGroupEntity Get(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/Get/{1}", Resource, id);
            return _apiRequest.Execute<CloneDeployUserGroupEntity>(Request);
        }

        public List<CloneDeployUserGroupEntity> Get(int limit, string searchstring)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/Get", Resource);
            Request.AddParameter("limit", limit);
            Request.AddParameter("searchstring", searchstring);
            return _apiRequest.Execute<List<CloneDeployUserGroupEntity>>(Request);
        }

        public string GetCount()
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetCount", Resource);
            var responseData = _apiRequest.Execute<ApiStringResponseDTO>(Request);
            return responseData != null ? responseData.Value : string.Empty;
        }

        public IEnumerable<UserGroupGroupManagementEntity> GetGroupManagements(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetGroupManagements/{1}", Resource, id);
            return _apiRequest.Execute<List<UserGroupGroupManagementEntity>>(Request);
        }

        public IEnumerable<CloneDeployUserEntity> GetGroupMembers(int id, string searchstring = "")
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetGroupMembers/{1}", Resource, id);
            Request.AddParameter("searchstring", searchstring);
            return _apiRequest.Execute<List<CloneDeployUserEntity>>(Request);
        }

        public IEnumerable<UserGroupImageManagementEntity> GetImageManagements(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetImageManagements/{1}", Resource, id);
            return _apiRequest.Execute<List<UserGroupImageManagementEntity>>(Request);
        }

        public string GetMemberCount(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetMemberCount/{1}", Resource, id);
            var response = _apiRequest.Execute<ApiStringResponseDTO>(Request);
            return response != null ? response.Value : string.Empty;
        }

        public IEnumerable<UserGroupRightEntity> GetRights(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetRights/{1}", Resource, id);
            return _apiRequest.Execute<List<UserGroupRightEntity>>(Request);
        }

        public ActionResultDTO Post(CloneDeployUserGroupEntity tObject)
        {
            Request.Method = Method.POST;
            Request.AddJsonBody(tObject);
            Request.Resource = string.Format("api/{0}/Post/", Resource);
            var response = _apiRequest.Execute<ActionResultDTO>(Request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }

        public ActionResultDTO Put(int id, CloneDeployUserGroupEntity tObject)
        {
            Request.Method = Method.PUT;
            Request.AddJsonBody(tObject);
            Request.Resource = string.Format("api/{0}/Put/{1}", Resource, id);
            var response = _apiRequest.Execute<ActionResultDTO>(Request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }

        public bool ToggleGroupManagement(int id, int value)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/ToggleGroupManagement/{1}", Resource, id);
            Request.AddParameter("value", value);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }

        public bool ToggleImageManagement(int id, int value)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/ToggleImageManagement/{1}", Resource, id);
            Request.AddParameter("value", value);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }

        public bool UpdateMemberAcls(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/UpdateMemberAcls/{1}", Resource, id);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }

        public bool UpdateMemberGroups(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/UpdateMemberGroups/{1}", Resource, id);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }

        public bool UpdateMemberImages(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/UpdateMemberImages/{1}", Resource, id);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }
    }
}