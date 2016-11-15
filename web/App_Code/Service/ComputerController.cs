using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Validation;
using Helpers;
using Models;
using Newtonsoft.Json;

namespace Service
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

            var objectId = Convert.ToInt32(actionContext.ControllerContext.RouteData.Values["id"]);

            var authorized = false;
            switch (Permission)
            {
                case "ComputerSearch": case "ComputerCreate":
                    if (new BLL.Auth(Convert.ToInt32(userId), Permission).IsAuthorized())
                        authorized = true;
                    break;
                case "ComputerDelete": case "ComputerUpdate": case "ComputerRead":
                    if (new BLL.Auth(Convert.ToInt32(userId), Permission).ComputerManagement(objectId))
                        authorized = true;
                    break;
            }

            if(!authorized)
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);

        }
    }
    public class ComputerController : ApiController
    {

        [ComputerAuthAttribute(Permission = "ComputerSearch")]
        public IEnumerable<Models.Computer> Get(int limit=0,string searchstring="")
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();
          
            if(string.IsNullOrEmpty(searchstring))   
                return BLL.Computer.SearchComputersForUser(Convert.ToInt32(userId),limit);
            else
                return BLL.Computer.SearchComputersForUser(Convert.ToInt32(userId), limit,searchstring); 

        }

        [ComputerAuthAttribute(Permission = "ComputerRead")]
        public Computer Get(int id)
        {      
            return BLL.Computer.GetComputer(id);
        }

        [ComputerAuthAttribute(Permission = "ComputerRead")]
        public Computer GetFromMac(string mac)
        {
          
            return BLL.Computer.GetComputerFromMac(mac);
        }

        [ComputerAuthAttribute(Permission = "ComputerCreate")]
        public HttpResponseMessage Post(Models.Computer computer)
        {
            var result = BLL.Computer.AddComputer(computer);
            if (result.Success)
            {
                var response = Request.CreateResponse<Models.ActionResult>(HttpStatusCode.Created, result);
                string uri = Url.Link("DefaultApi", new {id = result.ObjectId});
                response.Headers.Location = new Uri(uri);
                return response;
            }
            else
            {
                var response = Request.CreateResponse<Models.ActionResult>(HttpStatusCode.NotFound, result);
                throw new HttpResponseException(response);
            }
        }

        [ComputerAuthAttribute(Permission = "ComputerUpdate")]
        public Models.ActionResult Put(Models.Computer value)
        {
           
            return BLL.Computer.UpdateComputer(value);
        }

        [ComputerAuthAttribute(Permission = "ComputerDelete")]
        public Models.ActionResult Delete(int id)
        {
            var result = BLL.Computer.DeleteComputer(id);
            if (!result.Success)
            {
                var response = Request.CreateResponse<Models.ActionResult>(HttpStatusCode.NotFound, result);
                throw new HttpResponseException(response);
            }
            return result;
        }
    }
}
