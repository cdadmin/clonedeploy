using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class GroupAPI : BaseAPI
    {
        private readonly ApiRequest _apiRequest;

        public GroupAPI(string resource) : base(resource)
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

        public bool DeleteImageClassifications(int id)
        {
            Request.Method = Method.DELETE;
            Request.Resource = string.Format("api/{0}/DeleteImageClassifications/{1}", Resource, id);
            var result = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            if (result != null)
                return result.Value;
            return false;
        }

        public ApiBoolResponseDTO Export(string path)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/Export/", Resource);
            Request.AddParameter("path", path);
            return _apiRequest.Execute<ApiBoolResponseDTO>(Request);
        }

        public GroupEntity Get(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/Get/{1}", Resource, id);
            return _apiRequest.Execute<GroupEntity>(Request);
        }

        public List<GroupWithImage> Get(int limit, string searchstring)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/Get", Resource);
            Request.AddParameter("limit", limit);
            Request.AddParameter("searchstring", searchstring);
            var result = _apiRequest.Execute<List<GroupWithImage>>(Request);
            if (result == null)
                return new List<GroupWithImage>();
            else
                return result;
        }

        public string GetCount()
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetCount", Resource);
            var responseData = _apiRequest.Execute<ApiStringResponseDTO>(Request);
            return responseData != null ? responseData.Value : string.Empty;
        }

        public GroupBootMenuEntity GetCustomBootMenu(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetCustomBootMenu/{1}", Resource, id);
            return _apiRequest.Execute<GroupBootMenuEntity>(Request);
        }

        public string GetEffectiveManifest(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetEffectiveManifest/{1}", Resource, id);
            var response = _apiRequest.Execute<ApiStringResponseDTO>(Request);
            return response != null ? response.Value : string.Empty;
        }

        public IEnumerable<ComputerWithImage> GetGroupMembers(int id, string searchstring = "")
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetGroupMembers/{1}", Resource, id);
            Request.AddParameter("searchstring", searchstring);
            var result = _apiRequest.Execute<List<ComputerWithImage>>(Request);
            if (result == null)
                return new List<ComputerWithImage>();
            else
                return result;
        }

        public GroupPropertyEntity GetGroupProperties(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetGroupProperties/{1}", Resource, id);
            return _apiRequest.Execute<GroupPropertyEntity>(Request);
        }

        public List<GroupImageClassificationEntity> GetImageClassifications(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetImageClassifications/{1}", Resource, id);
            var result = _apiRequest.Execute<List<GroupImageClassificationEntity>>(Request);
            if (result == null)
                return new List<GroupImageClassificationEntity>();
            else
                return result;
        }

        public string GetMemberCount(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetMemberCount/{1}", Resource, id);
            var response = _apiRequest.Execute<ApiStringResponseDTO>(Request);
            return response != null ? response.Value : string.Empty;
        }

     

        public int Import(ApiStringResponseDTO csvContents)
        {
            Request.Method = Method.POST;
            Request.Resource = string.Format("api/{0}/Import/", Resource);
            Request.AddJsonBody(csvContents);
            var response = _apiRequest.Execute<ApiIntResponseDTO>(Request);
            return response != null ? response.Value : 0;
        }

        public ActionResultDTO Post(GroupEntity tObject)
        {
            Request.Method = Method.POST;
            Request.AddJsonBody(tObject);
            Request.Resource = string.Format("api/{0}/Post/", Resource);
            var response = _apiRequest.Execute<ActionResultDTO>(Request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }

        public ActionResultDTO Put(int id, GroupEntity tObject)
        {
            Request.Method = Method.PUT;
            Request.AddJsonBody(tObject);
            Request.Resource = string.Format("api/{0}/Put/{1}", Resource, id);
            var response = _apiRequest.Execute<ActionResultDTO>(Request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }

        public bool ReCalcSmart()
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/ReCalcSmart", Resource);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }

        public bool RemoveGroupMember(int id, int computerId)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/RemoveGroupMember/{1}", Resource, id);
            Request.AddParameter("computerId", computerId);
            var response = _apiRequest.Execute<ApiBoolResponseDTO>(Request);
            return response != null && response.Value;
        }

 

        public int StartGroupUnicast(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/StartGroupUnicast/{1}", Resource, id);
            var response = _apiRequest.Execute<ApiIntResponseDTO>(Request);
            return response != null ? response.Value : 0;
        }

        public string StartMulticast(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/StartMulticast/{1}", Resource, id);
            var response = _apiRequest.Execute<ApiStringResponseDTO>(Request);
            return response != null ? response.Value : string.Empty;
        }

        public ActionResultDTO UpdateSmartMembership(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/UpdateSmartMembership/{1}", Resource, id);
            return _apiRequest.Execute<ActionResultDTO>(Request);
        }
    }
}