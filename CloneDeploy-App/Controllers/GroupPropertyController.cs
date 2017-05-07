using System.Net;
using System.Net.Http;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;

namespace CloneDeploy_App.Controllers
{
    public class GroupPropertyController : ApiController
    {
        private readonly GroupPropertyServices _groupPropertyServices;

        public GroupPropertyController()
        {
            _groupPropertyServices = new GroupPropertyServices();
        }

        [CustomAuth(Permission = "GroupSearch")]
        public ActionResultDTO Post(GroupPropertyEntity groupProperty)
        {
            return _groupPropertyServices.AddGroupProperty(groupProperty);
        }

        [CustomAuth(Permission = "GroupSearch")]
        public ActionResultDTO Put(int id, GroupPropertyEntity groupProperty)
        {
            groupProperty.Id = id;
            var result = _groupPropertyServices.UpdateGroupProperty(groupProperty);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }
    }
}