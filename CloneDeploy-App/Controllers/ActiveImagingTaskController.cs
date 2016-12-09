using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;


namespace CloneDeploy_App.Controllers
{
    public class ActiveImagingTaskController: ApiController
    {
        private readonly ActiveImagingTaskServices _activeImagingTaskServices;

        public ActiveImagingTaskController()
        {
            _activeImagingTaskServices = new ActiveImagingTaskServices();
        }

        [ComputerAuth(Permission = "ComputerSearch")]
        public IEnumerable<ActiveImagingTaskEntity> GetUnicasts(string taskType)
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();

            return _activeImagingTaskServices.ReadUnicasts(Convert.ToInt32(userId), taskType);

        }

        [TaskAuth(Permission = "ImageTaskDelete")]
        public ActionResultDTO Delete(int id)
        {
            var result = _activeImagingTaskServices.DeleteActiveImagingTask(id);
            if (result.Id == 0) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, result));
            return result;
        }

        [Authorize]
        public IEnumerable<ActiveImagingTaskEntity> GetActiveTasks()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();
            return _activeImagingTaskServices.ReadAll(Convert.ToInt32(userId));
        }

        [TaskAuth(Permission = "ImageTaskDeploy")]
        public ApiStringResponseDTO GetActiveUnicastCount(string taskType)
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();

            return new ApiStringResponseDTO()
            {
                Value = _activeImagingTaskServices.ActiveUnicastCount(Convert.ToInt32(userId), taskType)
            };

        }

        [TaskAuth(Permission = "ImageTaskDeploy")]
        public ApiStringResponseDTO GetAllActiveCount()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();

            return new ApiStringResponseDTO()
            {
                Value = _activeImagingTaskServices.AllActiveCount(Convert.ToInt32(userId))
            };

        }

     
    }
}