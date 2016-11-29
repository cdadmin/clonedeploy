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
    public class UserImageManagement: ApiController
    {
      
        [UserAuth(Permission = "Administrator")]
        public ApiBoolDTO Post(List<UserImageManagementEntity> listOfImages)
        {
            var apiBoolDto = new ApiBoolDTO();
            apiBoolDto.Value = BLL.UserImageManagement.AddUserImageManagements(listOfImages);

            return apiBoolDto;
        }

        
    }
}