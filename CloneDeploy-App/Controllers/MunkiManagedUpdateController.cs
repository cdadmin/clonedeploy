using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
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

        [CustomAuth(Permission = "GlobalRead")]
        public MunkiManifestManagedUpdateEntity Get(int id)
        {

            return _munkiManagedUpdateServices.GetManagedUpdate(id);

        }

     
    }
}