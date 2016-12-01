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
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;


namespace CloneDeploy_App.Controllers
{
    public class ImageProfileSysprepTagController: ApiController
    {
        private readonly ImageProfileSysprepTagServices _imageProfileSysprepTagServices;

        public ImageProfileSysprepTagController()
        {
            _imageProfileSysprepTagServices = new ImageProfileSysprepTagServices();
        }
       

        [ImageProfileAuth(Permission = "ImageProfileCreate")]
        public ActionResultDTO Post(ImageProfileSysprepTagEntity imageProfileFileFolder)
        {

            return _imageProfileSysprepTagServices.AddImageProfileSysprepTag(imageProfileFileFolder);

        }

       

       
    }
}