using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Http;
using CloneDeploy_ApiCalls;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;


namespace CloneDeploy_App.Controllers
{
    public class ActiveMulticastSessionAPI:GenericAPI<ActiveMulticastSessionEntity>
    {
        public ActiveMulticastSessionAPI(string resource):base(resource)
        {
		
        }



        public IEnumerable<ActiveImagingTaskEntity> GetMemberStatus(int id)
        {
           
        }

        public IEnumerable<ComputerEntity> GetComputers(int id)
        {
          
        }


        public IEnumerable<ActiveImagingTaskEntity> GetProgress(int id)
        {
           
           
        }

     
    }
}