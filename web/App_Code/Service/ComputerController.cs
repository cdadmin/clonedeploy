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

            // Get the claims values
            //var name = identity.Claims.Where(c => c.Type == ClaimTypes.Name)
            //                   .Select(c => c.Value).SingleOrDefault();
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                              .Select(c => c.Value).SingleOrDefault();

            //Computer model = (Computer)actionContext.ActionArguments["value"];
            foreach (var t in actionContext.ActionArguments)
            {
                Logger.Log(t.Key);
            }

            //string param1 = (string)actionContext.ActionArguments["param1"];


            var objectId = Convert.ToInt32(actionContext.ControllerContext.RouteData.Values["id"]);

            bool authorized = false;
            switch (Permission)
            {
                case "ComputerSearch":
                    if (new BLL.Auth(Convert.ToInt32(userId), Permission).IsAuthorized())
                        authorized = true;
                    break;
                case "ComputerDelete":
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
            Logger.Log("limit" + limit + "search" + searchstring);
            if(string.IsNullOrEmpty(searchstring))   
                return BLL.Computer.SearchComputersForUser(Convert.ToInt32(userId),limit);
            else
                return BLL.Computer.SearchComputersForUser(Convert.ToInt32(userId), limit,searchstring); 

        }
    
        public Computer Get(int id)
        {      
            return BLL.Computer.GetComputer(id);
        }

        public Computer GetFromMac(string mac)
        {
          
            return BLL.Computer.GetComputerFromMac(mac);
        }

        public HttpResponseMessage Post(Models.Computer computer)
        {
            var result = BLL.Computer.AddComputer(computer);
            if (result.IsValid)
            {
                computer = BLL.Computer.GetComputer(result.ObjectId);
                var response = Request.CreateResponse<Models.Computer>(HttpStatusCode.Created, computer);

                string uri = Url.Link("DefaultApi", new {id = result.ObjectId});
                response.Headers.Location = new Uri(uri);

                return response;
            }
            else
            {
                Logger.Log("Could Not Create Computer" + JsonConvert.SerializeObject(computer));
                Logger.Log(result.Message);
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        public Models.ValidationResult Put(Models.Computer value)
        {
           
            return BLL.Computer.UpdateComputer(value);
        }

        [ComputerAuthAttribute(Permission = "ComputerDelete")]
        public Models.ValidationResult Delete(int id)
        {
            if (BLL.Computer.GetComputer(id) == null)
            {
                Logger.Log("Could Not Delete Computer With Id" + id + ".  The Computer Was not Found");
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return BLL.Computer.DeleteComputer(id);
        }
    }
}
