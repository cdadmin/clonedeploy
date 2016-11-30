using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Entities.DTOs.ImageSchemaBE;


namespace CloneDeploy_App.Controllers
{
    public class ImageController: ApiController
    {
        [ImageAuth(Permission = "ImageSearch")]
        public IEnumerable<ImageEntity> Get(string searchstring = "")
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();
            return string.IsNullOrEmpty(searchstring)
                ? BLL.Image.SearchImagesForUser(Convert.ToInt32(userId))
                : BLL.Image.SearchImagesForUser(Convert.ToInt32(userId),searchstring);
        }

        [ImageAuth(Permission = "ImageSearch")]
        public IEnumerable<ImageEntity> Search(string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? BLL.Image.SearchImages()
                : BLL.Image.SearchImages(searchstring);
        }

        [ImageAuth(Permission = "ImageSearch")]
        public ApiDTO GetCount()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();
            var ApiDTO = new ApiDTO();
            ApiDTO.Value = BLL.Image.ImageCountUser(Convert.ToInt32(userId));
            return ApiDTO;
        }

        [ImageAuth(Permission = "ImageRead")]
        public IHttpActionResult Get(int id)
        {
            var result = BLL.Image.GetImage(id);
            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }

        [ImageAuth]
        public IHttpActionResult SendImageApprovedMail(int id)
        {
            BLL.Image.SendImageApprovedEmail(id);
            return Ok();
        }

        [ImageAuth(Permission = "ImageCreate")]
        public ActionResultEntity Post(ImageEntity image)
        {
            var actionResult = BLL.Image.AddImage(image);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }

        [GlobalAuth(Permission = "GlobalUpdate")]
        public ActionResultEntity Put(int id, ImageEntity image, string originalName)
        {
            image.Id = id;
            var actionResult = BLL.Image.UpdateImage(image,originalName);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }

        [ImageAuth(Permission = "ImageDelete")]
        public ActionResultEntity Delete(int id)
        {
            var actionResult = BLL.Image.DeleteImage(id);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }

        [ImageProfileAuth(Permission = "ImageProfileSearch")]
        public IEnumerable<ImageProfileEntity> GetImageProfiles(int imageId)
        {
            return BLL.ImageProfile.SearchProfiles(imageId);

        }

        [HttpPost]
        [ImageProfileAuth(Permission = "ImageProfileCreate")]
        public ImageProfileEntity SeedDefaultProfile(int id)
        {
            return BLL.ImageProfile.SeedDefaultImageProfile(id);
        }

        [ImageAuth(Permission = "ImageRead")]
        public IEnumerable<ImageFileInfo> GetPartitionFileInfo(int id, string selectedHd, string selectedPartition)
        {

            return BLL.ImageSchema.GetPartitionImageFileInfoForGridView(id, selectedHd, selectedPartition);

        }

        [ImageAuth(Permission = "ImageRead")]
        public ApiDTO GetImageSizeOnServer(string imageName, string hdNumber)
        {
            var apiDto = new ApiDTO();
            apiDto.Value = BLL.ImageSchema.ImageSizeOnServerForGridView(imageName, hdNumber);
            return apiDto;

        }
    }
}