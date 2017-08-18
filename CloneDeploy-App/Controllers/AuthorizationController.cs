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
        private readonly int _userId;
        public AuthorizationController()
        {
            _userId = Convert.ToInt32(((ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == "user_id")
                .Select(c => c.Value).SingleOrDefault());
        }

        [Authorize]
        [HttpGet]
        public ApiBoolResponseDTO ComputerManagement(string requiredRight, int computerId)
        {
            return new ApiBoolResponseDTO
            {
                Value = new AuthorizationServices(Convert.ToInt32(_userId), requiredRight).ComputerManagement(computerId)
            };
        }

        [Authorize]
        [HttpGet]
        public ApiBoolResponseDTO GroupManagement(string requiredRight, int groupId)
        {
            return new ApiBoolResponseDTO
            {
                Value = new AuthorizationServices(Convert.ToInt32(_userId), requiredRight).GroupManagement(groupId)
            };
        }

        [Authorize]
        [HttpGet]
        public ApiBoolResponseDTO ImageManagement(string requiredRight, int imageId)
        {
            return new ApiBoolResponseDTO
            {
                Value = new AuthorizationServices(Convert.ToInt32(_userId), requiredRight).ImageManagement(imageId)
            };
        }

        [Authorize]
        [HttpGet]
        public ApiBoolResponseDTO IsAuthorized(string requiredRight)
        {
            return new ApiBoolResponseDTO
            {
                Value = new AuthorizationServices(Convert.ToInt32(_userId), requiredRight).IsAuthorized()
            };
        }
    }
}