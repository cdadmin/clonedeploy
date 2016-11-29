using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_App.DTOs;
using CloneDeploy_Entities;


namespace CloneDeploy_App.Controllers
{
    public class MunkiManagedInstallController: ApiController
    {
        [GlobalAuth(Permission = "GlobalRead")]
        public MunkiManifestManagedInstallEntity Get(int id)
        {

            return BLL.MunkiManagedInstall.GetManagedInstall(id);

        }

        [GlobalAuth(Permission = "GlobalCreate")]
        public ApiBoolDTO Post(MunkiManifestManagedInstallEntity managedInstall)
        {
            var apiBoolDto = new ApiBoolDTO();
            apiBoolDto.Value = BLL.MunkiManagedInstall.AddManagedInstallToTemplate(managedInstall);
          
            return apiBoolDto;
        }

       

        [GlobalAuth(Permission = "GlobalDelete")]
        public ApiBoolDTO Delete(int id)
        {
            var apiBoolDto = new ApiBoolDTO();
            apiBoolDto.Value = BLL.MunkiManagedInstall.DeleteManagedInstallFromTemplate(id);
           return apiBoolDto;
        }
    }
}