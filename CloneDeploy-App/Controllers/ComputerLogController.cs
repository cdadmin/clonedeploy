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
    public class ComputerLogController:ApiController
    {
        private readonly ComputerLogServices _computerLogServices;

        public ComputerLogController()
        {
            _computerLogServices = new ComputerLogServices();
        }

        [CustomAuth(Permission = "ComputerRead")]
        public ComputerLogEntity Get(int id)
        {
            var result = _computerLogServices.GetComputerLog(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [CustomAuth(Permission = "AdminRead")]
        public IEnumerable<ComputerLogEntity> GetOnDemandLogs(int limit = 0)
        {
            return _computerLogServices.SearchOnDemand(limit);
        }

        [CustomAuth(Permission = "ComputerCreate")]
        public ActionResultDTO Post(ComputerLogEntity computerLog)
        {
            var result = _computerLogServices.AddComputerLog(computerLog);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [CustomAuth(Permission = "ComputerDelete")]
        public ActionResultDTO Delete(int id)
        {
            var result = _computerLogServices.DeleteComputerLog(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

      
    }
}