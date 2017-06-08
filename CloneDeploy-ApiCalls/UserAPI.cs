using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    /// <summary>
    ///     Summary description for User
    /// </summary>
    public class UserAPI : BaseAPI
    {
        private readonly ApiRequest _apiRequest;

        public UserAPI(string resource) : base(resource)
        {
            _apiRequest = new ApiRequest();
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

        public CloneDeployUserEntity Get(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/Get/{1}", Resource, id);
            return _apiRequest.Execute<CloneDeployUserEntity>(Request);
        }


        public int GetAdminCount()
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetAdminCount/", Resource);
            var response = _apiRequest.Execute<ApiIntResponseDTO>(Request);
            return response != null ? response.Value : 0;
        }

        public List<UserWithUserGroup> GetAll(int limit, string searchstring)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetAll", Resource);
            Request.AddParameter("limit", limit);
            Request.AddParameter("searchstring", searchstring);
            return _apiRequest.Execute<List<UserWithUserGroup>>(Request);
        }

        public CloneDeployUserEntity GetByName(string username)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetByName/", Resource);
            Request.AddParameter("username", username);
            return _apiRequest.Execute<CloneDeployUserEntity>(Request);
        }

        public IEnumerable<AuditLogEntity> GetUserAuditLogs(int id, int limit)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetUserAuditLogs/{1}", Resource, id);
            Request.AddParameter("limit", limit);
            return _apiRequest.Execute<List<AuditLogEntity>>(Request);

        }

        public string GetCount()
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetCount", Resource);
            var responseData = _apiRequest.Execute<ApiStringResponseDTO>(Request);
            return responseData != null ? responseData.Value : string.Empty;
        }

        public ApiObjectResponseDTO GetForLogin(string username)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetForLogin/", Resource);
            Request.AddParameter("username", username);
            var response = _apiRequest.Execute<ApiObjectResponseDTO>(Request);

            if (response.Id == 0)
                response.Success = false;
            return response;
        }

        public IEnumerable<UserGroupManagementEntity> GetGroupManagements(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetGroupManagements/{1}", Resource, id);
            return _apiRequest.Execute<List<UserGroupManagementEntity>>(Request);
        }


        public IEnumerable<UserImageManagementEntity> GetImageManagements(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetImageManagements/{1}", Resource, id);
            return _apiRequest.Execute<List<UserImageManagementEntity>>(Request);
        }

        public IEnumerable<UserRightEntity> GetRights(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetRights/{1}", Resource, id);
            return _apiRequest.Execute<List<UserRightEntity>>(Request);
        }


        public bool IsAdmin(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/IsAdmin/", Resource);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }

        public ActionResultDTO Post(CloneDeployUserEntity tObject)
        {
            Request.Method = Method.POST;
            Request.AddJsonBody(tObject);
            Request.Resource = string.Format("api/{0}/Post/", Resource);
            var response = _apiRequest.Execute<ActionResultDTO>(Request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }

        public ActionResultDTO Put(int id, CloneDeployUserEntity tObject)
        {
            Request.Method = Method.PUT;
            Request.AddJsonBody(tObject);
            Request.Resource = string.Format("api/{0}/Put/{1}", Resource, id);
            var response = _apiRequest.Execute<ActionResultDTO>(Request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }
    }
}