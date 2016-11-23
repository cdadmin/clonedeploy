using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_App.Models;

namespace CloneDeploy_App.Controllers
{
    public class DistributionPointController: ApiController
    {
        [AdminAuth(Permission = "AdminRead")]
        public IEnumerable<Models.DistributionPoint> Get(string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? BLL.DistributionPoint.SearchDistributionPoints()
                : BLL.DistributionPoint.SearchDistributionPoints(searchstring);

        }

        [AdminAuth(Permission = "AdminRead")]
        public ApiDTO GetCount()
        {
            var ApiDTO = new ApiDTO();
            ApiDTO.Value = BLL.DistributionPoint.TotalCount();
            return ApiDTO;
        }

        [AdminAuth(Permission = "AdminRead")]
        public IHttpActionResult Get(int id)
        {
            var result = BLL.DistributionPoint.GetDistributionPoint(id);
            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }

        [AdminAuth(Permission = "AdminRead")]
        public IHttpActionResult GetPrimary()
        {
            var result = BLL.DistributionPoint.GetPrimaryDistributionPoint();
            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }

        [AdminAuth(Permission = "AdminUpdate")]
        public ActionResult Post(Models.DistributionPoint distributionPoint)
        {
            var actionResult = BLL.DistributionPoint.AddDistributionPoint(distributionPoint);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }

        [AdminAuth(Permission = "AdminUpdate")]
        public Models.ActionResult Put(int id, Models.DistributionPoint distributionPoint)
        {
            distributionPoint.Id = id;
            var actionResult = BLL.DistributionPoint.UpdateDistributionPoint(distributionPoint);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }

        [AdminAuth(Permission = "AdminUpdate")]
        public Models.ActionResult Delete(int id)
        {
            var actionResult = BLL.DistributionPoint.DeleteDistributionPoint(id);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }
    }
}