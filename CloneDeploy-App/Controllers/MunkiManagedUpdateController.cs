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
    public class MunkiManagedUpdateController: ApiController
    {
         private readonly MunkiManagedUpdateServices _munkiManagedUpdateServices;

        public MunkiManagedUpdateController()
        {
            _munkiManagedUpdateServices = new MunkiManagedUpdateServices();
        }

        [GlobalAuth(Permission = "GlobalRead")]
        public MunkiManifestManagedUpdateEntity Get(int id)
        {

            return _munkiManagedUpdateServices.GetManagedUpdate(id);

        }

     
    }
}