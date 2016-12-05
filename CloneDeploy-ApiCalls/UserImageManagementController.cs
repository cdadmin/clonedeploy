using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_App.DTOs;
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
      
        [UserAuth(Permission = "Administrator")]
        public ActionResultDTO Post(List<UserImageManagementEntity> listOfImages)
        {
            return  _userImageManagementServices.AddUserImageManagements(listOfImages);

        }

        
    }
}