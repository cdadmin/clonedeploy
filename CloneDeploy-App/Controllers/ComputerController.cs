using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_App.DTOs;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;


namespace CloneDeploy_App.Controllers
{
    public class ComputerController : ApiController
    {
        private readonly ComputerServices _computerService;

        public ComputerController()
        {
            _computerService = new ComputerServices();
        }

        [ComputerAuth(Permission = "ComputerSearch")]
        public IEnumerable<ComputerEntity> GetAll(int limit=0,string searchstring="")
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();
            
            return string.IsNullOrEmpty(searchstring)
                ? _computerService.SearchComputersForUser(Convert.ToInt32(userId), limit)
                : _computerService.SearchComputersForUser(Convert.ToInt32(userId), limit, searchstring);

        }

        [ComputerAuth(Permission = "ComputerRead")]
        public ComputerEntity Get(int id)
        {
            var computer = _computerService.GetComputer(id);
            if (computer == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return computer;

        }

        [ComputerAuthAttribute(Permission = "ComputerCreate")]
        public ActionResultDTO Post(ComputerEntity computer)
        {
            return _computerService.AddComputer(computer);
        }

        [ComputerAuth(Permission = "ComputerUpdate")]
        public ActionResultDTO Put(int id, ComputerEntity computer)
        {
            computer.Id = id;
            var result = _computerService.UpdateComputer(computer);
            if (result.Id == 0) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, result));
            return result;
        }

        [ComputerAuth(Permission = "ComputerDelete")]
        public ActionResultDTO Delete(int id)
        {
            var result =_computerService.DeleteComputer(id);
            if (result.Id == 0) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound,result));
            return result;
        }

        [ComputerAuth(Permission = "ComputerSearch")]
        public ApiStringResponseDTO GetCount()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();

            return new ApiStringResponseDTO(){Value = _computerService.ComputerCountUser(Convert.ToInt32(userId))};
        }

        [ComputerAuth(Permission = "ComputerSearch")]
        public IEnumerable<ComputerEntity> GetComputersWithoutGroup(int limit = 0, string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? _computerService.ComputersWithoutGroup(limit)
                : _computerService.ComputersWithoutGroup(limit, searchstring);

        }

        [ComputerAuth(Permission = "ComputerRead")]
        public List<GroupMembershipEntity> GetGroupMemberships(int id)
        {
            return BLL.GroupMembership.GetAllComputerMemberships(id);
        }

        [ComputerAuth(Permission = "ComputerRead")]
        public ComputerEntity GetByMac(string mac)
        {
            return _computerService.GetComputerFromMac(mac);       
        }

        [ComputerAuth(Permission = "ComputerRead")]
        public DistributionPointEntity GetDistributionPoint(int id)
        {
            return _computerService.GetDistributionPoint(id);
        }

        [HttpGet]
        [ComputerAuth(Permission = "ComputerRead")]
        public bool IsComputerActive(int id)
        {
            return _computerService.IsComputerActive(id);       
        }

        [ComputerAuth(Permission = "ComputerRead")]
        public ActiveImagingTaskEntity GetActiveTask(int id)
        {
            return _computerService.GetTaskForComputer(id);
        }

        [ComputerAuth(Permission = "ComputerRead")]
        public ComputerBootMenuEntity GetBootMenu(int id)
        {
            return _computerService.GetComputerBootMenu(id);
        }

        [ComputerAuth(Permission = "ComputerDelete")]
        public ActionResultDTO DeleteBootMenus(int id)
        {
            return _computerService.DeleteComputerBootMenus(id);
        }

        [HttpPost]
        [ComputerAuth(Permission = "ComputerUpdate")]
        public ApiBoolDTO CreateCustomBootFiles(ComputerEntity computer)
        {
            var result = new ApiBoolDTO() { Value = true };
            BLL.ComputerBootMenu.CreateBootFiles(computer);

            return result;
        }

        [HttpPost]
        [ComputerAuth(Permission = "ComputerRead")]
        public ApiDTO GetProxyPath(ComputerEntity computer, bool isActiveOrCustom, string proxyType)
        {
            var result = new ApiDTO();
            result.Value = BLL.ComputerBootMenu.GetComputerProxyPath(computer, isActiveOrCustom, proxyType);
            return result;
        }

        [HttpPost]
        [ComputerAuth(Permission = "ComputerRead")]
        public ApiDTO GetNonProxyPath(ComputerEntity computer, bool isActiveOrCustom)
        {
            var result = new ApiDTO();
            result.Value = BLL.ComputerBootMenu.GetComputerNonProxyPath(computer, isActiveOrCustom);
            return result;
        }

        [ComputerAuth(Permission = "ComputerRead")]
        public IEnumerable<ComputerLogEntity> GetComputerLogs(int id)
        {
            return BLL.ComputerLog.Search(id);
        }

        [ComputerAuth(Permission = "ComputerDelete")]
        public ActionResultEntity DeleteAllComputerLogs(int id)
        {
            var actionResult = BLL.ComputerLog.DeleteComputerLogs(id);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }

        [ComputerAuth(Permission = "ComputerRead")]
        public IHttpActionResult GetMunkiTemplates(int id)
        {
            var munkiTemplates = BLL.ComputerMunki.Get(id);
            if (munkiTemplates == null)
                return NotFound();
            else
                return Ok(munkiTemplates);
        }

        [ComputerAuth(Permission = "ComputerRead")]
        public IHttpActionResult GetEffectiveManifest(int id)
        {
            var result = new ApiDTO();
            var effectiveManifest = new BLL.Workflows.EffectiveMunkiTemplate().Computer(id);
            result.Value = Encoding.UTF8.GetString(effectiveManifest.ToArray());
            return Ok(result);
        }

        [ComputerAuth(Permission = "ComputerDelete")]
        public ActionResultEntity DeleteMunkiTemplates(int id)
        {
            var actionResult = BLL.ComputerMunki.DeleteMunkiTemplates(id);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }

        [ComputerAuth(Permission = "ComputerRead")]
        public IHttpActionResult GetProxyReservation(int id)
        {
            var reservation = BLL.ComputerProxyReservation.GetComputerProxyReservation(id);
            if (reservation == null)
                return NotFound();
            else
                return Ok(reservation);
        }

        [HttpGet]
        [ComputerAuth(Permission = "ComputerUpdate")]
        public ApiBoolDTO ToggleProxyReservation(int id, bool status)
        {
            var result = new ApiBoolDTO();
            result.Value = BLL.ComputerProxyReservation.ToggleProxyReservation(id, status);
            return result;
        }

        [HttpGet]
        [ComputerAuth(Permission = "ComputerUpdate")]
        public ApiBoolDTO Toggle(int id, bool status)
        {
            var result = new ApiBoolDTO();
            result.Value = BLL.ComputerBootMenu.ToggleComputerBootMenu(id, status);
            return result;
        }
    }
}
