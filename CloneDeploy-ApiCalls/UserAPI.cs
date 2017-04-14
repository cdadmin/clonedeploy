using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    /// <summary>
    /// Summary description for User
    /// </summary>
    public class UserAPI : BaseAPI
    {
        public UserAPI(string resource):base(resource)
        {
		
        }

        public List<CloneDeployUserEntity> GetAll(int limit, string searchstring)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetAll", _resource);
            _request.AddParameter("limit", limit);
            _request.AddParameter("searchstring", searchstring);
            return new ApiRequest().Execute<List<CloneDeployUserEntity>>(_request);
        }

        public CloneDeployUserEntity Get(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/Get/{1}", _resource, id);
            return new ApiRequest().Execute<CloneDeployUserEntity>(_request);
        }

        public string GetCount()
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetCount", _resource);
            var responseData = new ApiRequest().Execute<ApiStringResponseDTO>(_request);
            return responseData != null ? responseData.Value : string.Empty;

        }

        public ActionResultDTO Put(int id, CloneDeployUserEntity tObject)
        {
            _request.Method = Method.PUT;
            _request.AddJsonBody(tObject);
            _request.Resource = string.Format("api/{0}/Put/{1}", _resource, id);
            var response = new ApiRequest().Execute<ActionResultDTO>(_request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }

        public ActionResultDTO Post(CloneDeployUserEntity tObject)
        {
            _request.Method = Method.POST;
            _request.AddJsonBody(tObject);
            _request.Resource = string.Format("api/{0}/Post/", _resource);
            var response = new ApiRequest().Execute<ActionResultDTO>(_request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }

        public ActionResultDTO Delete(int id)
        {
            _request.Method = Method.DELETE;
            _request.Resource = string.Format("api/{0}/Delete/{1}", _resource, id);
            var response = new ApiRequest().Execute<ActionResultDTO>(_request);
            if (response.Id == 0)
                response.Success = false;
            return response;

        }
        public CloneDeployUserEntity GetByName(string username)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetByName/", _resource);
            _request.AddParameter("username", username);
            return new ApiRequest().Execute<CloneDeployUserEntity>(_request);
        }

        public ApiObjectResponseDTO GetForLogin(string username)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetForLogin/", _resource);
            _request.AddParameter("username", username);
            var response = new ApiRequest().Execute<ApiObjectResponseDTO>(_request);

            if (response.Id == 0)
                response.Success = false;
            return response;

        }



        public int GetAdminCount()
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetAdminCount/", _resource);
            return new ApiRequest().Execute<ApiIntResponseDTO>(_request).Value;
        }




        public bool IsAdmin(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/IsAdmin/", _resource);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;
        }

        public IEnumerable<UserRightEntity> GetRights(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetRights/{1}", _resource,id);
            return new ApiRequest().Execute<List<UserRightEntity>>(_request);
        }


        public bool DeleteRights(int id)
        {
            _request.Method = Method.DELETE;
            _request.Resource = string.Format("api/{0}/DeleteRights/{1}", _resource,id);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;

        }


        public IEnumerable<UserImageManagementEntity> GetImageManagements(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetImageManagements/{1}", _resource,id);
            return new ApiRequest().Execute<List<UserImageManagementEntity>>(_request);
        }


        public bool DeleteImageManagements(int id)
        {
            _request.Method = Method.DELETE;
            _request.Resource = string.Format("api/{0}/DeleteImageManagements/{1}", _resource,id);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;

        }

        public IEnumerable<UserGroupManagementEntity> GetGroupManagements(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetGroupManagements/{1}", _resource,id);
            return new ApiRequest().Execute<List<UserGroupManagementEntity>>(_request);
        }


        public bool DeleteGroupManagements(int id)
        {
            _request.Method = Method.DELETE;
            _request.Resource = string.Format("api/{0}/DeleteGroupManagements/", _resource,id);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;
        }
    }
}