using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;

namespace CloneDeploy_App.Controllers
{
    public class GroupMunkiController : ApiController
    {
        private readonly GroupMunkiServices _groupMunkiServices;

        public GroupMunkiController()
        {
            _groupMunkiServices = new GroupMunkiServices();
        }

        [CustomAuth(Permission = "GroupSearch")]
        public ActionResultDTO Post(GroupMunkiEntity groupMunki)
        {
            return _groupMunkiServices.AddMunkiTemplates(groupMunki);
        }
    }
}