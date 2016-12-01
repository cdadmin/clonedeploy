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
using CloneDeploy_Services;


namespace CloneDeploy_App.Controllers
{
    public class MunkiManagedUnInstallController: ApiController
    {
         private readonly MunkiManagedUninstallServices _munkiManagedUninstallServices;

        public MunkiManagedUnInstallController()
        {
            _munkiManagedUninstallServices = new MunkiManagedUninstallServices();
        }

        [GlobalAuth(Permission = "GlobalRead")]
        public MunkiManifestManagedUnInstallEntity Get(int id)
        {

            return _munkiManagedUninstallServices.GetManagedUnInstall(id);

        }

       
    }
}