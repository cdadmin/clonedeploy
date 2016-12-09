using System.Net;
using System.Net.Http;
using System.Web.Http;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;


namespace CloneDeploy_App.Controllers
{
    public class CdVersionController: ApiController
    {
         private readonly CdVersionServices _cdVersionServices;

        public CdVersionController()
        {
            _cdVersionServices = new CdVersionServices();
        }

        [HttpGet]
        [Authorize]
        public ApiBoolResponseDTO IsFirstRunCompleted()
        {
            return new ApiBoolResponseDTO() {Value = _cdVersionServices.FirstRunCompleted()};
        }

        [Authorize]
        public CdVersionEntity Get(int id)
        {
            var result = _cdVersionServices.Get(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

         [Authorize]
        public ActionResultDTO Post(CdVersionEntity cdVersion)
        {
            return _cdVersionServices.Update(cdVersion);

        }

       
    }
}