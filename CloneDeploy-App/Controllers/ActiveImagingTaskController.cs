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
        private readonly int _userId;

        public ActiveImagingTaskController()
        {
            _activeImagingTaskServices = new ActiveImagingTaskServices();
            _userId = Convert.ToInt32(((ClaimsIdentity) User.Identity).Claims.Where(c => c.Type == "user_id")
                .Select(c => c.Value).SingleOrDefault());
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
            return new ApiStringResponseDTO
            {
                Value = _activeImagingTaskServices.ActiveCountNotOwnedByuser(Convert.ToInt32(_userId))
            };
        }

        [Authorize]
        public IEnumerable<TaskWithComputer> GetActiveTasks()
        {
            return _activeImagingTaskServices.ReadAll(Convert.ToInt32(_userId));
        }

        [Authorize]
        public ApiStringResponseDTO GetActiveUnicastCount(string taskType)
        {
            return new ApiStringResponseDTO
            {
                Value = _activeImagingTaskServices.ActiveUnicastCount(Convert.ToInt32(_userId), taskType)
            };
        }

        [Authorize]
        public ApiStringResponseDTO GetAllActiveCount()
        {
            return new ApiStringResponseDTO
            {
                Value = _activeImagingTaskServices.AllActiveCount(Convert.ToInt32(_userId))
            };
        }

        [Authorize]
        public IEnumerable<ActiveImagingTaskEntity> GetAllOnDemandUnregistered()
        {
            return _activeImagingTaskServices.GetAllOnDemandUnregistered();
        }

        [Authorize]
        public IEnumerable<TaskWithComputer> GetPermanentUnicasts()
        {
            var identity = (ClaimsPrincipal) Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                .Select(c => c.Value).SingleOrDefault();

            return _activeImagingTaskServices.ReadUnicasts(Convert.ToInt32(userId));
        }

        [Authorize]
        public IEnumerable<TaskWithComputer> GetUnicasts()
        {
            return _activeImagingTaskServices.ReadUnicasts(Convert.ToInt32(_userId));
        }

        [Authorize]
        [HttpGet]
        public ApiIntResponseDTO OnDemandCount()
        {
            return new ApiIntResponseDTO {Value = _activeImagingTaskServices.OnDemandCount()};
        }
    }
}