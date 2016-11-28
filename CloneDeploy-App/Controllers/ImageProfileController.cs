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
using CloneDeploy_App.DTOs;
using CloneDeploy_App.Models;

namespace CloneDeploy_App.Controllers
{
    public class ImageProfileController: ApiController
    {
        [ImageProfileAuth(Permission = "ImageProfileSearch")]
        public IEnumerable<Models.ImageProfile> GetByImage(int imageId)
        {
            return BLL.ImageProfile.SearchProfiles(imageId);

        }


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
        public ActionResult Post(Models.ImageProfile imageProfile)
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
        public ImageProfile Seed(Models.Image image)
        {
           return BLL.ImageProfile.SeedDefaultImageProfile(image);
          
           
        }

        [HttpPost]
        [ImageProfileAuth(Permission = "ImageProfileCreate")]
        public IHttpActionResult Clone(Models.ImageProfile imageProfile)
        {
            BLL.ImageProfile.CloneProfile(imageProfile);
            return Ok();
        }

        [GlobalAuth(Permission = "GlobalUpdate")]
        public Models.ActionResult Put(int id, Models.ImageProfile imageProfile)
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
    }
}