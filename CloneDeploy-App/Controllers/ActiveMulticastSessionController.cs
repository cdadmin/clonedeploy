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
    public class ActiveMulticastSessionController : ApiController
    {
        private readonly ActiveMulticastSessionServices _activeMulticastSessionServices;
        private readonly int _userId;

        public ActiveMulticastSessionController()
        {
            _activeMulticastSessionServices = new ActiveMulticastSessionServices();
            _userId = Convert.ToInt32(((ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == "user_id")
                .Select(c => c.Value).SingleOrDefault());
        }

        [CustomAuth(Permission = "ImageTaskMulticast")]
        public ActionResultDTO Delete(int id)
        {
            var result = _activeMulticastSessionServices.Delete(id);
            if (result.Id == 0)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, result));
            return result;
        }


        [Authorize]
        public IEnumerable<ActiveMulticastSessionEntity> Get()
        {
            return _activeMulticastSessionServices.GetAllMulticastSessions(Convert.ToInt32(_userId));
        }

        [CustomAuth(Permission = "ImageTaskMulticast")]
        public IEnumerable<ComputerEntity> GetComputers(int id)
        {
            return new ActiveImagingTaskServices().GetMulticastComputers(id);
        }

        [Authorize]
        public ApiStringResponseDTO GetCount()
        {
            return new ApiStringResponseDTO
            {
                Value = _activeMulticastSessionServices.ActiveCount(Convert.ToInt32(_userId))
            };
        }

        [CustomAuth(Permission = "ImageTaskMulticast")]
        public IEnumerable<TaskWithComputer> GetMemberStatus(int id)
        {
            return new ActiveImagingTaskServices().MulticastMemberStatus(id);
        }

        [CustomAuth(Permission = "ImageTaskMulticast")]
        public IEnumerable<ActiveImagingTaskEntity> GetProgress(int id)
        {
            return new ActiveImagingTaskServices().MulticastProgress(id);
        }
    }
}