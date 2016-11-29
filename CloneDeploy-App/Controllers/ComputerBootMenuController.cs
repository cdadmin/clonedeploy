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

        [ComputerAuth(Permission = "ComputerRead")]
        public IHttpActionResult Get(int id)
        {
            var result = BLL.ComputerBootMenu.GetComputerBootMenu(id);
            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }

        [HttpGet]
        [ComputerAuth(Permission = "ComputerUpdate")]
        public ApiBoolDTO Toggle(int id, bool status)
        {
            var result = new ApiBoolDTO();
            result.Value = BLL.ComputerBootMenu.ToggleComputerBootMenu(id, status);
            return result;
        }

        [ComputerAuth(Permission = "ComputerUpdate")]
        public ActionResultEntity Put(ComputerBootMenuEntity computerBootMenu)
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

        [ComputerAuth(Permission = "ComputerDelete")]
        public ActionResultEntity Delete(int id)
        {
            var actionResult = BLL.ComputerBootMenu.DeleteComputerBootMenus(id);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }

        [HttpPost]
        [ComputerAuth(Permission = "ComputerUpdate")]
        public ApiBoolDTO CreateCustomBootFiles(ComputerEntity computer)
        {
            var result = new ApiBoolDTO(){Value = true};
            BLL.ComputerBootMenu.CreateBootFiles(computer);
           
            return result;
        }

        [HttpPost]
        [ComputerAuth(Permission = "ComputerRead")]
        public ApiDTO GetProxyPath(ComputerEntity computer, bool isActiveOrCustom, string proxyType)
        {
            var result = new ApiDTO();
            result.Value = BLL.ComputerBootMenu.GetComputerProxyPath(computer, isActiveOrCustom,proxyType);
            return result;
        }

        [HttpPost]
        [ComputerAuth(Permission = "ComputerRead")]
        public ApiDTO GetNonProxyPath(ComputerEntity computer, bool isActiveOrCustom)
        {
            var result = new ApiDTO();
            result.Value = BLL.ComputerBootMenu.GetComputerNonProxyPath(computer, isActiveOrCustom);
            return result;
        }
    }
}