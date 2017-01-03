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
    public class SecondaryServerController: ApiController
    {
        private readonly SecondaryServerServices _secondaryServerServices;

        public SecondaryServerController()
        {
            _secondaryServerServices = new SecondaryServerServices();
        }

        [CustomAuth(Permission = "AdminRead")]
        public IEnumerable<SecondaryServerEntity> GetAll(string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? _secondaryServerServices.SearchSecondaryServers()
                : _secondaryServerServices.SearchSecondaryServers(searchstring);

        }

        [CustomAuth(Permission = "AdminRead")]
        public SecondaryServerEntity Get(int id)
        {
            var result = _secondaryServerServices.GetSecondaryServer(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [CustomAuth(Permission = "AdminUpdate")]
        public ActionResultDTO Post(SecondaryServerEntity secondaryServer)
        {
            return _secondaryServerServices.AddSecondaryServer(secondaryServer);
        }

        [CustomAuth(Permission = "AdminUpdate")]
        public ActionResultDTO Put(int id, SecondaryServerEntity secondaryServer)
        {
            secondaryServer.Id = id;
            var result = _secondaryServerServices.UpdateSecondaryServer(secondaryServer);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [CustomAuth(Permission = "AdminUpdate")]
        public ActionResultDTO Delete(int id)
        {

            var result = _secondaryServerServices.DeleteSecondaryServer(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }
    }
}