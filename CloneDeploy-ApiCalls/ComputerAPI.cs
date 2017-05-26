using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class ComputerAPI : BaseAPI
    {
        private readonly ApiRequest _apiRequest;

        public ComputerAPI(string resource) : base(resource)
        {
            _apiRequest = new ApiRequest();
        }

        public ApiBoolResponseDTO AddToSmartGroups(ComputerEntity computer)
        {
            Request.Method = Method.POST;
            Request.Resource = string.Format("api/{0}/AddToSmartGroups/", Resource);
            Request.AddJsonBody(computer);
            return _apiRequest.Execute<ApiBoolResponseDTO>(Request);
        }


        public bool CreateCustomBootFiles(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/CreateCustomBootFiles/{1}", Resource, id);
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


        public ActionResultDTO DeleteAllComputerLogs(int id)
        {
            Request.Method = Method.DELETE;
            Request.Resource = string.Format("api/{0}/DeleteAllComputerLogs/{1}", Resource, id);
            return _apiRequest.Execute<ActionResultDTO>(Request);
        }

        public ActionResultDTO DeleteBootMenus(int id)
        {
            Request.Method = Method.DELETE;
            Request.Resource = string.Format("api/{0}/DeleteBootMenus/{1}", Resource, id);
            return _apiRequest.Execute<ActionResultDTO>(Request);
        }


        public ActionResultDTO DeleteMunkiTemplates(int id)
        {
            Request.Method = Method.DELETE;
            Request.Resource = string.Format("api/{0}/DeleteMunkiTemplates/{1}", Resource, id);
            return _apiRequest.Execute<ActionResultDTO>(Request);
        }

        public ApiBoolResponseDTO Export(string path)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/Export/", Resource);
            Request.AddParameter("path", path);
            return _apiRequest.Execute<ApiBoolResponseDTO>(Request);
        }

        public ComputerEntity Get(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/Get/{1}", Resource, id);
            return _apiRequest.Execute<ComputerEntity>(Request);
        }

        public List<ComputerEntity> GetAll()
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetAll", Resource);
            return _apiRequest.Execute<List<ComputerEntity>>(Request);
        }


        public ActiveImagingTaskEntity GetActiveTask(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetActiveTask/{1}", Resource, id);
            return _apiRequest.Execute<ActiveImagingTaskEntity>(Request);
        }

        public List<ComputerWithImage> Search(int limit, string searchstring)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/Search", Resource);
            Request.AddParameter("limit", limit);
            Request.AddParameter("searchstring", searchstring);
            return _apiRequest.Execute<List<ComputerWithImage>>(Request);
        }

        public IEnumerable<ComputerEntity> SearchByName(int limit = 0, string searchstring = "")
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/SearchByName", Resource);
            Request.AddParameter("limit", limit);
            Request.AddParameter("searchstring", searchstring);
            return _apiRequest.Execute<List<ComputerEntity>>(Request);
        }


        public ComputerBootMenuEntity GetBootMenu(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetBootMenu/{1}", Resource, id);
            return _apiRequest.Execute<ComputerBootMenuEntity>(Request);
        }

        public ComputerEntity GetByMac(string mac)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetByMac/", Resource);
            Request.AddParameter("mac", mac);
            return _apiRequest.Execute<ComputerEntity>(Request);
        }

        public IEnumerable<AuditLogEntity> GetComputerAuditLogs(int id,int limit)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetComputerAuditLogs/{1}", Resource, id);
            Request.AddParameter("limit", limit);
            return _apiRequest.Execute<List<AuditLogEntity>>(Request);

        }
        public IEnumerable<ComputerLogEntity> GetComputerLogs(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetComputerLogs/{1}", Resource, id);
            return _apiRequest.Execute<List<ComputerLogEntity>>(Request);
        }

        public IEnumerable<ComputerEntity> GetComputersWithoutGroup(int limit, string searchstring)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetComputersWithoutGroup", Resource);
            Request.AddParameter("limit", limit);
            Request.AddParameter("searchstring", searchstring);
            return _apiRequest.Execute<List<ComputerEntity>>(Request);
        }

        public string GetCount()
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetCount", Resource);
            var responseData = _apiRequest.Execute<ApiStringResponseDTO>(Request);
            return responseData != null ? responseData.Value : string.Empty;
        }


        public string GetEffectiveManifest(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetEffectiveManifest/{1}", Resource, id);
            return _apiRequest.Execute<ApiStringResponseDTO>(Request).Value;
        }


        public IEnumerable<GroupMembershipEntity> GetGroupMemberships(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetGroupMemberships/{1}", Resource, id);
            return _apiRequest.Execute<List<GroupMembershipEntity>>(Request);
        }


        public IEnumerable<ComputerMunkiEntity> GetMunkiTemplates(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetMunkiTemplates/{1}", Resource, id);
            return _apiRequest.Execute<List<ComputerMunkiEntity>>(Request);
        }


        public string GetNonProxyPath(int id, bool isActiveOrCustom)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetNonProxyPath/{1}", Resource, id);
            Request.AddParameter("isActiveOrCustom", isActiveOrCustom);
            var response = _apiRequest.Execute<ApiStringResponseDTO>(Request);
            return response != null ? response.Value : string.Empty;
        }


        public string GetProxyPath(int id, bool isActiveOrCustom, string proxyType)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetProxyPath/{1}", Resource, id);
            Request.AddParameter("isActiveOrCustom", isActiveOrCustom);
            Request.AddParameter("proxyType", proxyType);
            var response = _apiRequest.Execute<ApiStringResponseDTO>(Request);
            return response != null ? response.Value : string.Empty;
        }


        public ComputerProxyReservationEntity GetProxyReservation(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetProxyReservation/{1}", Resource, id);
            return _apiRequest.Execute<ComputerProxyReservationEntity>(Request);
        }

        public int Import(ApiStringResponseDTO csvContents)
        {
            Request.Method = Method.POST;
            Request.Resource = string.Format("api/{0}/Import/", Resource);
            Request.AddJsonBody(csvContents);
            var response = _apiRequest.Execute<ApiIntResponseDTO>(Request);
            return response != null ? response.Value : 0;
        }

        public bool IsComputerActive(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/IsComputerActive/{1}", Resource, id);
            return _apiRequest.Execute<ApiBoolResponseDTO>(Request).Value;
        }

        public ActionResultDTO Post(ComputerEntity tObject)
        {
            Request.Method = Method.POST;
            Request.AddJsonBody(tObject);
            Request.Resource = string.Format("api/{0}/Post/", Resource);
            var response = _apiRequest.Execute<ActionResultDTO>(Request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }

        public ActionResultDTO Put(int id, ComputerEntity tObject)
        {
            Request.Method = Method.PUT;
            Request.AddJsonBody(tObject);
            Request.Resource = string.Format("api/{0}/Put/{1}", Resource, id);
            var response = _apiRequest.Execute<ActionResultDTO>(Request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }


        public string StartDeploy(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/StartDeploy/{1}", Resource, id);
            var response = _apiRequest.Execute<ApiStringResponseDTO>(Request);
            return response != null ? response.Value : string.Empty;
        }

        public string StartPermanentDeploy(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/StartPermanentDeploy/{1}", Resource, id);
            var response = _apiRequest.Execute<ApiStringResponseDTO>(Request);
            return response != null ? response.Value : string.Empty;
        }


        public string StartUpload(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/StartUpload/{1}", Resource, id);
            var response = _apiRequest.Execute<ApiStringResponseDTO>(Request);
            return response != null ? response.Value : string.Empty;
        }


        public bool ToggleBootMenu(int id, bool status)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/ToggleBootMenu/{1}", Resource, id);
            Request.AddParameter("status", status);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }


        public bool ToggleProxyReservation(int id, bool status)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/ToggleProxyReservation/{1}", Resource, id);
            Request.AddParameter("status", status);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }
    }
}