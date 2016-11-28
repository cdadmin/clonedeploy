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
    public class ScriptController: ApiController
    {
        [GlobalAuth(Permission = "GlobalRead")]
        public IEnumerable<Models.Script> Get(string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? BLL.Script.SearchScripts()
                : BLL.Script.SearchScripts(searchstring);

        }

        [GlobalAuth(Permission = "GlobalRead")]
        public ApiDTO GetCount()
        {
            var ApiDTO = new ApiDTO();
            ApiDTO.Value = BLL.Script.TotalCount();
            return ApiDTO;
        }

        [GlobalAuth(Permission = "GlobalRead")]
        public IHttpActionResult Get(int id)
        {
            var result = BLL.Script.GetScript(id);
            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }

        [GlobalAuth(Permission = "GlobalCreate")]
        public ActionResult Post(Models.Script script)
        {
            var actionResult = BLL.Script.AddScript(script);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }

        [GlobalAuth(Permission = "GlobalUpdate")]
        public Models.ActionResult Put(int id, Models.Script script)
        {
            script.Id = id;
            var actionResult = BLL.Script.UpdateScript(script);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }

        [GlobalAuth(Permission = "GlobalDelete")]
        public ApiBoolDTO Delete(int id)
        {
            var apiBoolDto = new ApiBoolDTO();
            apiBoolDto.Value = BLL.Script.DeleteScript(id);
           return apiBoolDto;
        }
    }
}