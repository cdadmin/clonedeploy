using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class ComputerAPI :GenericAPI<ComputerEntity>
    {
        public ComputerAPI(string resource):base(resource)
        {
		
        }


        public IEnumerable<ComputerEntity> GetComputersWithoutGroup(int limit, string searchstring)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetComputersWithoutGroup", _resource);
            _request.AddParameter("limit", limit);
            _request.AddParameter("searchstring", searchstring);
            return new ApiRequest().Execute<List<ComputerEntity>>(_request);
        }


        public IEnumerable<GroupMembershipEntity> GetGroupMemberships(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetGroupMemberships/{1}", _resource,id);
            return new ApiRequest().Execute<List<GroupMembershipEntity>>(_request);
        }


        public ComputerEntity GetByMac(string mac)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetByMac/", _resource);
            _request.AddParameter("mac", mac);
            return new ApiRequest().Execute<ComputerEntity>(_request);
        }

        public ApiBoolResponseDTO Export(string path)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/Export/", _resource);
            _request.AddParameter("path", path);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request);
        }

     



        public bool IsComputerActive(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/IsComputerActive/{1}", _resource, id);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;
        }


        public ActiveImagingTaskEntity GetActiveTask(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetActiveTask/{1}", _resource, id);
            return new ApiRequest().Execute<ActiveImagingTaskEntity>(_request);
        }


        public ComputerBootMenuEntity GetBootMenu(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetBootMenu/{1}", _resource, id);
            return new ApiRequest().Execute<ComputerBootMenuEntity>(_request);
        }

        public ActionResultDTO DeleteBootMenus(int id)
        {
            _request.Method = Method.DELETE;
            _request.Resource = string.Format("api/{0}/DeleteBootMenus/{1}", _resource, id);
            return new ApiRequest().Execute<ActionResultDTO>(_request);
        }


        public bool CreateCustomBootFiles(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/CreateCustomBootFiles/{1}", _resource,id);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;
        }


        public string GetProxyPath(int id, bool isActiveOrCustom, string proxyType)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetProxyPath/{1}", _resource, id);
            _request.AddParameter("isActiveOrCustom", isActiveOrCustom);
            _request.AddParameter("proxyType", proxyType);
            return new ApiRequest().Execute<ApiStringResponseDTO>(_request).Value;
        }


        public string GetNonProxyPath(int id, bool isActiveOrCustom)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetNonProxyPath/{1}", _resource, id);
            _request.AddParameter("isActiveOrCustom", isActiveOrCustom);
            return new ApiRequest().Execute<ApiStringResponseDTO>(_request).Value;
        }

        public IEnumerable<ComputerLogEntity> GetComputerLogs(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetComputerLogs/{1}", _resource, id);
            return new ApiRequest().Execute<List<ComputerLogEntity>>(_request);
        }


        public ActionResultDTO DeleteAllComputerLogs(int id)
        {
            _request.Method = Method.DELETE;
            _request.Resource = string.Format("api/{0}/DeleteAllComputerLogs/{1}", _resource, id);
            return new ApiRequest().Execute<ActionResultDTO>(_request);
        }


        public IEnumerable<ComputerMunkiEntity> GetMunkiTemplates(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetMunkiTemplates/{1}", _resource, id);
            return new ApiRequest().Execute<List<ComputerMunkiEntity>>(_request);
        }


        public string GetEffectiveManifest(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetEffectiveManifest/{1}", _resource, id);
            return new ApiRequest().Execute<ApiStringResponseDTO>(_request).Value;
        }


        public ActionResultDTO DeleteMunkiTemplates(int id)
        {
            _request.Method = Method.DELETE;
            _request.Resource = string.Format("api/{0}/DeleteMunkiTemplates/{1}", _resource, id);
            return new ApiRequest().Execute<ActionResultDTO>(_request);
        }


        public ComputerProxyReservationEntity GetProxyReservation(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetProxyReservation/{1}", _resource, id);
            return new ApiRequest().Execute<ComputerProxyReservationEntity>(_request);
        }


        public bool ToggleProxyReservation(int id, bool status)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/ToggleProxyReservation/{1}", _resource, id);
            _request.AddParameter("status", status);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;
        }


        public bool ToggleBootMenu(int id, bool status)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/ToggleBootMenu/{1}", _resource, id);
            _request.AddParameter("status", status);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;

        }


        public string StartUpload(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/StartUpload/{1}", _resource, id);
            return new ApiRequest().Execute<ApiStringResponseDTO>(_request).Value;
        }


        public string StartDeploy(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/StartDeploy/{1}", _resource, id);
            return new ApiRequest().Execute<ApiStringResponseDTO>(_request).Value;
        }

        public string StartPermanentDeploy(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/StartPermanentDeploy/{1}", _resource, id);
            return new ApiRequest().Execute<ApiStringResponseDTO>(_request).Value;
        }
    }
}
