using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class ComputerAPI : BaseAPI
    {
        public ComputerAPI(string resource):base(resource)
        {
		
        }
        public List<ComputerEntity> GetAll(int limit, string searchstring)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetAll", _resource);
            _request.AddParameter("limit", limit);
            _request.AddParameter("searchstring", searchstring);
            return new ApiRequest().Execute<List<ComputerEntity>>(_request);
        }

        public ComputerEntity Get(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/Get/{1}", _resource, id);
            return new ApiRequest().Execute<ComputerEntity>(_request);
        }

        public string GetCount()
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetCount", _resource);
            var responseData = new ApiRequest().Execute<ApiStringResponseDTO>(_request);
            return responseData != null ? responseData.Value : string.Empty;

        }

        public ActionResultDTO Put(int id, ComputerEntity tObject)
        {
            _request.Method = Method.PUT;
            _request.AddJsonBody(tObject);
            _request.Resource = string.Format("api/{0}/Put/{1}", _resource, id);
            var response = new ApiRequest().Execute<ActionResultDTO>(_request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }

        public ActionResultDTO Post(ComputerEntity tObject)
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

        public IEnumerable<ComputerEntity> GetAllByName(int limit = 0, string searchstring = "")
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetAllByName", _resource);
            _request.AddParameter("limit", limit);
            _request.AddParameter("searchstring", searchstring);
            return new ApiRequest().Execute<List<ComputerEntity>>(_request);
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

        public ApiBoolResponseDTO AddToSmartGroups(ComputerEntity computer)
        {
            _request.Method = Method.POST;
            _request.Resource = string.Format("api/{0}/AddToSmartGroups/", _resource);
            _request.AddJsonBody(computer);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request);
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

        public int Import(ApiStringResponseDTO csvContents)
        {
            _request.Method = Method.POST;
            _request.Resource = string.Format("api/{0}/Import/", _resource);
            _request.AddJsonBody(csvContents);
            return new ApiRequest().Execute<ApiIntResponseDTO>(_request).Value;
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
