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
        public ActionResultDTO Post(BootEntryEntity bootEntry)
        {
            return _bootEntryServices.AddBootEntry(bootEntry);
        }

        [GlobalAuth(Permission = "GlobalUpdate")]
        public ActionResultDTO Put(int id, BootEntryEntity bootEntry)
        {
            bootEntry.Id = id;
            var result = _bootEntryServices.UpdateBootEntry(bootEntry);
            if (result.Id == 0) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, result));
            return result;
        }

        [GlobalAuth(Permission = "GlobalDelete")]
        public ActionResultDTO Delete(int id)
        {
            var result = _bootEntryServices.DeleteBootEntry(id);
            if (result.Id == 0) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, result));
            return result;
        }
    }
}