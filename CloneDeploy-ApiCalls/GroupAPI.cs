using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using CloneDeploy_App.DTOs;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    /// <summary>
    /// Summary description for User
    /// </summary>
    public class GroupAPI: GenericAPI<GroupEntity>
    {
        public GroupAPI(string resource):base(resource)
        {
		
        }

        public string GetMemberCount(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetMemberCount/{1}", _resource,id);
            return new ApiRequest().Execute<ApiStringResponseDTO>(_request).Value;
        }



        public bool RemoveGroupMember(int id, int computerId)
        {
            _request.Method = Method.DELETE;
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


        public GroupBootMenuEntity GetCustomBootMenu(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetCustomBootMenu/{1}", _resource, id);
            return new ApiRequest().Execute<GroupBootMenuEntity>(_request);
        }
    }
}