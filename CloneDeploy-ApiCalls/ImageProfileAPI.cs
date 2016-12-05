using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Http;
using CloneDeploy_ApiCalls;
using CloneDeploy_App.BLL;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_App.DTOs;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;


namespace CloneDeploy_App.Controllers
{
    public class ImageProfileAPI : GenericAPI<ImageProfileEntity>
    {
        public ImageProfileAPI(string resource):base(resource)
        {
		
        }


     

    

        [HttpPost]
        [ImageProfileAuth(Permission = "ImageProfileCreate")]
        public ApiBoolResponseDTO Clone(int id)
        {
            _imageProfileServices.CloneProfile(id);
            return new ApiBoolResponseDTO() {Value = true};
        }

    

        [ImageProfileAuth(Permission = "ImageProfileSearch")]
        public IEnumerable<ImageProfileFileFolderEntity> GetFileFolder(int profileId)
        {

            return _imageProfileServices.SearchImageProfileFileFolders(profileId);

        }

        [ImageProfileAuth(Permission = "ImageProfileSearch")]
        public IEnumerable<ImageProfileScriptEntity> GetScript(int profileId)
        {

            return _imageProfileServices.SearchImageProfileScripts(profileId);

        }

        [ImageProfileAuth(Permission = "ImageProfileSearch")]
        public IEnumerable<ImageProfileSysprepTagEntity> GetSysprepTag(int profileId)
        {

            return _imageProfileServices.SearchImageProfileSysprepTags(profileId);

        }

        [ImageAuth(Permission = "ImageRead")]
        public ApiStringResponseDTO GetMinimumClientSize(int profileId, int hdNumber)
        {
            return new ApiStringResponseDTO()
            {
                Value = _imageProfileServices.MinimumClientSizeForGridView(profileId, hdNumber)
            };

        }

        [ImageProfileAuth(Permission = "ImageProfileDelete")]
        public ActionResultDTO RemoveProfileFileFolders(int id)
        {
            
            return _imageProfileServices.DeleteImageProfileFileFolders(id);

        }

        [ImageProfileAuth(Permission = "ImageProfileDelete")]
        public ActionResultDTO RemoveProfileScripts(int id)
        {
            return _imageProfileServices.DeleteImageProfileScripts(id);

        }

        [ImageProfileAuth(Permission = "ImageProfileDelete")]
        public ApiBoolResponseDTO RemoveProfileSysprepTags(int id)
        {
            return new ApiBoolResponseDTO() {Value = _imageProfileServices.DeleteImageProfileSysprepTags(id)};
        }
       
    }
}