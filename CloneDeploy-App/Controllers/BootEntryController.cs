using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_App.Models;

namespace CloneDeploy_App.Controllers
{
    public class BootEntryController: ApiController
    {
        [GlobalAuth(Permission = "GlobalRead")]
        public IEnumerable<Models.BootEntry> Get(string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? BLL.BootEntry.SearchBootEntrys()
                : BLL.BootEntry.SearchBootEntrys(searchstring);

        }

        [GlobalAuth(Permission = "GlobalRead")]
        public ApiDTO GetCount()
        {
            var ApiDTO = new ApiDTO();
            ApiDTO.Value = BLL.BootEntry.TotalCount();
            return ApiDTO;
        }

        [GlobalAuth(Permission = "GlobalRead")]
        public IHttpActionResult Get(int id)
        {
            var result = BLL.BootEntry.GetBootEntry(id);
            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }

        [GlobalAuth(Permission = "GlobalCreate")]
        public ActionResult Post(Models.BootEntry bootEntry)
        {
            var actionResult = BLL.BootEntry.AddBootEntry(bootEntry);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }

        [GlobalAuth(Permission = "GlobalUpdate")]
        public Models.ActionResult Put(int id, Models.BootEntry bootEntry)
        {
            bootEntry.Id = id;
            var actionResult = BLL.BootEntry.UpdateBootEntry(bootEntry);
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
            var actionResult = BLL.BootEntry.DeleteBootEntry(id);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }
    }
}