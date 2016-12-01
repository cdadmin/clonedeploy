using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Http;
using CloneDeploy_App.BLL;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;


namespace CloneDeploy_App.Controllers
{
    public class ComputerLogController:ApiController
    {
        private readonly ComputerLogServices _computerLogServices;

        public ComputerLogController()
        {
            _computerLogServices = new ComputerLogServices();
        }

        [ComputerAuth(Permission = "ComputerRead")]
        public ComputerLogEntity Get(int id)
        {
            var result = _computerLogServices.GetComputerLog(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [ComputerAuth(Permission = "AdminRead")]
        public IEnumerable<ComputerLogEntity> GetOnDemandLogs(int limit = 0)
        {
            return _computerLogServices.SearchOnDemand(limit);
        }

        [ComputerAuth(Permission = "ComputerCreate")]
        public ActionResultDTO Post(ComputerLogEntity computerLog)
        {
            var result = _computerLogServices.AddComputerLog(computerLog);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [ComputerAuth(Permission = "ComputerDelete")]
        public ActionResultDTO Delete(int id)
        {
            var result = _computerLogServices.DeleteComputerLog(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

      
    }
}