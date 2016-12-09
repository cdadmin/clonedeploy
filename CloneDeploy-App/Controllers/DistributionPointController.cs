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
    public class DistributionPointController: ApiController
    {
        private readonly DistributionPointServices _distributionPointServices;

        public DistributionPointController()
        {
            _distributionPointServices = new DistributionPointServices();
        }

        [AdminAuth(Permission = "AdminRead")]
        public IEnumerable<DistributionPointEntity> GetAll(string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? _distributionPointServices.SearchDistributionPoints()
                : _distributionPointServices.SearchDistributionPoints(searchstring);

        }

        [AdminAuth(Permission = "AdminRead")]
        public ApiStringResponseDTO GetCount()
        {

            return new ApiStringResponseDTO() {Value = _distributionPointServices.TotalCount()};

        }

        [AdminAuth(Permission = "AdminRead")]
        public DistributionPointEntity Get(int id)
        {
            var result = _distributionPointServices.GetDistributionPoint(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [AdminAuth(Permission = "AdminRead")]
        public DistributionPointEntity GetPrimary()
        {
            var result = _distributionPointServices.GetPrimaryDistributionPoint();
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [AdminAuth(Permission = "AdminUpdate")]
        public ActionResultDTO Post(DistributionPointEntity distributionPoint)
        {
            var result = _distributionPointServices.AddDistributionPoint(distributionPoint);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [AdminAuth(Permission = "AdminUpdate")]
        public ActionResultDTO Put(int id, DistributionPointEntity distributionPoint)
        {
            distributionPoint.Id = id;
            var result = _distributionPointServices.UpdateDistributionPoint(distributionPoint);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [AdminAuth(Permission = "AdminUpdate")]
        public ActionResultDTO Delete(int id)
        {
            var result = _distributionPointServices.DeleteDistributionPoint(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }
    }
}