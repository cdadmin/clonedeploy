using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;


namespace CloneDeploy_App.Controllers
{
    public class FileFolderController: ApiController
    {
        private readonly FileFolderServices _fileFolderServices;

        public FileFolderController()
        {
            _fileFolderServices = new FileFolderServices();
        }

        [CustomAuth(Permission = "GlobalRead")]
        public IEnumerable<FileFolderEntity> GetAll(string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? _fileFolderServices.SearchFileFolders()
                : _fileFolderServices.SearchFileFolders(searchstring);

        }

        [CustomAuth(Permission = "GlobalRead")]
        public ApiStringResponseDTO GetCount()
        {

            return new ApiStringResponseDTO() {Value = _fileFolderServices.TotalCount()};

        }

        [CustomAuth(Permission = "GlobalRead")]
        public FileFolderEntity Get(int id)
        {
            var result = _fileFolderServices.GetFileFolder(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [CustomAuth(Permission = "GlobalCreate")]
        public ActionResultDTO Post(FileFolderEntity fileFolder)
        {
            var result = _fileFolderServices.AddFileFolder(fileFolder);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [CustomAuth(Permission = "GlobalUpdate")]
        public ActionResultDTO Put(int id, FileFolderEntity fileFolder)
        {
            fileFolder.Id = id;
            var result = _fileFolderServices.UpdateFileFolder(fileFolder);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [CustomAuth(Permission = "GlobalDelete")]
        public ActionResultDTO Delete(int id)
        {
            var result = _fileFolderServices.DeleteFileFolder(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }
    }
}