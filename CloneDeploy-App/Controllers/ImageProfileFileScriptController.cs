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
    public class ImageProfileScriptController: ApiController
    {
        [ImageProfileAuth(Permission = "ImageProfileSearch")]
        public IEnumerable<ImageProfileScriptEntity> Get(int profileId)
        {
            
             return BLL.ImageProfileScript.SearchImageProfileScripts(profileId);

        }

        [ImageProfileAuth(Permission = "ImageProfileCreate")]
        public ApiBoolDTO Post(ImageProfileScriptEntity imageProfileFileFolder)
        {
            var apiBoolDto = new ApiBoolDTO();
            apiBoolDto.Value = BLL.ImageProfileScript.AddImageProfileScript(imageProfileFileFolder);
          
            return apiBoolDto;
        }

       

        [ImageProfileAuth(Permission = "ImageProfileDelete")]
        public ApiBoolDTO Delete(int id)
        {
            var apiBoolDto = new ApiBoolDTO();
            apiBoolDto.Value = BLL.ImageProfileScript.DeleteImageProfileScripts(id);
           return apiBoolDto;
        }
    }
}