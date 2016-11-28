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
    public class SiteController: ApiController
    {
        [GlobalAuth(Permission = "GlobalRead")]
        public IEnumerable<Models.Site> Get(string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? BLL.Site.SearchSites()
                : BLL.Site.SearchSites(searchstring);

        }

        [GlobalAuth(Permission = "GlobalRead")]
        public ApiDTO GetCount()
        {
            var ApiDTO = new ApiDTO();
            ApiDTO.Value = BLL.Site.TotalCount();
            return ApiDTO;
        }

        [GlobalAuth(Permission = "GlobalRead")]
        public IHttpActionResult Get(int id)
        {
            var result = BLL.Site.GetSite(id);
            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }

        [GlobalAuth(Permission = "GlobalCreate")]
        public ActionResult Post(Models.Site site)
        {
            var actionResult = BLL.Site.AddSite(site);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }

        [GlobalAuth(Permission = "GlobalUpdate")]
        public Models.ActionResult Put(int id, Models.Site site)
        {
            site.Id = id;
            var actionResult = BLL.Site.UpdateSite(site);
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
            apiBoolDto.Value = BLL.Site.DeleteSite(id);
           return apiBoolDto;
        }
    }
}