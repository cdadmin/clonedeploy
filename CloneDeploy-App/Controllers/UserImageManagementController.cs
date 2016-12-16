using System.Collections.Generic;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;


namespace CloneDeploy_App.Controllers
{
    public class UserImageManagementController: ApiController
    {
        private readonly UserImageManagementServices _userImageManagementServices;

        public UserImageManagementController()
        {
            _userImageManagementServices = new UserImageManagementServices();
        }
      
        [CustomAuth(Permission = "Administrator")]
        public ActionResultDTO Post(List<UserImageManagementEntity> listOfImages)
        {
            return  _userImageManagementServices.AddUserImageManagements(listOfImages);

        }

        
    }
}