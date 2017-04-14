using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class ImageProfileAPI : BaseAPI
    {
        public ImageProfileAPI(string resource):base(resource)
        {
		
        }

        public List<ImageProfileEntity> GetAll(int limit, string searchstring)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetAll", _resource);
            _request.AddParameter("limit", limit);
            _request.AddParameter("searchstring", searchstring);
            return new ApiRequest().Execute<List<ImageProfileEntity>>(_request);
        }

   
        public ImageProfileEntity Get(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/Get/{1}", _resource, id);
            return new ApiRequest().Execute<ImageProfileEntity>(_request);
        }

        public ActionResultDTO Put(int id, ImageProfileEntity tObject)
        {
            _request.Method = Method.PUT;
            _request.AddJsonBody(tObject);
            _request.Resource = string.Format("api/{0}/Put/{1}", _resource, id);
            var response = new ApiRequest().Execute<ActionResultDTO>(_request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }

        public ActionResultDTO Post(ImageProfileEntity tObject)
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
        public bool Clone(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/Clone/{1}", _resource, id);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;
        }

    

        public IEnumerable<ImageProfileFileFolderEntity> GetFileFolders(int id)
        {

            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetFileFolders/{1}", _resource, id);
            return new ApiRequest().Execute<List<ImageProfileFileFolderEntity>>(_request);

        }

        public IEnumerable<ImageProfileScriptEntity> GetScripts(int id)
        {

            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetScripts/{1}", _resource, id);
            return new ApiRequest().Execute<List<ImageProfileScriptEntity>>(_request);

        }


        public IEnumerable<ImageProfileSysprepTagEntity> GetSysprepTags(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetSysprepTags/{1}", _resource, id);
            return new ApiRequest().Execute<List<ImageProfileSysprepTagEntity>>(_request);

        }


        public string GetMinimumClientSize(int id, int hdNumber)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetMinimumClientSize/{1}", _resource, id);
            return new ApiRequest().Execute<ApiStringResponseDTO>(_request).Value;

        }


        public ActionResultDTO RemoveProfileFileFolders(int id)
        {
            _request.Method = Method.DELETE;
            _request.Resource = string.Format("api/{0}/RemoveProfileFileFolders/{1}", _resource, id);
            return new ApiRequest().Execute<ActionResultDTO>(_request);


        }


        public ActionResultDTO RemoveProfileScripts(int id)
        {
            _request.Method = Method.DELETE;
            _request.Resource = string.Format("api/{0}/RemoveProfileScripts/{1}", _resource, id);
            return new ApiRequest().Execute<ActionResultDTO>(_request);

        }


        public bool RemoveProfileSysprepTags(int id)
        {
            _request.Method = Method.DELETE;
            _request.Resource = string.Format("api/{0}/RemoveProfileSysprepTags/{1}", _resource, id);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;
        }
       
    }
}