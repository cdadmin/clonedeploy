using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web.Http;
using CloneDeploy_ApiCalls;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_App.DTOs;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;


namespace CloneDeploy_App.Controllers
{
    
    public class GroupMembershipAPI: GenericAPI<GroupMembershipEntity>
    {
        public GroupMembershipAPI(string resource):base(resource)
        {
		
        }
    {

       
        public ActionResultDTO Post(List<GroupMembershipEntity> groupMemberships)
        {
            
          
        }
    }
}
