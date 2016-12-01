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
    public class BootTemplateController: ApiController
    {
         private readonly BootTemplateServices _bootTemplateServices;

        public BootTemplateController()
        {
            _bootTemplateServices = new BootTemplateServices();
        }

        [GlobalAuth(Permission = "GlobalRead")]
        public IEnumerable<BootTemplateEntity> Get(string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? _bootTemplateServices.SearchBootTemplates()
                : _bootTemplateServices.SearchBootTemplates(searchstring);

        }

        [GlobalAuth(Permission = "GlobalRead")]
        public ApiStringResponseDTO GetCount()
        {
            return new ApiStringResponseDTO() {Value = _bootTemplateServices.TotalCount()};

        }

        [GlobalAuth(Permission = "GlobalRead")]
        public BootTemplateEntity Get(int id)
        {
            var result = _bootTemplateServices.GetBootTemplate(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [GlobalAuth(Permission = "GlobalCreate")]
        public ActionResultDTO Post(BootTemplateEntity bootTemplate)
        {
            return  _bootTemplateServices.AddBootTemplate(bootTemplate);
          
        }

        [GlobalAuth(Permission = "GlobalUpdate")]
        public ActionResultDTO Put(int id, BootTemplateEntity bootTemplate)
        {
            bootTemplate.Id = id;
            var result = _bootTemplateServices.UpdateBootTemplate(bootTemplate);
            if (result.Id == 0) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, result));
            return result;
        }

        [GlobalAuth(Permission = "GlobalDelete")]
        public ActionResultDTO Delete(int id)
        {
            var result = _bootTemplateServices.DeleteBootTemplate(id);
            if (result.Id == 0) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, result));
            return result;
        }
    }
}