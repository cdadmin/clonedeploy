using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using CloneDeploy_App.BLL;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;


namespace CloneDeploy_App.Controllers
{
    public class FileFolderController: ApiController
    {
        [GlobalAuth(Permission = "GlobalRead")]
        public IEnumerable<FileFolderEntity> Get(string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? BLL.FileFolder.SearchFileFolders()
                : BLL.FileFolder.SearchFileFolders(searchstring);

        }

        [GlobalAuth(Permission = "GlobalRead")]
        public ApiDTO GetCount()
        {
            var ApiDTO = new ApiDTO();
            ApiDTO.Value = BLL.FileFolder.TotalCount();
            return ApiDTO;
        }

        [GlobalAuth(Permission = "GlobalRead")]
        public IHttpActionResult Get(int id)
        {
            var result = BLL.FileFolder.GetFileFolder(id);
            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }

        [GlobalAuth(Permission = "GlobalCreate")]
        public ActionResultEntity Post(FileFolderEntity fileFolder)
        {
            var actionResult = BLL.FileFolder.AddFileFolder(fileFolder);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }

        [GlobalAuth(Permission = "GlobalUpdate")]
        public ActionResultEntity Put(int id, FileFolderEntity fileFolder)
        {
            fileFolder.Id = id;
            var actionResult = BLL.FileFolder.UpdateFileFolder(fileFolder);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }

        [GlobalAuth(Permission = "GlobalDelete")]
        public ActionResultEntity Delete(int id)
        {
            var actionResult = BLL.FileFolder.DeleteFileFolder(id);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }
    }
}