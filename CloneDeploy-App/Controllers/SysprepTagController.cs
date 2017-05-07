using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;

namespace CloneDeploy_App.Controllers
{
    public class SysprepTagController : ApiController
    {
        private readonly SysprepTagServices _sysprepTagServices;

        public SysprepTagController()
        {
            _sysprepTagServices = new SysprepTagServices();
        }

        [CustomAuth(Permission = "GlobalDelete")]
        public ActionResultDTO Delete(int id)
        {
            var result = _sysprepTagServices.DeleteSysprepTag(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [CustomAuth(Permission = "GlobalRead")]
        public SysprepTagEntity Get(int id)
        {
            var result = _sysprepTagServices.GetSysprepTag(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [CustomAuth(Permission = "GlobalRead")]
        public IEnumerable<SysprepTagEntity> GetAll(string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? _sysprepTagServices.SearchSysprepTags()
                : _sysprepTagServices.SearchSysprepTags(searchstring);
        }

        [CustomAuth(Permission = "GlobalRead")]
        public ApiStringResponseDTO GetCount()
        {
            return new ApiStringResponseDTO {Value = _sysprepTagServices.TotalCount()};
        }

        [CustomAuth(Permission = "GlobalCreate")]
        public ActionResultDTO Post(SysprepTagEntity sysprepTag)
        {
            return _sysprepTagServices.AddSysprepTag(sysprepTag);
        }

        [CustomAuth(Permission = "GlobalUpdate")]
        public ActionResultDTO Put(int id, SysprepTagEntity sysprepTag)
        {
            sysprepTag.Id = id;
            var result = _sysprepTagServices.UpdateSysprepTag(sysprepTag);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }
    }
}