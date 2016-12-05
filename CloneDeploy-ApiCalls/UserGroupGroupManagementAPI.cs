using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using CloneDeploy_ApiCalls;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_App.DTOs;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;


namespace CloneDeploy_App.Controllers
{
    public class UserGroupGroupManagementAPI : GenericAPI<UserGroupGroupManagementEntity>
    {
        public UserGroupGroupManagementAPI(string resource):base(resource)
        {
		
        }
  

        [UserAuth(Permission = "Administrator")]
        public ActionResultDTO Post(List<UserGroupGroupManagementEntity> listOfGroups)
        {
            return _userGroupGroupManagementServices.AddUserGroupGroupManagements(listOfGroups);
        }

        
    }
}