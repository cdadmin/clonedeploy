using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
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
    public class ComputerLogAPI:GenericAPI<ComputerLogEntity>
    {
        public ComputerLogAPI(string resource):base(resource)
        {
		
        }

        [ComputerAuth(Permission = "AdminRead")]
        public IEnumerable<ComputerLogEntity> GetOnDemandLogs(int limit = 0)
        {
            return _computerLogServices.SearchOnDemand(limit);
        }

      

      
    }
}