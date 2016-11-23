using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_App.Models;

namespace CloneDeploy_App.Controllers
{
    public class ComputerLogController:ApiController
    {
        [ComputerAuth(Permission = "ComputerRead")]
        public IEnumerable<Models.ComputerLog> GetComputerLogs(int id)
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
        public IEnumerable<Models.ComputerLog> GetOnDemandLogs(int limit=0)
        {
            return BLL.ComputerLog.SearchOnDemand(limit);
        }

        [ComputerAuth(Permission = "ComputerCreate")]
        public ActionResult Post(Models.ComputerLog computerLog)
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
        public Models.ActionResult Delete(int id)
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
        public Models.ActionResult DeleteAllComputerLogs(int id)
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