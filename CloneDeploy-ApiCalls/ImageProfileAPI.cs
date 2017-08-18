using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class ImageProfileAPI : BaseAPI
    {
        private readonly ApiRequest _apiRequest;

        public ImageProfileAPI(string resource) : base(resource)
        {
            _apiRequest = new ApiRequest();
        }

        public bool Clone(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/Clone/{1}", Resource, id);
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

        public ImageProfileWithImage Get(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/Get/{1}", Resource, id);
            return _apiRequest.Execute<ImageProfileWithImage>(Request);
        }

        public List<ImageProfileWithImage> Get(int limit, string searchstring)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/Get", Resource);
            Request.AddParameter("limit", limit);
            Request.AddParameter("searchstring", searchstring);
            return _apiRequest.Execute<List<ImageProfileWithImage>>(Request);
        }

        public IEnumerable<ImageProfileFileFolderEntity> GetFileFolders(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetFileFolders/{1}", Resource, id);
            return _apiRequest.Execute<List<ImageProfileFileFolderEntity>>(Request);
        }

        public string GetMinimumClientSize(int id, int hdNumber)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetMinimumClientSize/{1}", Resource, id);
            return _apiRequest.Execute<ApiStringResponseDTO>(Request).Value;
        }

        public IEnumerable<ImageProfileScriptEntity> GetScripts(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetScripts/{1}", Resource, id);
            return _apiRequest.Execute<List<ImageProfileScriptEntity>>(Request);
        }

        public IEnumerable<ImageProfileSysprepTagEntity> GetSysprepTags(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetSysprepTags/{1}", Resource, id);
            return _apiRequest.Execute<List<ImageProfileSysprepTagEntity>>(Request);
        }

        public ActionResultDTO Post(ImageProfileEntity tObject)
        {
            Request.Method = Method.POST;
            Request.AddJsonBody(tObject);
            Request.Resource = string.Format("api/{0}/Post/", Resource);
            var response = _apiRequest.Execute<ActionResultDTO>(Request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }

        public ActionResultDTO Put(int id, ImageProfileEntity tObject)
        {
            Request.Method = Method.PUT;
            Request.AddJsonBody(tObject);
            Request.Resource = string.Format("api/{0}/Put/{1}", Resource, id);
            var response = _apiRequest.Execute<ActionResultDTO>(Request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }

        public ActionResultDTO RemoveProfileFileFolders(int id)
        {
            Request.Method = Method.DELETE;
            Request.Resource = string.Format("api/{0}/RemoveProfileFileFolders/{1}", Resource, id);
            return _apiRequest.Execute<ActionResultDTO>(Request);
        }

        public ActionResultDTO RemoveProfileScripts(int id)
        {
            Request.Method = Method.DELETE;
            Request.Resource = string.Format("api/{0}/RemoveProfileScripts/{1}", Resource, id);
            return _apiRequest.Execute<ActionResultDTO>(Request);
        }

        public bool RemoveProfileSysprepTags(int id)
        {
            Request.Method = Method.DELETE;
            Request.Resource = string.Format("api/{0}/RemoveProfileSysprepTags/{1}", Resource, id);
            return _apiRequest.Execute<ApiBoolResponseDTO>(Request).Value;
        }
    }
}