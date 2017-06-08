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
    public class ActiveImagingTaskController : ApiController
    {
        private readonly ActiveImagingTaskServices _activeImagingTaskServices;

        public ActiveImagingTaskController()
        {
            _activeImagingTaskServices = new ActiveImagingTaskServices();
        }

        [CustomAuth(Permission = "ImageTaskDelete")]
        public ActionResultDTO Delete(int id)
        {
            var result = _activeImagingTaskServices.DeleteActiveImagingTask(id);
            if (result.Id == 0)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, result));
            return result;
        }

        [Authorize]
        public ActionResultDTO DeleteOnDemand(int id)
        {
            var result = _activeImagingTaskServices.DeleteUnregisteredOndTask(id);
            if (result.Id == 0)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, result));
            return result;
        }

        [Authorize]
        public ApiStringResponseDTO GetActiveNotOwned()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                .Select(c => c.Value).SingleOrDefault();

            return new ApiStringResponseDTO
            {
                Value = _activeImagingTaskServices.ActiveCountNotOwnedByuser(Convert.ToInt32(userId))
            };
        }

        [Authorize]
        public IEnumerable<TaskWithComputer> GetActiveTasks()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                .Select(c => c.Value).SingleOrDefault();
            return _activeImagingTaskServices.ReadAll(Convert.ToInt32(userId));
        }

        [Authorize]
        public ApiStringResponseDTO GetActiveUnicastCount(string taskType)
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                .Select(c => c.Value).SingleOrDefault();

            return new ApiStringResponseDTO
            {
                Value = _activeImagingTaskServices.ActiveUnicastCount(Convert.ToInt32(userId), taskType)
            };
        }


        [Authorize]
        public ApiStringResponseDTO GetAllActiveCount()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                .Select(c => c.Value).SingleOrDefault();

            return new ApiStringResponseDTO
            {
                Value = _activeImagingTaskServices.AllActiveCount(Convert.ToInt32(userId))
            };
        }

        [Authorize]
        public IEnumerable<TaskWithComputer> GetUnicasts()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                .Select(c => c.Value).SingleOrDefault();

            return _activeImagingTaskServices.ReadUnicasts(Convert.ToInt32(userId));
        }

        [Authorize]
        public IEnumerable<TaskWithComputer> GetPermanentUnicasts()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                .Select(c => c.Value).SingleOrDefault();

            return _activeImagingTaskServices.ReadUnicasts(Convert.ToInt32(userId));
        }

        [Authorize]
        public IEnumerable<ActiveImagingTaskEntity> GetAllOnDemandUnregistered()
        {
            return _activeImagingTaskServices.GetAllOnDemandUnregistered();
        }

        [Authorize]
        [HttpGet]
        public ApiIntResponseDTO OnDemandCount()
        {
            return new ApiIntResponseDTO { Value = _activeImagingTaskServices.OnDemandCount() };
        }
    }
}