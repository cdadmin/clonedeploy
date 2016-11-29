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


namespace CloneDeploy_App.Controllers
{
    public class ComputerLogController:ApiController
    {
        [ComputerAuth(Permission = "ComputerRead")]
        public IEnumerable<ComputerLogEntity> GetComputerLogs(int id)
        {       
            return BLL.ComputerLog.Search(id);
        }

        [ComputerAuth(Permission = "ComputerRead")]
        public IHttpActionResult Get(int id)
        {
            var log = BLL.ComputerLog.GetComputerLog(id);
            if (log == null)
                return NotFound();
            else
                return Ok(log);
        }

        [ComputerAuth(Permission = "AdminRead")]
        public IEnumerable<ComputerLogEntity> GetOnDemandLogs(int limit = 0)
        {
            return BLL.ComputerLog.SearchOnDemand(limit);
        }

        [ComputerAuth(Permission = "ComputerCreate")]
        public ActionResultEntity Post(ComputerLogEntity computerLog)
        {
            var actionResult = BLL.ComputerLog.AddComputerLog(computerLog);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }

        [ComputerAuth(Permission = "ComputerDelete")]
        public ActionResultEntity Delete(int id)
        {
            var actionResult = BLL.ComputerLog.DeleteComputerLog(id);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }

        [ComputerAuth(Permission = "ComputerDelete")]
        public ActionResultEntity DeleteAllComputerLogs(int id)
        {
            var actionResult = BLL.ComputerLog.DeleteComputerLogs(id);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }
    }
}