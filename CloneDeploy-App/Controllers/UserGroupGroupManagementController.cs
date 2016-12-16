using System.Collections.Generic;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;


namespace CloneDeploy_App.Controllers
{
    public class UserGroupGroupManagementController: ApiController
    {
        private readonly UserGroupGroupManagementServices _userGroupGroupManagementServices;

        public UserGroupGroupManagementController()
        {
            _userGroupGroupManagementServices = new UserGroupGroupManagementServices();
        }

        [CustomAuth(Permission = "Administrator")]
        public ActionResultDTO Post(List<UserGroupGroupManagementEntity> listOfGroups)
        {
            return _userGroupGroupManagementServices.AddUserGroupGroupManagements(listOfGroups);
        }

        
    }
}