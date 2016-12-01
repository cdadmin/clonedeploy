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
    public class UserRightController: ApiController
    {
        private readonly UserRightServices _userRightServices;

        public UserRightController()
        {
            _userRightServices = new UserRightServices();
        }

        [UserAuth(Permission = "Administrator")]
        public ActionResultDTO Post(List<UserRightEntity> listOfRights)
        {
            return _userRightServices.AddUserRights(listOfRights);
        }

        
    }
}