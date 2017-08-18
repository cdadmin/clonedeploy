using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;
using CloneDeploy_Common;

namespace CloneDeploy_App.Controllers
{
    public class AlternateServerIpController : ApiController
    {
        private readonly AlternateServerIpServices _alternateServerIpServices;

        public AlternateServerIpController()
        {
            _alternateServerIpServices = new AlternateServerIpServices();
        }

        [CustomAuth(Permission = AuthorizationStrings.UpdateAdmin)]
        public ActionResultDTO Delete(int id)
        {
            var result = _alternateServerIpServices.DeleteAlternateServerIp(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [CustomAuth(Permission = AuthorizationStrings.ReadAdmin)]
        public AlternateServerIpEntity Get(int id)
        {
            var result = _alternateServerIpServices.GetAlternateServerIp(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [CustomAuth(Permission = AuthorizationStrings.ReadAdmin)]
        public IEnumerable<AlternateServerIpEntity> Get()
        {
            return _alternateServerIpServices.GetAll();
           
        }

        [CustomAuth(Permission = AuthorizationStrings.ReadAdmin)]
        public ApiStringResponseDTO GetCount()
        {
            return new ApiStringResponseDTO {Value = _alternateServerIpServices.TotalCount()};
        }

        [CustomAuth(Permission = AuthorizationStrings.UpdateAdmin)]
        public ActionResultDTO Post(AlternateServerIpEntity alternateServerIp)
        {
            return _alternateServerIpServices.AddAlternateServerIp(alternateServerIp);
        }

        [CustomAuth(Permission = AuthorizationStrings.UpdateAdmin)]
        public ActionResultDTO Put(int id, AlternateServerIpEntity alternateServerIp)
        {
            alternateServerIp.Id = id;
            var result = _alternateServerIpServices.UpdateAlternateServerIp(alternateServerIp);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }
    }
}