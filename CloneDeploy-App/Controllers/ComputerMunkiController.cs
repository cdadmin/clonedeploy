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
        
        [ComputerAuth(Permission = "ComputerRead")]
        public IHttpActionResult Get(int id)
        {
            var munkiTemplates = BLL.ComputerMunki.Get(id);
            if (munkiTemplates == null)
                return NotFound();
            else
                return Ok(munkiTemplates);
        }

        [ComputerAuth(Permission = "GlobalRead")]
        public IHttpActionResult GetTemplateComputers(int id)
        {
            var munkiComputers = BLL.ComputerMunki.GetComputersForManifestTemplate(id);
            if (munkiComputers == null)
                return NotFound();
            else
                return Ok(munkiComputers);
        }

        [ComputerAuth(Permission = "ComputerRead")]
        public IHttpActionResult GetEffectiveManifest(int id)
        {
            var result = new ApiDTO();
            var effectiveManifest = new BLL.Workflows.EffectiveMunkiTemplate().Computer(id);
            result.Value = Encoding.UTF8.GetString(effectiveManifest.ToArray());
            return Ok(result);
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

        [ComputerAuth(Permission = "ComputerDelete")]
        public ActionResultEntity Delete(int id)
        {
            var actionResult = BLL.ComputerMunki.DeleteMunkiTemplates(id);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }
    }
}