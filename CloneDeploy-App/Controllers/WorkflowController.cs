using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services.Workflows;


namespace CloneDeploy_App.Controllers
{
    
    public class WorkflowController : ApiController
    {
        [CustomAuth(Permission = "AdminUpdate")]
        [HttpPost]
        public ApiBoolResponseDTO CreateDefaultBootMenu(BootMenuGenOptionsDTO defaultMenuOptions)
        {
           new DefaultBootMenu(defaultMenuOptions).CreateGlobalDefaultBootMenu();
            return new ApiBoolResponseDTO() {Value = true};
        }

        [CustomAuth(Permission = "AdminUpdate")]
        [HttpPost]
        public ApiBoolResponseDTO GenerateLinuxIsoConfig(IsoGenOptionsDTO isoOptions)
        {
            new IsoGen(isoOptions).Generate();
            return new ApiBoolResponseDTO() { Value = true };
        }

        [CustomAuth(Permission = "AdminUpdate")]
        [HttpGet]
        public ApiBoolResponseDTO CreateClobberBootMenu(int profileId, bool promptComputerName)
        {
            new ClobberBootMenu(profileId, promptComputerName).CreatePxeBootFiles();
            return new ApiBoolResponseDTO() { Value = true };
        }

        [CustomAuth(Permission = "AdminUpdate")]
        [HttpGet]
        public ApiBoolResponseDTO CopyPxeBinaries()
        {
            return new ApiBoolResponseDTO() {Value = new CopyPxeBinaries().CopyFiles()};
        }

        [CustomAuth(Permission = "Administrator")]
        [HttpGet]
        public ApiBoolResponseDTO CancelAllImagingTasks()
        {
            return new ApiBoolResponseDTO() { Value = CloneDeploy_Services.Workflows.CancelAllImagingTasks.Run() };
        }

        [CustomAuth(Permission = "AllowOnd")]
        [HttpGet]
        public ApiStringResponseDTO StartOnDemandMulticast(int profileId, string clientCount)
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();

            return new ApiStringResponseDTO() { Value = new Multicast(profileId, clientCount, Convert.ToInt32(userId)).Create() };
        }   

       
    }
}
