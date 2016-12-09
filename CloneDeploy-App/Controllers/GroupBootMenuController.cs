using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;


namespace CloneDeploy_App.Controllers
{
    public class GroupBootMenuController : ApiController
    {

        private readonly GroupBootMenuServices _groupBootMenuServices;

        public GroupBootMenuController()
        {
            _groupBootMenuServices = new GroupBootMenuServices();
            
        }

        [GroupAuth(Permission = "GroupUpdate")]
        public ActionResultDTO Post(GroupBootMenuEntity groupBootMenu)
        {
            return _groupBootMenuServices.UpdateGroupBootMenu(groupBootMenu);
        }

       

       
    }
}