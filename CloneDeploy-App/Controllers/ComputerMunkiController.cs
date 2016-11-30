using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using CloneDeploy_App.BLL;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;


namespace CloneDeploy_App.Controllers
{
    public class ComputerMunkiController:ApiController
    {

        [ComputerAuth(Permission = "GlobalRead")]
        public IHttpActionResult GetTemplateComputers(int id)
        {
            var munkiComputers = BLL.ComputerMunki.GetComputersForManifestTemplate(id);
            if (munkiComputers == null)
                return NotFound();
            else
                return Ok(munkiComputers);
        }

        

        [ComputerAuth(Permission = "ComputerCreate")]
        public ActionResultEntity Post(ComputerMunkiEntity computerMunki)
        {
            var actionResult = BLL.ComputerMunki.AddMunkiTemplates(computerMunki);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }

       
    }
}