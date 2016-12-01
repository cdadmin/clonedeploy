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
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;


namespace CloneDeploy_App.Controllers
{
    
    public class GroupMembershipController : ApiController
    {

        private readonly GroupMembershipServices _groupMembershipServices;

        public GroupMembershipController()
        {
            _groupMembershipServices = new GroupMembershipServices();
        }

        [GroupAuth]
        public ActionResultDTO Post(List<GroupMembershipEntity> groupMemberships)
        {
            return  _groupMembershipServices.AddMembership(groupMemberships);
          
        }
    }
}
