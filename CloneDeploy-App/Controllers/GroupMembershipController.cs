using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_App.DTOs;
using CloneDeploy_App.Models;

namespace CloneDeploy_App.Controllers
{
    

    public class GroupMembershipController : ApiController
    {    
        [GroupAuth]
        public ApiBoolDTO Post(List<Models.GroupMembership> groupMemberships)
        {
           var apiBoolDto = new ApiBoolDTO();

            apiBoolDto.Value = BLL.GroupMembership.AddMembership(groupMemberships);
            if (!apiBoolDto.Value)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound);
                throw new HttpResponseException(response);
            }
            return apiBoolDto;
        }
    }
}
