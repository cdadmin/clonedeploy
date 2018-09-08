using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Common.Enum;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;

namespace CloneDeploy_App.Controllers
{
    public class ImageProfileTemplateController : ApiController
    {
        private readonly ImageProfileTemplateServices _imageProfileTemplateServices;

        public ImageProfileTemplateController()
        {
           _imageProfileTemplateServices = new ImageProfileTemplateServices();
        }

      
        [Authorize]
        public ImageProfileTemplate Get(EnumProfileTemplate.TemplateType templateType)
        {
            var result = _imageProfileTemplateServices.GetTemplate(templateType);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [CustomAuth(Permission = "Administrator")]
        public ActionResultDTO Put(ImageProfileTemplate template)
        {
            var result = _imageProfileTemplateServices.UpdateTemplate(template);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }
    }
}