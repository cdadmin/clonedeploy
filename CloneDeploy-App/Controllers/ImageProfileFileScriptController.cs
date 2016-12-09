using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;


namespace CloneDeploy_App.Controllers
{
    public class ImageProfileScriptController: ApiController
    {
        private readonly ImageProfileScriptServices _imageProfileScriptServices;

        public ImageProfileScriptController()
        {
            _imageProfileScriptServices = new ImageProfileScriptServices();
        }

        [ImageProfileAuth(Permission = "ImageProfileCreate")]
        public ActionResultDTO Post(ImageProfileScriptEntity imageProfileFileFolder)
        {
            
            return _imageProfileScriptServices.AddImageProfileScript(imageProfileFileFolder);

        }

       

      
    }
}