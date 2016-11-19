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
    public class ComputerAuthAttribute : AuthorizeAttribute
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
                case "ComputerSearch": case "ComputerCreate":
                    if (new BLL.Auth(Convert.ToInt32(userId), Permission).IsAuthorized())
                        authorized = true;
                    break;
                case "ComputerDelete": case "ComputerUpdate": case "ComputerRead":
                    var objectId = Convert.ToInt32(actionContext.ControllerContext.RouteData.Values["id"]);
                    if (new BLL.Auth(Convert.ToInt32(userId), Permission).ComputerManagement(objectId))
                        authorized = true;
                    break;
                case "NotRequired":
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

    public class ComputerController : ApiController
    {
        [ComputerAuth(Permission = "ComputerSearch")]
        public IEnumerable<Models.Computer> Get(int limit=0,string searchstring="")
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();

            return string.IsNullOrEmpty(searchstring)
                ? BLL.Computer.SearchComputersForUser(Convert.ToInt32(userId), limit)
                : BLL.Computer.SearchComputersForUser(Convert.ToInt32(userId), limit, searchstring);

        }

        [ComputerAuth(Permission = "ComputerSearch")]
        public ApiDTO GetCount()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();
  
            return BLL.Computer.ComputerCountUser(Convert.ToInt32(userId));               
        }

        [ComputerAuth(Permission = "ComputerRead")]
        public IHttpActionResult Get(int id)
        {      
            var computer = BLL.Computer.GetComputer(id);
            if (computer == null)
                return NotFound();
            else
                return Ok(computer);
        }

        [ComputerAuth(Permission = "ComputerRead")]
        public IHttpActionResult GetFromMac(string mac)
        {
            var computer = BLL.Computer.GetComputerFromMac(mac);
            if (computer == null)
                return NotFound();
            else
                return Ok(computer);
        }

        //[ComputerAuthAttribute(Permission = "ComputerCreate")]
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

        [ComputerAuth(Permission = "ComputerUpdate")]
        public Models.ActionResult Put(int id,Models.Computer computer)
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

        [ComputerAuth(Permission = "ComputerDelete")]
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
