using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities;
using CloneDeploy_Services;


namespace CloneDeploy_App.Controllers
{
    public class MunkiCatalogController: ApiController
    {
        private readonly MunkiCatalogServices _munkiCatalogServices;

        public MunkiCatalogController()
        {
            _munkiCatalogServices = new MunkiCatalogServices();
        }

        [GlobalAuth(Permission = "GlobalRead")]
        public MunkiManifestCatalogEntity Get(int id)
        {           
             return _munkiCatalogServices.GetCatalog(id);
        }

       
    }
}