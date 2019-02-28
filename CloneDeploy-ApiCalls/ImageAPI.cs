﻿using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Entities.DTOs.ImageSchemaBE;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class ImageAPI : BaseAPI
    {
        public ImageAPI(string resource) : base(resource)
        {
        }

        public ActionResultDTO Delete(int id)
        {
            Request.Method = Method.DELETE;
            Request.Resource = string.Format("api/{0}/Delete/{1}", Resource, id);
            var response = new ApiRequest().Execute<ActionResultDTO>(Request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }

        public ApiBoolResponseDTO Export(string path)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/Export/", Resource);
            Request.AddParameter("path", path);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(Request);
        }

        public ImageEntity Get(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/Get/{1}", Resource, id);
            return new ApiRequest().Execute<ImageEntity>(Request);
        }

        public List<ImageWithDate> Get(int limit, string searchstring)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/Get", Resource);
            Request.AddParameter("limit", limit);
            Request.AddParameter("searchstring", searchstring);
            var result = new ApiRequest().Execute<List<ImageWithDate>>(Request);
            if (result == null)
                return new List<ImageWithDate>();
            else
                return result;
        }

        public string GetCount()
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetCount", Resource);
            var responseData = new ApiRequest().Execute<ApiStringResponseDTO>(Request);
            return responseData != null ? responseData.Value : string.Empty;
        }

        public IEnumerable<AuditLogEntity> GetImageAuditLogs(int id, int limit)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetImageAuditLogs/{1}", Resource, id);
            Request.AddParameter("limit", limit);
            var result = new ApiRequest().Execute<List<AuditLogEntity>>(Request);
            if (result == null)
                return new List<AuditLogEntity>();
            else
                return result;
        }

        public IEnumerable<ImageProfileEntity> GetImageProfiles(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetImageProfiles/{1}", Resource, id);
            var result = new ApiRequest().Execute<List<ImageProfileEntity>>(Request);
            if (result == null)
                return new List<ImageProfileEntity>();
            else
                return result;
        }

        public string GetImageSizeOnServer(string imageName, string hdNumber)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetImageSizeOnServer/", Resource);
            Request.AddParameter("imageName", imageName);
            Request.AddParameter("hdNumber", hdNumber);
            return new ApiRequest().Execute<ApiStringResponseDTO>(Request).Value;
        }

        public IEnumerable<ImageFileInfo> GetPartitionFileInfo(int id, string selectedHd, string selectedPartition)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetPartitionFileInfo/{1}", Resource, id);
            Request.AddParameter("selectedHd", selectedHd);
            Request.AddParameter("selectedPartition", selectedPartition);
            var result = new ApiRequest().Execute<List<ImageFileInfo>>(Request);
            if (result == null)
                return new List<ImageFileInfo>();
            else
                return result;
        }

        public int Import(ApiStringResponseDTO csvContents)
        {
            Request.Method = Method.POST;
            Request.Resource = string.Format("api/{0}/Import/", Resource);
            Request.AddJsonBody(csvContents);
            return new ApiRequest().Execute<ApiIntResponseDTO>(Request).Value;
        }

        public ActionResultDTO Post(ImageEntity tObject)
        {
            Request.Method = Method.POST;
            Request.AddJsonBody(tObject);
            Request.Resource = string.Format("api/{0}/Post/", Resource);
            var response = new ApiRequest().Execute<ActionResultDTO>(Request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }

        public ActionResultDTO Put(int id, ImageEntity tObject)
        {
            Request.Method = Method.PUT;
            Request.AddJsonBody(tObject);
            Request.Resource = string.Format("api/{0}/Put/{1}", Resource, id);
            var response = new ApiRequest().Execute<ActionResultDTO>(Request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }

        public IEnumerable<ImageWithDate> Search(string searchstring = "")
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/Search/", Resource);
            Request.AddParameter("searchstring", searchstring);
            var result = new ApiRequest().Execute<List<ImageWithDate>>(Request);
            if (result == null)
                return new List<ImageWithDate>();
            else
                return result;
        }

        public ImageProfileEntity SeedDefaultProfile(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/SeedDefaultProfile/{1}", Resource, id);
            return new ApiRequest().Execute<ImageProfileEntity>(Request);
        }

        public bool SendImageApprovedMail(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/SendImageApprovedMail/{1}", Resource, id);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(Request).Value;
        }
    }
}