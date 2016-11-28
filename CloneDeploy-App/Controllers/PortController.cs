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
    public class PortController: ApiController
    {
       

        [AdminAuth(Permission = "AdminUpdate")]
        public ApiBoolDTO Post(Models.Port port)
        {
            var apiBoolDto = new ApiBoolDTO();
            apiBoolDto.Value = BLL.Port.AddPort(port);
          
            return apiBoolDto;
        }

       

      
    }
}