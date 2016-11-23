using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_App.DAL;
using CloneDeploy_App.DTOs;
using CloneDeploy_App.Models;
using CloneDeploy_App.Service;

namespace CloneDeploy_App.Controllers
{
    public class ComputerController : ApiController
    {
        private readonly ComputerService _computerService;

        public ComputerController()
        {
            _computerService = new ComputerService();
        }

        [ComputerAuth(Permission = "ComputerSearch")]
        public IEnumerable<Models.Computer> Get(int limit=0,string searchstring="")
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();
            
            return string.IsNullOrEmpty(searchstring)
                ? _computerService.SearchComputersForUser(Convert.ToInt32(userId), limit)
                : _computerService.SearchComputersForUser(Convert.ToInt32(userId), limit, searchstring);

        }

        [ComputerAuth(Permission = "ComputerSearch")]
        public IEnumerable<Models.Computer> GetComputersWithoutGroup(int limit = 0, string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? _computerService.ComputersWithoutGroup(limit)
                : _computerService.ComputersWithoutGroup(limit, searchstring);

        }

        [ComputerAuth(Permission = "ComputerSearch")]
        public ApiDTO GetCount()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();
  
            return _computerService.ComputerCountUser(Convert.ToInt32(userId));               
        }

        [ComputerAuth(Permission = "ComputerRead")]
        public IHttpActionResult Get(int id)
        {      
            var computer = _computerService.GetComputer(id);
            if (computer == null)
                return NotFound();
            else
                return Ok(computer);
        }

        [ComputerAuth(Permission = "ComputerRead")]
        public List<GroupMembership> GetGroupMemberships(int id)
        {
            return BLL.GroupMembership.GetAllComputerMemberships(id);
        }

        [ComputerAuth(Permission = "ComputerRead")]
        public IHttpActionResult GetFromMac(string mac)
        {
            var computer = _computerService.GetComputerFromMac(mac);
            if (computer == null)
                return NotFound();
            else
                return Ok(computer);
        }

        [ComputerAuthAttribute(Permission = "ComputerCreate")]
        public ActionResult Post(Models.Computer computer)
        {
            var actionResult = _computerService.AddComputer(computer);
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
            var actionResult = _computerService.UpdateComputer(computer);
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
            var actionResult = _computerService.DeleteComputer(id);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }

        [HttpGet]
        [ComputerAuth(Permission = "ComputerRead")]
        public ApiBoolDTO IsComputerActive(int id)
        {
            var result = new ApiBoolDTO();
            result.Value = BLL.ActiveImagingTask.IsComputerActive(id);
            return result;
        }
    }
}
