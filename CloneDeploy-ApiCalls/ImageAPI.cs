using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Entities.DTOs.ImageSchemaBE;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class ImageAPI : BaseAPI
    {
        public ImageAPI(string resource):base(resource)
        {
		
        }

        public List<ImageEntity> GetAll(int limit, string searchstring)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetAll", _resource);
            _request.AddParameter("limit", limit);
            _request.AddParameter("searchstring", searchstring);
            return new ApiRequest().Execute<List<ImageEntity>>(_request);
        }

        public ImageEntity Get(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/Get/{1}", _resource, id);
            return new ApiRequest().Execute<ImageEntity>(_request);
        }

        public string GetCount()
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetCount", _resource);
            var responseData = new ApiRequest().Execute<ApiStringResponseDTO>(_request);
            return responseData != null ? responseData.Value : string.Empty;

        }

        public ActionResultDTO Put(int id, ImageEntity tObject)
        {
            _request.Method = Method.PUT;
            _request.AddJsonBody(tObject);
            _request.Resource = string.Format("api/{0}/Put/{1}", _resource, id);
            var response = new ApiRequest().Execute<ActionResultDTO>(_request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }

        public ActionResultDTO Post(ImageEntity tObject)
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

        public IEnumerable<ImageEntity> Search(string searchstring = "")
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/Search/", _resource);
            _request.AddParameter("searchstring", searchstring);
            return new ApiRequest().Execute<List<ImageEntity>>(_request);
        }

       
        public bool SendImageApprovedMail(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/SendImageApprovedMail/{1}", _resource, id);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;
        }

     
        public IEnumerable<ImageProfileEntity> GetImageProfiles(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetImageProfiles/{1}", _resource, id);
            return new ApiRequest().Execute<List<ImageProfileEntity>>(_request);
        }

        public ApiBoolResponseDTO Export(string path)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/Export/", _resource);
            _request.AddParameter("path", path);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request);
        }

        public ImageProfileEntity SeedDefaultProfile(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/SeedDefaultProfile/{1}", _resource, id);
            return new ApiRequest().Execute<ImageProfileEntity>(_request);
        }

        public int Import(ApiStringResponseDTO csvContents)
        {
            _request.Method = Method.POST;
            _request.Resource = string.Format("api/{0}/Import/", _resource);
            _request.AddJsonBody(csvContents);
            return new ApiRequest().Execute<ApiIntResponseDTO>(_request).Value;
        }

        public IEnumerable<ImageFileInfo> GetPartitionFileInfo(int id, string selectedHd, string selectedPartition)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetPartitionFileInfo/{1}", _resource, id);
            _request.AddParameter("selectedHd", selectedHd);
            _request.AddParameter("selectedPartition", selectedPartition);
            return new ApiRequest().Execute<List<ImageFileInfo>>(_request);

        }


        public string GetImageSizeOnServer(string imageName, string hdNumber)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetImageSizeOnServer/", _resource);
            _request.AddParameter("imageName", imageName);
            _request.AddParameter("hdNumber", hdNumber);
            return new ApiRequest().Execute<ApiStringResponseDTO>(_request).Value;

        }
    }
}