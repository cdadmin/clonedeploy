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
using CloneDeploy_Services;


namespace CloneDeploy_App.Controllers
{
    public class PortController: ApiController
    {
        private readonly PortServices _portServices;

        public PortController()
        {
            _portServices = new PortServices();
        }
       

        [AdminAuth(Permission = "AdminUpdate")]
        public ApiBoolResponseDTO Post(PortEntity port)
        {
            return new ApiBoolResponseDTO() {Value = _portServices.AddPort(port)};
        }

       

      
    }
}