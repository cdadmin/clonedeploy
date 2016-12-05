using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Web.Http;
using CloneDeploy_ApiCalls;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_App.DTOs;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;


namespace CloneDeploy_App.Controllers
{
    public class ComputerAPI :GenericAPI<ComputerEntity>
    {
        public ComputerAPI(string resource):base(resource)
        {
		
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
            return _computerService.GetAllComputerMemberships(id);
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
        public ApiBoolResponseDTO CreateCustomBootFiles(ComputerEntity computer)
        {
            return new ApiBoolResponseDTO() {Value = _computerService.CreateBootFiles(computer)};
        }

        [HttpPost]
        [ComputerAuth(Permission = "ComputerRead")]
        public ApiStringResponseDTO GetProxyPath(ComputerEntity computer, bool isActiveOrCustom, string proxyType)
        {
            return new ApiStringResponseDTO()
            {
                Value = _computerService.GetComputerProxyPath(computer, isActiveOrCustom, proxyType)
            };
        }

        [HttpPost]
        [ComputerAuth(Permission = "ComputerRead")]
        public ApiStringResponseDTO GetNonProxyPath(ComputerEntity computer, bool isActiveOrCustom)
        {
            return new ApiStringResponseDTO()
            {
                Value = _computerService.GetComputerNonProxyPath(computer, isActiveOrCustom)
            };
        }

        [ComputerAuth(Permission = "ComputerRead")]
        public IEnumerable<ComputerLogEntity> GetComputerLogs(int id)
        {
            return _computerService.SearchComputerLogs(id);
        }

        [ComputerAuth(Permission = "ComputerDelete")]
        public ActionResultDTO DeleteAllComputerLogs(int id)
        {
            var result = _computerService.DeleteComputerLogs(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [ComputerAuth(Permission = "ComputerRead")]
        public IEnumerable<ComputerMunkiEntity> GetMunkiTemplates(int id)
        {
            return _computerService.GetMunkiTemplates(id);       
        }

        [ComputerAuth(Permission = "ComputerRead")]
        public ApiStringResponseDTO GetEffectiveManifest(int id)
        {
            var effectiveManifest = new BLL.Workflows.EffectiveMunkiTemplate().Computer(id);
            return new ApiStringResponseDTO() {Value = Encoding.UTF8.GetString(effectiveManifest.ToArray())};
        }

        [ComputerAuth(Permission = "ComputerDelete")]
        public ActionResultDTO DeleteMunkiTemplates(int id)
        {
            var result = _computerService.DeleteMunkiTemplates(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [ComputerAuth(Permission = "ComputerRead")]
        public ComputerProxyReservationEntity GetProxyReservation(int id)
        {
            var result = _computerService.GetComputerProxyReservation(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [HttpGet]
        [ComputerAuth(Permission = "ComputerUpdate")]
        public ApiBoolResponseDTO ToggleProxyReservation(int id, bool status)
        {

            return new ApiBoolResponseDTO() {Value = _computerService.ToggleProxyReservation(id, status)};

        }

        [HttpGet]
        [ComputerAuth(Permission = "ComputerUpdate")]
        public ApiBoolResponseDTO Toggle(int id, bool status)
        {

            return new ApiBoolResponseDTO() {Value = _computerService.ToggleComputerBootMenu(id, status)};

        }
    }
}
