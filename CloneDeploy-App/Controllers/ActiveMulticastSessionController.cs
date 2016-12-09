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
    public class ActiveMulticastSessionController: ApiController
    {
        private readonly ActiveMulticastSessionServices _activeMulticastSessionServices;


        public ActiveMulticastSessionController()
        {
            _activeMulticastSessionServices = new ActiveMulticastSessionServices();
        }

        [TaskAuth(Permission = "ImageTaskMulticast")]
        public IEnumerable<ActiveMulticastSessionEntity> GetAll()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();

            return _activeMulticastSessionServices.GetAllMulticastSessions(Convert.ToInt32(userId));
          
        }

        [TaskAuth(Permission = "ImageTaskMulticast")]
        public ApiStringResponseDTO GetCount()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();

            return new ApiStringResponseDTO()
            {
                Value = _activeMulticastSessionServices.ActiveCount(Convert.ToInt32(userId))
            };

        }

        [TaskAuth(Permission = "ImageTaskMulticast")]
        public IEnumerable<ActiveImagingTaskEntity> GetMemberStatus(int id)
        {
            return new ActiveImagingTaskServices().MulticastMemberStatus(id);       
        }

        [TaskAuth(Permission = "ImageTaskMulticast")]
        public IEnumerable<ComputerEntity> GetComputers(int id)
        {
            return new ActiveImagingTaskServices().GetMulticastComputers(id);
        }

        [TaskAuth(Permission = "ImageTaskMulticast")]
        public IEnumerable<ActiveImagingTaskEntity> GetProgress(int id)
        {
            return new ActiveImagingTaskServices().MulticastProgress(id);
           
        }

        [TaskAuth(Permission = "ImageTaskMulticast")]
        public ActionResultDTO Delete(int id)
        {
            var result = _activeMulticastSessionServices.Delete(id);
            if (result.Id == 0) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, result));
            return result;
        }
    }
}