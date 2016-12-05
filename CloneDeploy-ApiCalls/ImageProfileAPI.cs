using System.Collections.Generic;
using CloneDeploy_App.DTOs;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class ImageProfileAPI : GenericAPI<ImageProfileEntity>
    {
        public ImageProfileAPI(string resource):base(resource)
        {
		
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