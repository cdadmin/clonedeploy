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
    public class SysprepTagController: ApiController
    {
        [GlobalAuth(Permission = "GlobalRead")]
        public IEnumerable<Models.SysprepTag> Get(string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? BLL.SysprepTag.SearchSysprepTags()
                : BLL.SysprepTag.SearchSysprepTags(searchstring);

        }

        [GlobalAuth(Permission = "GlobalRead")]
        public ApiDTO GetCount()
        {
            var ApiDTO = new ApiDTO();
            ApiDTO.Value = BLL.SysprepTag.TotalCount();
            return ApiDTO;
        }

        [GlobalAuth(Permission = "GlobalRead")]
        public IHttpActionResult Get(int id)
        {
            var result = BLL.SysprepTag.GetSysprepTag(id);
            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }

        [GlobalAuth(Permission = "GlobalCreate")]
        public ActionResult Post(Models.SysprepTag sysprepTag)
        {
            var actionResult = BLL.SysprepTag.AddSysprepTag(sysprepTag);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }

        [GlobalAuth(Permission = "GlobalUpdate")]
        public Models.ActionResult Put(int id, Models.SysprepTag sysprepTag)
        {
            sysprepTag.Id = id;
            var actionResult = BLL.SysprepTag.UpdateSysprepTag(sysprepTag);
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
            apiBoolDto.Value = BLL.SysprepTag.DeleteSysprepTag(id);
           return apiBoolDto;
        }
    }
}