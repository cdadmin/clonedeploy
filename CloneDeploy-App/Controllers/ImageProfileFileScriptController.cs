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

        [CustomAuth(Permission = "ProfileSearch")]
        public ActionResultDTO Post(ImageProfileScriptEntity imageProfileScript)
        {
            
            return _imageProfileScriptServices.AddImageProfileScript(imageProfileScript);

        }

       

      
    }
}