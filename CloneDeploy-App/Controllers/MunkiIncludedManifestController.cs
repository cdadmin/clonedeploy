using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities;
using CloneDeploy_Services;


namespace CloneDeploy_App.Controllers
{
    public class MunkiIncludedManifestController: ApiController
    {
         private readonly MunkiIncludedManifestServices _munkiIncludedManifestServices;

        public MunkiIncludedManifestController()
        {
            _munkiIncludedManifestServices = new MunkiIncludedManifestServices();
        }

        [GlobalAuth(Permission = "GlobalRead")]
        public MunkiManifestIncludedManifestEntity Get(int id)
        {
            
             return _munkiIncludedManifestServices.GetIncludedManifest(id);

        }

       
    }
}