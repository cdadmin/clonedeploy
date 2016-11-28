using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_App.DTOs;
using CloneDeploy_App.Models;

namespace CloneDeploy_App.Controllers
{
    public class UserGroupManagement: ApiController
    {
      
        [UserAuth(Permission = "Administrator")]
        public ApiBoolDTO Post(List<Models.UserGroupManagement> listOfGroups )
        {
            var apiBoolDto = new ApiBoolDTO();
            apiBoolDto.Value = BLL.UserGroupManagement.AddUserGroupManagements(listOfGroups);

            return apiBoolDto;
        }

        
    }
}