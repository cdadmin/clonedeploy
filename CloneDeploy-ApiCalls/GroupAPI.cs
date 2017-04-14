using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    /// <summary>
    /// Summary description for User
    /// </summary>
    public class GroupAPI : BaseAPI
    {
        public GroupAPI(string resource):base(resource)
        {
		
        }

        public List<GroupEntity> GetAll(int limit, string searchstring)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetAll", _resource);
            _request.AddParameter("limit", limit);
            _request.AddParameter("searchstring", searchstring);
            return new ApiRequest().Execute<List<GroupEntity>>(_request);
        }

        public GroupEntity Get(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/Get/{1}", _resource, id);
            return new ApiRequest().Execute<GroupEntity>(_request);
        }

        public string GetCount()
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetCount", _resource);
            var responseData = new ApiRequest().Execute<ApiStringResponseDTO>(_request);
            return responseData != null ? responseData.Value : string.Empty;

        }

        public ActionResultDTO Put(int id, GroupEntity tObject)
        {
            _request.Method = Method.PUT;
            _request.AddJsonBody(tObject);
            _request.Resource = string.Format("api/{0}/Put/{1}", _resource, id);
            var response = new ApiRequest().Execute<ActionResultDTO>(_request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }

        public ActionResultDTO Post(GroupEntity tObject)
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

        public string GetMemberCount(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetMemberCount/{1}", _resource,id);
            return new ApiRequest().Execute<ApiStringResponseDTO>(_request).Value;
        }



        public bool RemoveGroupMember(int id, int computerId)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/RemoveGroupMember/{1}", _resource, id);
            _request.AddParameter("computerId", computerId);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;
        }


        public bool RemoveMunkiTemplates(int id)
        {
            _request.Method = Method.DELETE;
            _request.Resource = string.Format("api/{0}/RemoveMunkiTemplates/{1}", _resource, id);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;
        }

        public bool ReCalcSmart()
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/ReCalcSmart", _resource);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;
        }

        public IEnumerable<GroupMunkiEntity> GetMunkiTemplates(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetMunkiTemplates/{1}", _resource, id);
            return new ApiRequest().Execute<List<GroupMunkiEntity>>(_request);
        }


        public GroupPropertyEntity GetGroupProperties(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetGroupProperties/{1}", _resource, id);
            return new ApiRequest().Execute<GroupPropertyEntity>(_request);
        }

        public ApiBoolResponseDTO Export(string path)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/Export/", _resource);
            _request.AddParameter("path", path);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request);
        }

        public ActionResultDTO UpdateSmartMembership(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/UpdateSmartMembership/{1}", _resource, id);
            return new ApiRequest().Execute<ActionResultDTO>(_request);
        }


        public int StartGroupUnicast(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/StartGroupUnicast/{1}", _resource, id);
            return new ApiRequest().Execute<ApiIntResponseDTO>(_request).Value;

        }

        public string StartMulticast(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/StartMulticast/{1}", _resource, id);
            return new ApiRequest().Execute<ApiStringResponseDTO>(_request).Value;

        }


        public IEnumerable<ComputerEntity> GetGroupMembers(int id, string searchstring = "")
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetGroupMembers/{1}", _resource, id);
            _request.AddParameter("searchstring", searchstring);
            return new ApiRequest().Execute<List<ComputerEntity>>(_request);

        }

        public int Import(ApiStringResponseDTO csvContents)
        {
            _request.Method = Method.POST;
            _request.Resource = string.Format("api/{0}/Import/", _resource);
            _request.AddJsonBody(csvContents);
            return new ApiRequest().Execute<ApiIntResponseDTO>(_request).Value;
        }

        public GroupBootMenuEntity GetCustomBootMenu(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetCustomBootMenu/{1}", _resource, id);
            return new ApiRequest().Execute<GroupBootMenuEntity>(_request);
        }

        public string GetEffectiveManifest(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetEffectiveManifest/{1}", _resource, id);
            return new ApiRequest().Execute<ApiStringResponseDTO>(_request).Value;
        }
    }
}