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
    public class ActiveImagingTaskController: ApiController
    {
        [ComputerAuth(Permission = "ComputerSearch")]
        public IEnumerable<ActiveImagingTaskEntity> GetUnicasts(string taskType)
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();

            return BLL.ActiveImagingTask.ReadUnicasts(Convert.ToInt32(userId), taskType);

        }

        [TaskAuth(Permission = "ImageTaskDelete")]
        public ActionResultEntity Delete(int id)
        {
            var actionResult = BLL.ActiveImagingTask.DeleteActiveImagingTask(id);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }

        [TaskAuth(Permission = "ImageTaskDeploy")]
        public IHttpActionResult GetActiveTasks()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();
            var tasks = BLL.ActiveImagingTask.ReadAll(Convert.ToInt32(userId));
            if (tasks == null)
                return NotFound();
            else
                return Ok(tasks);
        }

        [TaskAuth(Permission = "ImageTaskDeploy")]
        public ApiDTO GetActiveUnicastCount(string taskType)
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();
            var apiDto = new ApiDTO();
            apiDto.Value = BLL.ActiveImagingTask.ActiveUnicastCount(Convert.ToInt32(userId), taskType);
            return apiDto;
        }

        [TaskAuth(Permission = "ImageTaskDeploy")]
        public ApiDTO GetAllActiveCount()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();
            var apiDto = new ApiDTO();
            apiDto.Value = BLL.ActiveImagingTask.AllActiveCount(Convert.ToInt32(userId));
            return apiDto;
        }

     
    }
}