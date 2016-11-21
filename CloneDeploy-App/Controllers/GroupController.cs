using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using CloneDeploy_App.Models;

namespace CloneDeploy_App.Controllers
{
    public class GroupAuthAttribute : AuthorizeAttribute
    {
        public string Permission { get; set; }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                              .Select(c => c.Value).SingleOrDefault();

            if (userId == null) return;


            var authorized = false;
            switch (Permission)
            {
                case "GroupSearch":
                case "GroupCreate":
                    if (new BLL.Auth(Convert.ToInt32(userId), Permission).IsAuthorized())
                        authorized = true;
                    break;
                case "GroupDelete":
                case "GroupUpdate":
                case "GroupRead":
                    var objectId = Convert.ToInt32(actionContext.ControllerContext.RouteData.Values["id"]);
                    if (new BLL.Auth(Convert.ToInt32(userId), Permission).GroupManagement(objectId))
                        authorized = true;
                    break;
            }

            if (!authorized)
            {
                var response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden, new ActionResult() { Success = false, Message = "Forbidden" });
                throw new HttpResponseException(response);
            }
        }
    }

    public class GroupController : ApiController
    {
        [GroupAuth(Permission = "GroupSearch")]
        public IEnumerable<Models.Group> Get(string searchstring = "")
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();
            return string.IsNullOrEmpty(searchstring)
                ? BLL.Group.SearchGroupsForUser(Convert.ToInt32(userId))
                : BLL.Group.SearchGroupsForUser(Convert.ToInt32(userId), searchstring);

        }

        [GroupAuth(Permission = "GroupSearch")]
        public ApiDTO GetCount()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();

            return BLL.Group.GroupCountUser(Convert.ToInt32(userId));
        }

        [GroupAuth(Permission = "GroupRead")]
        public IHttpActionResult Get(int id)
        {
            var group = BLL.Group.GetGroup(id);
            if (group == null)
                return Content(HttpStatusCode.NotFound, new ActionResult());
            else
                return Ok(group);
        }

        [GroupAuth(Permission = "GroupRead")]
        public IHttpActionResult GetMemberCount(int id)
        {
            var group = BLL.GroupMembership.GetGroupMemberCount(id);
            if (group == null)
                return Content(HttpStatusCode.NotFound, new ActionResult());
            else
                return Ok(group);
        }
       

        [ComputerAuthAttribute(Permission = "GroupCreate")]
        public ActionResult Post(Models.Computer computer)
        {
            var actionResult = BLL.Computer.AddComputer(computer);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }

        [ComputerAuth(Permission = "GroupUpdate")]
        public Models.ActionResult Put(int id, Models.Computer computer)
        {
            computer.Id = id;
            var actionResult = BLL.Computer.UpdateComputer(computer);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }

        [ComputerAuth(Permission = "GroupDelete")]
        public Models.ActionResult Delete(int id)
        {
            var actionResult = BLL.Computer.DeleteComputer(id);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }
    }
}
