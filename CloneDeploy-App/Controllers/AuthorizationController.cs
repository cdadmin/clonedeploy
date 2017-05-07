using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web.Http;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;

namespace CloneDeploy_App.Controllers
{
    public class AuthorizationController : ApiController
    {
        [Authorize]
        [HttpGet]
        public ApiBoolResponseDTO ComputerManagement(string requiredRight, int computerId)
        {
            var identity = (ClaimsPrincipal) Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                .Select(c => c.Value).SingleOrDefault();
            return new ApiBoolResponseDTO
            {
                Value = new AuthorizationServices(Convert.ToInt32(userId), requiredRight).ComputerManagement(computerId)
            };
        }

        [Authorize]
        [HttpGet]
        public ApiBoolResponseDTO GroupManagement(string requiredRight, int groupId)
        {
            var identity = (ClaimsPrincipal) Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                .Select(c => c.Value).SingleOrDefault();
            return new ApiBoolResponseDTO
            {
                Value = new AuthorizationServices(Convert.ToInt32(userId), requiredRight).GroupManagement(groupId)
            };
        }

        [Authorize]
        [HttpGet]
        public ApiBoolResponseDTO ImageManagement(string requiredRight, int imageId)
        {
            var identity = (ClaimsPrincipal) Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                .Select(c => c.Value).SingleOrDefault();
            return new ApiBoolResponseDTO
            {
                Value = new AuthorizationServices(Convert.ToInt32(userId), requiredRight).ImageManagement(imageId)
            };
        }

        [Authorize]
        [HttpGet]
        public ApiBoolResponseDTO IsAuthorized(string requiredRight)
        {
            var identity = (ClaimsPrincipal) Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                .Select(c => c.Value).SingleOrDefault();
            return new ApiBoolResponseDTO
            {
                Value = new AuthorizationServices(Convert.ToInt32(userId), requiredRight).IsAuthorized()
            };
        }
    }
}