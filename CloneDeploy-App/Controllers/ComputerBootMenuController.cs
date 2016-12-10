using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;


namespace CloneDeploy_App.Controllers
{
    public class ComputerBootMenuController : ApiController
    {

        private readonly ComputerBootMenuServices _computerBootMenuServices;

        public ComputerBootMenuController()
        {
            _computerBootMenuServices = new ComputerBootMenuServices();
        }

        [CustomAuth(Permission = "ComputerUpdate")]
        public ActionResultDTO Post(ComputerBootMenuEntity computerBootMenu)
        {
            return _computerBootMenuServices.UpdateComputerBootMenu(computerBootMenu);
        }

       

      
    }
}