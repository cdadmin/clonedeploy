using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using CloneDeploy_App.BLL;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_App.DTOs;
using CloneDeploy_Entities;


namespace CloneDeploy_App.Controllers
{
    public class ImageProfileSysprepTagController: ApiController
    {
        [ImageProfileAuth(Permission = "ImageProfileSearch")]
        public IEnumerable<ImageProfileSysprepTagEntity> Get(int profileId)
        {
            
             return BLL.ImageProfileSysprepTag.SearchImageProfileSysprepTags(profileId);

        }

        [ImageProfileAuth(Permission = "ImageProfileCreate")]
        public ApiBoolDTO Post(ImageProfileSysprepTagEntity imageProfileFileFolder)
        {
            var apiBoolDto = new ApiBoolDTO();
            apiBoolDto.Value = BLL.ImageProfileSysprepTag.AddImageProfileSysprepTag(imageProfileFileFolder);
          
            return apiBoolDto;
        }

       

        [ImageProfileAuth(Permission = "ImageProfileDelete")]
        public ApiBoolDTO Delete(int id)
        {
            var apiBoolDto = new ApiBoolDTO();
            apiBoolDto.Value = BLL.ImageProfileSysprepTag.DeleteImageProfileSysprepTags(id);
           return apiBoolDto;
        }
    }
}