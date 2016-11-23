using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_App.Models;

namespace CloneDeploy_App.Controllers
{
    public class MunkiManifestTemplateController: ApiController
    {
        [GlobalAuth(Permission = "GlobalRead")]
        public IEnumerable<Models.MunkiManifestTemplate> Get(string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? BLL.MunkiManifestTemplate.SearchManifests()
                : BLL.MunkiManifestTemplate.SearchManifests(searchstring);

        }

        [GlobalAuth(Permission = "GlobalRead")]
        public ApiDTO GetCount()
        {
            var ApiDTO = new ApiDTO();
            ApiDTO.Value = BLL.MunkiManifestTemplate.TotalCount();
            return ApiDTO;
        }

        [GlobalAuth(Permission = "GlobalRead")]
        public IHttpActionResult Get(int id)
        {
            var manifest = BLL.MunkiManifestTemplate.GetManifest(id);
            if (manifest == null)
                return NotFound();
            else
                return Ok(manifest);
        }

        [GlobalAuth(Permission = "GlobalCreate")]
        public ActionResult Post(Models.MunkiManifestTemplate manifest)
        {
            var actionResult = BLL.MunkiManifestTemplate.AddManifest(manifest);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }

        [GlobalAuth(Permission = "GlobalUpdate")]
        public Models.ActionResult Put(int id, Models.MunkiManifestTemplate manifest)
        {
            manifest.Id = id;
            var actionResult = BLL.MunkiManifestTemplate.UpdateManifest(manifest);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }

        [GlobalAuth(Permission = "GlobalDelete")]
        public Models.ActionResult Delete(int id)
        {
            var actionResult = BLL.MunkiManifestTemplate.DeleteManifest(id);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }
    }
}