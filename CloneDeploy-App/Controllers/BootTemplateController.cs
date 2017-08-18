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
    public class BootTemplateController : ApiController
    {
        private readonly BootTemplateServices _bootTemplateServices;

        public BootTemplateController()
        {
            _bootTemplateServices = new BootTemplateServices();
        }

        [CustomAuth(Permission = "GlobalDelete")]
        public ActionResultDTO Delete(int id)
        {
            var result = _bootTemplateServices.DeleteBootTemplate(id);
            if (result.Id == 0)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, result));
            return result;
        }

        [CustomAuth(Permission = "GlobalRead")]
        public BootTemplateEntity Get(int id)
        {
            var result = _bootTemplateServices.GetBootTemplate(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [CustomAuth(Permission = "GlobalRead")]
        public IEnumerable<BootTemplateEntity> Get(string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? _bootTemplateServices.SearchBootTemplates()
                : _bootTemplateServices.SearchBootTemplates(searchstring);
        }

        [CustomAuth(Permission = "GlobalRead")]
        public ApiStringResponseDTO GetCount()
        {
            return new ApiStringResponseDTO {Value = _bootTemplateServices.TotalCount()};
        }

        [CustomAuth(Permission = "GlobalCreate")]
        public ActionResultDTO Post(BootTemplateEntity bootTemplate)
        {
            return _bootTemplateServices.AddBootTemplate(bootTemplate);
        }

        [CustomAuth(Permission = "GlobalUpdate")]
        public ActionResultDTO Put(int id, BootTemplateEntity bootTemplate)
        {
            bootTemplate.Id = id;
            var result = _bootTemplateServices.UpdateBootTemplate(bootTemplate);
            if (result.Id == 0)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, result));
            return result;
        }
    }
}