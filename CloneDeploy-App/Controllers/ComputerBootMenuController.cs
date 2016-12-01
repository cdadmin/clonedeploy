using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using CloneDeploy_App.BLL;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_App.DTOs;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;


namespace CloneDeploy_App.Controllers
{
    public class ComputerBootMenuController : ApiController
    {

        private readonly ComputerBootMenuServices _computerBootMenuServices;

        public ComputerBootMenuController()
        {
            _computerBootMenuServices = new ComputerBootMenuServices();
        }

        [ComputerAuth(Permission = "ComputerUpdate")]
        public ApiBoolResponseDTO Post(ComputerBootMenuEntity computerBootMenu)
        {
            return new ApiBoolResponseDTO() {Value = _computerBootMenuServices.UpdateComputerBootMenu(computerBootMenu)};
        }

       

      
    }
}