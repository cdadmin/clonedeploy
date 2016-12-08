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
    
    public class WorkflowController : ApiController
    {
        [UserAuth(Permission = "Administrator")]
        [HttpPost]
        public ApiBoolResponseDTO CreateDefaultBootMenu(BootMenuGenOptionsDTO defaultMenuOptions)
        {
           new BLL.Workflows.DefaultBootMenu(defaultMenuOptions).CreateGlobalDefaultBootMenu();
            return new ApiBoolResponseDTO() {Value = true};
        }

        [UserAuth(Permission = "Administrator")]
        [HttpPost]
        public ApiBoolResponseDTO GenerateLinuxIsoConfig(IsoGenOptionsDTO isoOptions)
        {
            new BLL.Workflows.IsoGen(isoOptions).Generate();
            return new ApiBoolResponseDTO() { Value = true };
        }

        [UserAuth(Permission = "Administrator")]
        [HttpGet]
        public ApiBoolResponseDTO CreateClobberBootMenu(int profileId, bool promptComputerName)
        {
            new BLL.Workflows.ClobberBootMenu(profileId, promptComputerName).CreatePxeBootFiles();
            return new ApiBoolResponseDTO() { Value = true };
        }

        [UserAuth(Permission = "Administrator")]
        [HttpGet]
        public ApiBoolResponseDTO CopyPxeBinaries()
        {
            return new ApiBoolResponseDTO() {Value = new BLL.Workflows.CopyPxeBinaries().CopyFiles()};
        }

        [UserAuth(Permission = "Administrator")]
        [HttpGet]
        public ApiBoolResponseDTO CancelAllImagingTasks()
        {
            return new ApiBoolResponseDTO() { Value = BLL.Workflows.CancelAllImagingTasks.Run() };
        }

        [UserAuth(Permission = "Administrator")]
        [HttpGet]
        public ApiStringResponseDTO StartOnDemandMulticast(int profileId, string clientCount)
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();

            return new ApiStringResponseDTO() { Value = new BLL.Workflows.Multicast(profileId, clientCount, Convert.ToInt32(userId)).Create() };
        }   

       
    }
}
