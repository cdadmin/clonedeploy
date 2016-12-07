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
        public ApiBoolResponseDTO CreateDefaultBootMenu(BootMenuGenOptionsDTO defaultMenuOptions)
        {
           new BLL.Workflows.DefaultBootMenu(defaultMenuOptions).CreateGlobalDefaultBootMenu();
            return new ApiBoolResponseDTO() {Value = true};
        }

        [UserAuth(Permission = "Administrator")]
        public ApiBoolResponseDTO GenerateLinuxIsoConfig(IsoGenOptionsDTO isoOptions)
        {
            new BLL.Workflows.IsoGen(isoOptions).Generate();
            return new ApiBoolResponseDTO() { Value = true };
        }

        [UserAuth(Permission = "Administrator")]
        public ApiBoolResponseDTO CreateClobberBootMenu(int profileId, bool promptComputerName)
        {
            new BLL.Workflows.ClobberBootMenu(profileId, promptComputerName).CreatePxeBootFiles();
            return new ApiBoolResponseDTO() { Value = true };
        }

        [UserAuth(Permission = "Administrator")]
        public ApiBoolResponseDTO CopyPxeBinaries()
        {
            return new ApiBoolResponseDTO() {Value = new BLL.Workflows.CopyPxeBinaries().CopyFiles()};
        }     
    }
}
