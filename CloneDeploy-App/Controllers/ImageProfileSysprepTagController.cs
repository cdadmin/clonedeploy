using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;

namespace CloneDeploy_App.Controllers
{
    public class ImageProfileSysprepTagController : ApiController
    {
        private readonly ImageProfileSysprepTagServices _imageProfileSysprepTagServices;

        public ImageProfileSysprepTagController()
        {
            _imageProfileSysprepTagServices = new ImageProfileSysprepTagServices();
        }


        [CustomAuth(Permission = "ProfileSearch")]
        public ActionResultDTO Post(ImageProfileSysprepTagEntity imageProfileFileFolder)
        {
            return _imageProfileSysprepTagServices.AddImageProfileSysprepTag(imageProfileFileFolder);
        }
    }
}