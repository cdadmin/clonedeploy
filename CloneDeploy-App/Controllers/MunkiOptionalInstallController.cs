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
    public class MunkiOptionalInstallController: ApiController
    {
        [GlobalAuth(Permission = "GlobalRead")]
        public Models.MunkiManifestOptionInstall Get(int id)
        {

            return BLL.MunkiOptionalInstall.GetOptionalInstall(id);

        }

        [GlobalAuth(Permission = "GlobalCreate")]
        public ApiBoolDTO Post(Models.MunkiManifestOptionInstall optionalInstall)
        {
            var apiBoolDto = new ApiBoolDTO();
            apiBoolDto.Value = BLL.MunkiOptionalInstall.AddOptionalInstallToTemplate(optionalInstall);
          
            return apiBoolDto;
        }

       

        [GlobalAuth(Permission = "GlobalDelete")]
        public ApiBoolDTO Delete(int id)
        {
            var apiBoolDto = new ApiBoolDTO();
            apiBoolDto.Value = BLL.MunkiOptionalInstall.DeleteOptionalInstallFromTemplate(id);
           return apiBoolDto;
        }
    }
}