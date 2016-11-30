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


namespace CloneDeploy_App.Controllers
{
    public class ImageProfileController: ApiController
    {

        [ImageProfileAuth(Permission = "ImageProfileSearch")]
        public IHttpActionResult GetAll()
        {
            var result = BLL.ImageProfile.GetAllProfiles();
            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }

      

        [ImageProfileAuth(Permission = "ImageProfileRead")]
        public IHttpActionResult Get(int id)
        {
            var result = BLL.ImageProfile.ReadProfile(id);
            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }

       

        [ImageProfileAuth(Permission = "ImageProfileCreate")]
        public ActionResultEntity Post(ImageProfileEntity imageProfile)
        {
            var actionResult = BLL.ImageProfile.AddProfile(imageProfile);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }

    

        [HttpPost]
        [ImageProfileAuth(Permission = "ImageProfileCreate")]
        public IHttpActionResult Clone(ImageProfileEntity imageProfile)
        {
            BLL.ImageProfile.CloneProfile(imageProfile);
            return Ok();
        }

        [GlobalAuth(Permission = "GlobalUpdate")]
        public ActionResultEntity Put(int id, ImageProfileEntity imageProfile)
        {
            imageProfile.Id = id;
            var actionResult = BLL.ImageProfile.UpdateProfile(imageProfile);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }

        [ImageProfileAuth(Permission = "ImageProfileDelete")]
        public ApiBoolDTO Delete(int id)
        {
            var apiBoolDto = new ApiBoolDTO();
            apiBoolDto.Value = BLL.ImageProfile.DeleteProfile(id);
            return apiBoolDto;
          
        }

        [ImageProfileAuth(Permission = "ImageProfileSearch")]
        public IEnumerable<ImageProfileFileFolderEntity> GetFileFolder(int profileId)
        {

            return BLL.ImageProfileFileFolder.SearchImageProfileFileFolders(profileId);

        }

        [ImageProfileAuth(Permission = "ImageProfileSearch")]
        public IEnumerable<ImageProfileScriptEntity> GetScript(int profileId)
        {

            return BLL.ImageProfileScript.SearchImageProfileScripts(profileId);

        }

        [ImageProfileAuth(Permission = "ImageProfileSearch")]
        public IEnumerable<ImageProfileSysprepTagEntity> GetSysprepTag(int profileId)
        {

            return BLL.ImageProfileSysprepTag.SearchImageProfileSysprepTags(profileId);

        }

        [ImageAuth(Permission = "ImageRead")]
        public ApiDTO GetMinimumClientSize(int profileId, int hdNumber)
        {
            var apiDto = new ApiDTO();
            apiDto.Value = BLL.ImageSchema.MinimumClientSizeForGridView(profileId, hdNumber);
            return apiDto;

        }
    }
}