using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Http;
using CloneDeploy_App.BLL;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_App.DTOs;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;


namespace CloneDeploy_App.Controllers
{
    public class ImageProfileController: ApiController
    {
        private readonly ImageProfileServices _imageProfileServices;

        public ImageProfileController()
        {
            _imageProfileServices = new ImageProfileServices();

        }


        [ImageProfileAuth(Permission = "ImageProfileSearch")]
        public IEnumerable<ImageProfileEntity> GetAll()
        {
            return _imageProfileServices.GetAllProfiles();
        }

        [ImageProfileAuth(Permission = "ImageProfileRead")]
        public ImageProfileEntity Get(int id)
        {
            var result = _imageProfileServices.ReadProfile(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

       

        [ImageProfileAuth(Permission = "ImageProfileCreate")]
        public ActionResultDTO Post(ImageProfileEntity imageProfile)
        {
            return _imageProfileServices.AddProfile(imageProfile);
           
        }

    

        [HttpGet]
        [ImageProfileAuth(Permission = "ImageProfileCreate")]
        public ApiBoolResponseDTO Clone(int id)
        {
            _imageProfileServices.CloneProfile(id);
            return new ApiBoolResponseDTO() {Value = true};
        }

        [GlobalAuth(Permission = "GlobalUpdate")]
        public ActionResultDTO Put(int id, ImageProfileEntity imageProfile)
        {
            imageProfile.Id = id;
            var result = _imageProfileServices.UpdateProfile(imageProfile);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [ImageProfileAuth(Permission = "ImageProfileDelete")]
        public ActionResultDTO Delete(int id)
        {
            var result = _imageProfileServices.DeleteProfile(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
          
        }

        [ImageProfileAuth(Permission = "ImageProfileSearch")]
        public IEnumerable<ImageProfileFileFolderEntity> GetFileFolders(int id)
        {

            return _imageProfileServices.SearchImageProfileFileFolders(id);

        }

        [ImageProfileAuth(Permission = "ImageProfileSearch")]
        public IEnumerable<ImageProfileScriptEntity> GetScripts(int id)
        {

            return _imageProfileServices.SearchImageProfileScripts(id);

        }

        [ImageProfileAuth(Permission = "ImageProfileSearch")]
        public IEnumerable<ImageProfileSysprepTagEntity> GetSysprepTags(int id)
        {

            return _imageProfileServices.SearchImageProfileSysprepTags(id);

        }

        [ImageAuth(Permission = "ImageRead")]
        public ApiStringResponseDTO GetMinimumClientSize(int id, int hdNumber)
        {
            return new ApiStringResponseDTO()
            {
                Value = _imageProfileServices.MinimumClientSizeForGridView(id, hdNumber)
            };

        }

        [ImageProfileAuth(Permission = "ImageProfileDelete")]
        [HttpDelete]
        public ActionResultDTO RemoveProfileFileFolders(int id)
        {
            
            return _imageProfileServices.DeleteImageProfileFileFolders(id);

        }

        [ImageProfileAuth(Permission = "ImageProfileDelete")]
        [HttpDelete]
        public ActionResultDTO RemoveProfileScripts(int id)
        {
            return _imageProfileServices.DeleteImageProfileScripts(id);

        }

        [ImageProfileAuth(Permission = "ImageProfileDelete")]
        [HttpDelete]
        public ApiBoolResponseDTO RemoveProfileSysprepTags(int id)
        {
            return new ApiBoolResponseDTO() {Value = _imageProfileServices.DeleteImageProfileSysprepTags(id)};
        }
       
    }
}