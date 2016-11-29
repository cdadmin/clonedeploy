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
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;


namespace CloneDeploy_App.Controllers
{
    public class ActiveMulticastSessionController: ApiController
    {
        [TaskAuth(Permission = "ImageTaskMulticast")]
        public IHttpActionResult GetAll()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();

            var sessions = BLL.ActiveMulticastSession.GetAllMulticastSessions(Convert.ToInt32(userId));
            if (sessions == null)
                return NotFound();
            else
                return Ok(sessions);
        }

        [TaskAuth(Permission = "ImageTaskMulticast")]
        public ApiDTO GetCount()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();
            var apiDTO = new ApiDTO();
            apiDTO.Value = BLL.ActiveMulticastSession.ActiveCount(Convert.ToInt32(userId));
            return apiDTO;
        }

        [TaskAuth(Permission = "ImageTaskMulticast")]
        public IHttpActionResult GetMemberStatus(int id)
        {
            var memberStatus = BLL.ActiveImagingTask.MulticastMemberStatus(id);
            if (memberStatus == null)
                return NotFound();
            else
                return Ok(memberStatus);
        }

        [TaskAuth(Permission = "ImageTaskMulticast")]
        public IHttpActionResult GetComputers(int id)
        {
            var computers = BLL.ActiveImagingTask.GetMulticastComputers(id);
            if (computers == null)
                return NotFound();
            else
                return Ok(computers);
        }

        [TaskAuth(Permission = "ImageTaskMulticast")]
        public IHttpActionResult GetProgress(int id)
        {
            var progress = BLL.ActiveImagingTask.MulticastProgress(id);
            if (progress == null)
                return NotFound();
            else
                return Ok(progress);
        }

        [TaskAuth(Permission = "ImageTaskMulticast")]
        public ActionResultEntity Delete(int id)
        {
            var actionResult = BLL.ActiveMulticastSession.Delete(id);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }
    }
}