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
using CloneDeploy_Services;


namespace CloneDeploy_App.Controllers
{
    public class BootEntryController: ApiController
    {
         private readonly BootEntryServices _bootEntryServices;

        public BootEntryController()
        {
            _bootEntryServices = new BootEntryServices();
        }

        [GlobalAuth(Permission = "GlobalRead")]
        public IEnumerable<BootEntryEntity> Get(string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? _bootEntryServices.SearchBootEntrys()
                : _bootEntryServices.SearchBootEntrys(searchstring);

        }

        [GlobalAuth(Permission = "GlobalRead")]
        public ApiStringResponseDTO GetCount()
        {

            return new ApiStringResponseDTO() {Value = _bootEntryServices.TotalCount()};

        }

        [GlobalAuth(Permission = "GlobalRead")]
        public BootEntryEntity Get(int id)
        {
            var result = _bootEntryServices.GetBootEntry(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [GlobalAuth(Permission = "GlobalCreate")]
        public ActionResultEntity Post(BootEntryEntity bootEntry)
        {
            var actionResult = BLL.BootEntryServices.AddBootEntry(bootEntry);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }

        [GlobalAuth(Permission = "GlobalUpdate")]
        public ActionResultEntity Put(int id, BootEntryEntity bootEntry)
        {
            bootEntry.Id = id;
            var actionResult = BLL.BootEntryServices.UpdateBootEntry(bootEntry);
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
            var actionResult = BLL.BootEntryServices.DeleteBootEntry(id);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }
    }
}