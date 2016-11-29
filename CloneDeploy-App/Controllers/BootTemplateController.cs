using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using CloneDeploy_App.BLL;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;


namespace CloneDeploy_App.Controllers
{
    public class BootTemplateController: ApiController
    {
        [GlobalAuth(Permission = "GlobalRead")]
        public IEnumerable<BootTemplateEntity> Get(string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? BLL.BootTemplate.SearchBootTemplates()
                : BLL.BootTemplate.SearchBootTemplates(searchstring);

        }

        [GlobalAuth(Permission = "GlobalRead")]
        public ApiDTO GetCount()
        {
            var ApiDTO = new ApiDTO();
            ApiDTO.Value = BLL.BootTemplate.TotalCount();
            return ApiDTO;
        }

        [GlobalAuth(Permission = "GlobalRead")]
        public IHttpActionResult Get(int id)
        {
            var result = BLL.BootTemplate.GetBootTemplate(id);
            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }

        [GlobalAuth(Permission = "GlobalCreate")]
        public ActionResultEntity Post(BootTemplateEntity bootTemplate)
        {
            var actionResult = BLL.BootTemplate.AddBootTemplate(bootTemplate);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }

        [GlobalAuth(Permission = "GlobalUpdate")]
        public ActionResultEntity Put(int id, BootTemplateEntity bootTemplate)
        {
            bootTemplate.Id = id;
            var actionResult = BLL.BootTemplate.UpdateBootTemplate(bootTemplate);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }

        [GlobalAuth(Permission = "GlobalDelete")]
        public ActionResultEntity Delete(int id)
        {
            var actionResult = BLL.BootTemplate.DeleteBootTemplate(id);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }
    }
}