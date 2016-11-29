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


namespace CloneDeploy_App.Controllers
{
    public class UserGroupImageManagementController: ApiController
    {
      
        [UserAuth(Permission = "Administrator")]
        public ApiBoolDTO Post(List<UserGroupImageManagementEntity> listOfImages)
        {
            var apiBoolDto = new ApiBoolDTO();
            apiBoolDto.Value = BLL.UserGroupImageManagement.AddUserGroupImageManagements(listOfImages);

            return apiBoolDto;
        }

        
    }
}