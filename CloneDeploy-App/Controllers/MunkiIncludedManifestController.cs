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
    public class MunkiIncludedManifestController: ApiController
    {
        [GlobalAuth(Permission = "GlobalRead")]
        public MunkiManifestIncludedManifestEntity Get(int id)
        {
            
             return BLL.MunkiIncludedManifest.GetIncludedManifest(id);

        }

        [GlobalAuth(Permission = "GlobalCreate")]
        public ApiBoolDTO Post(MunkiManifestIncludedManifestEntity manifest)
        {
            var apiBoolDto = new ApiBoolDTO();
            apiBoolDto.Value = BLL.MunkiIncludedManifest.AddIncludedManifestToTemplate(manifest);
            return apiBoolDto;
        }

       

        [GlobalAuth(Permission = "GlobalDelete")]
        public ApiBoolDTO Delete(int id)
        {
            var apiBoolDto = new ApiBoolDTO();
            apiBoolDto.Value = BLL.MunkiIncludedManifest.DeleteIncludedManifestFromTemplate(id);
           return apiBoolDto;
        }
    }
}