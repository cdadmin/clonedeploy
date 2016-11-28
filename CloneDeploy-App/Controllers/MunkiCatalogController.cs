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
    public class MunkiCatalogController: ApiController
    {
        [GlobalAuth(Permission = "GlobalRead")]
        public Models.MunkiManifestCatalog Get(int id)
        {
            
             return BLL.MunkiCatalog.GetCatalog(id);

        }

        [GlobalAuth(Permission = "GlobalCreate")]
        public ApiBoolDTO Post(Models.MunkiManifestCatalog catalog)
        {
            var apiBoolDto = new ApiBoolDTO();
            apiBoolDto.Value = BLL.MunkiCatalog.AddCatalogToTemplate(catalog);
          
            return apiBoolDto;
        }

       

        [GlobalAuth(Permission = "GlobalDelete")]
        public ApiBoolDTO Delete(int id)
        {
            var apiBoolDto = new ApiBoolDTO();
            apiBoolDto.Value = BLL.MunkiCatalog.DeleteCatalogFromTemplate(id);
           return apiBoolDto;
        }
    }
}