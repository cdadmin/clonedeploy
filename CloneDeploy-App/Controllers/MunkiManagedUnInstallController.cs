using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_App.DTOs;
using CloneDeploy_App.Models;

namespace CloneDeploy_App.Controllers
{
    public class MunkiManagedUnInstallController: ApiController
    {
        [GlobalAuth(Permission = "GlobalRead")]
        public Models.MunkiManifestManagedUnInstall Get(int id)
        {

            return BLL.MunkiManagedUninstall.GetManagedUnInstall(id);

        }

        [GlobalAuth(Permission = "GlobalCreate")]
        public ApiBoolDTO Post(Models.MunkiManifestManagedUnInstall managedUninstall)
        {
            var apiBoolDto = new ApiBoolDTO();
            apiBoolDto.Value = BLL.MunkiManagedUninstall.AddManagedUnInstallToTemplate(managedUninstall);
          
            return apiBoolDto;
        }

       

        [GlobalAuth(Permission = "GlobalDelete")]
        public ApiBoolDTO Delete(int id)
        {
            var apiBoolDto = new ApiBoolDTO();
            apiBoolDto.Value = BLL.MunkiManagedUninstall.DeleteManagedUnInstallFromTemplate(id);
           return apiBoolDto;
        }
    }
}