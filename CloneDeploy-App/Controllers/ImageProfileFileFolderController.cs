using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;


namespace CloneDeploy_App.Controllers
{
    public class ImageProfileFileFolderController: ApiController
    {
        private readonly ImageProfileFileFolderService _imageProfileFileFolderService;

        public ImageProfileFileFolderController()
        {
            _imageProfileFileFolderService = new ImageProfileFileFolderService();
        }

        [ImageProfileAuth(Permission = "ImageProfileCreate")]
        public ActionResultDTO Post(ImageProfileFileFolderEntity imageProfileFileFolder)
        {
            return _imageProfileFileFolderService.AddImageProfileFileFolder(imageProfileFileFolder);
        }

       
       
    }
}