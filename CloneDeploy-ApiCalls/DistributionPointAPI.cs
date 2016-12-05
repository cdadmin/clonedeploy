using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using CloneDeploy_ApiCalls;
using CloneDeploy_App.BLL;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;


namespace CloneDeploy_App.Controllers
{
    public class DistributionPointAPI:GenericAPI<DistributionPointEntity>
    {
        public DistributionPointAPI(string resource):base(resource)
        {
		
        }
    
       

        [AdminAuth(Permission = "AdminRead")]
        public DistributionPointEntity GetPrimary()
        {
            var result = _distributionPointServices.GetPrimaryDistributionPoint();
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

       
    }
}