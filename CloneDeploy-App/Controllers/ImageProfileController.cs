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
    public class ImageProfileController: ApiController
    {
        private readonly ImageProfileServices _imageProfileServices;

        public ImageProfileController()
        {
            _imageProfileServices = new ImageProfileServices();

        }


        [CustomAuth(Permission = "ProfileSearch")]
        public IEnumerable<ImageProfileWithImage> GetAll()
        {
            return _imageProfileServices.GetAllProfiles();
        }

        [CustomAuth(Permission = "ProfileRead")]
        public ImageProfileWithImage Get(int id)
        {
            var result = _imageProfileServices.ReadProfile(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

       

        [CustomAuth(Permission = "ProfileCreate")]
        public ActionResultDTO Post(ImageProfileEntity imageProfile)
        {
            return _imageProfileServices.AddProfile(imageProfile);
           
        }

    

        [HttpGet]
        [CustomAuth(Permission = "ProfileCreate")]
        public ApiBoolResponseDTO Clone(int id)
        {
            _imageProfileServices.CloneProfile(id);
            return new ApiBoolResponseDTO() {Value = true};
        }

        [CustomAuth(Permission = "ProfileUpdate")]
        public ActionResultDTO Put(int id, ImageProfileEntity imageProfile)
        {
            imageProfile.Id = id;
            var result = _imageProfileServices.UpdateProfile(imageProfile);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [CustomAuth(Permission = "ProfileDelete")]
        public ActionResultDTO Delete(int id)
        {
            var result = _imageProfileServices.DeleteProfile(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
          
        }

        [CustomAuth(Permission = "ProfileRead")]
        public IEnumerable<ImageProfileFileFolderEntity> GetFileFolders(int id)
        {

            return _imageProfileServices.SearchImageProfileFileFolders(id);

        }

        [CustomAuth(Permission = "ProfileRead")]
        public IEnumerable<ImageProfileScriptEntity> GetScripts(int id)
        {

            return _imageProfileServices.SearchImageProfileScripts(id);

        }

        [CustomAuth(Permission = "ProfileRead")]
        public IEnumerable<ImageProfileSysprepTagEntity> GetSysprepTags(int id)
        {

            return _imageProfileServices.SearchImageProfileSysprepTags(id);

        }

        [CustomAuth(Permission = "ProfileSearch")]
        public ApiStringResponseDTO GetMinimumClientSize(int id, int hdNumber)
        {
            return new ApiStringResponseDTO()
            {
                Value = _imageProfileServices.MinimumClientSizeForGridView(id, hdNumber)
            };

        }

        [CustomAuth(Permission = "ProfileUpdate")]
        [HttpDelete]
        public ActionResultDTO RemoveProfileFileFolders(int id)
        {
            
            return _imageProfileServices.DeleteImageProfileFileFolders(id);

        }

        [CustomAuth(Permission = "ProfileUpdate")]
        [HttpDelete]
        public ActionResultDTO RemoveProfileScripts(int id)
        {
            return _imageProfileServices.DeleteImageProfileScripts(id);

        }

        [CustomAuth(Permission = "ProfileUpdate")]
        [HttpDelete]
        public ApiBoolResponseDTO RemoveProfileSysprepTags(int id)
        {
            return new ApiBoolResponseDTO() {Value = _imageProfileServices.DeleteImageProfileSysprepTags(id)};
        }
       
    }
}