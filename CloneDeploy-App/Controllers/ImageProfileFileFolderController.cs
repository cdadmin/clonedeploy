using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_App.DTOs;
using CloneDeploy_App.Models;

namespace CloneDeploy_App.Controllers
{
    public class ImageProfileFileFolderController: ApiController
    {
        [ImageProfileAuth(Permission = "ImageProfileSearch")]
        public IEnumerable<Models.ImageProfileFileFolder> Get(int profileId)
        {
            
             return BLL.ImageProfileFileFolder.SearchImageProfileFileFolders(profileId);

        }

        [ImageProfileAuth(Permission = "ImageProfileCreate")]
        public ApiBoolDTO Post(Models.ImageProfileFileFolder imageProfileFileFolder)
        {
            var apiBoolDto = new ApiBoolDTO();
            apiBoolDto.Value = BLL.ImageProfileFileFolder.AddImageProfileFileFolder(imageProfileFileFolder);
          
            return apiBoolDto;
        }

       

        [ImageProfileAuth(Permission = "ImageProfileDelete")]
        public ApiBoolDTO Delete(int id)
        {
            var apiBoolDto = new ApiBoolDTO();
            apiBoolDto.Value = BLL.ImageProfileFileFolder.DeleteImageProfileFileFolders(id);
           return apiBoolDto;
        }
    }
}