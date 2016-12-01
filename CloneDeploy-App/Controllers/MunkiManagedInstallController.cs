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
    public class MunkiManagedInstallController: ApiController
    {
         private readonly MunkiManagedInstallServices _munkiManagedInstallServices;

        public MunkiManagedInstallController()
        {
            _munkiManagedInstallServices = new MunkiManagedInstallServices();
        }

        [GlobalAuth(Permission = "GlobalRead")]
        public MunkiManifestManagedInstallEntity Get(int id)
        {

            return _munkiManagedInstallServices.GetManagedInstall(id);

        }

      
    }
}