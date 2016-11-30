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


namespace CloneDeploy_App.Controllers
{
    public class ComputerBootMenuController : ApiController
    {

       

       

        [ComputerAuth(Permission = "ComputerUpdate")]
        public ActionResultEntity Post(ComputerBootMenuEntity computerBootMenu)
        {
            var actionResult = new ActionResultEntity();
            actionResult.Success = BLL.ComputerBootMenu.UpdateComputerBootMenu(computerBootMenu);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }

       

      
    }
}