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
    public class MunkiOptionalInstallController: ApiController
    {
         private readonly MunkiOptionalInstallServices _munkiOptionalInstallServices;

        public MunkiOptionalInstallController()
        {
            _munkiOptionalInstallServices = new MunkiOptionalInstallServices();
        }

        [GlobalAuth(Permission = "GlobalRead")]
        public MunkiManifestOptionInstallEntity Get(int id)
        {

            return _munkiOptionalInstallServices.GetOptionalInstall(id);

        }

      
    }
}