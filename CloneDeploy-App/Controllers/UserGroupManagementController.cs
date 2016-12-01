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
    public class UserGroupManagement: ApiController
    {
        private readonly UserGroupManagementServices _userGroupManagementServices;

        public UserGroupManagement()
        {
            _userGroupManagementServices = new UserGroupManagementServices();
        }

        [UserAuth(Permission = "Administrator")]
        public ActionResultDTO Post(List<UserGroupManagementEntity> listOfGroups)
        {
            return _userGroupManagementServices.AddUserGroupManagements(listOfGroups);
        }

        
    }
}