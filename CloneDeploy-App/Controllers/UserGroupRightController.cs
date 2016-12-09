using System.Collections.Generic;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;

namespace CloneDeploy_App.Controllers
{
    public class UserGroupRightController: ApiController
    {
        private readonly UserGroupRightServices _userGroupRightServices;

        public UserGroupRightController()
        {
            _userGroupRightServices = new UserGroupRightServices();
        }
      
        [UserAuth(Permission = "Administrator")]
        public ActionResultDTO Post(List<UserGroupRightEntity> listOfRights)
        {
            return _userGroupRightServices.AddUserGroupRights(listOfRights);
        }

        
    }
}