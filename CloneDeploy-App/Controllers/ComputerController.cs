using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;
using CloneDeploy_Services.Workflows;
using Newtonsoft.Json;

namespace CloneDeploy_App.Controllers
{
    public class ComputerController : ApiController
    {
        private readonly AuditLogEntity _auditLog;
        private readonly AuditLogServices _auditLogService;
        private readonly ComputerServices _computerService;
        private readonly int _userId;

        public ComputerController()
        {
            _computerService = new ComputerServices();
            _auditLogService = new AuditLogServices();
            _userId = Convert.ToInt32(((ClaimsIdentity) User.Identity).Claims.Where(c => c.Type == "user_id")
                .Select(c => c.Value).SingleOrDefault());
            _auditLog = new AuditLogEntity();
            _auditLog.ObjectType = "Computer";
            _auditLog.UserId = _userId;
            var user = new UserServices().GetUser(_userId);
            if (user != null)
                _auditLog.UserName = user.Name;
        }

        [HttpPost]
        [CustomAuth(Permission = "ComputerCreate")]
        public ApiBoolResponseDTO AddToSmartGroups(ComputerEntity computer)
        {
            _computerService.AddComputerToSmartGroups(computer);
            return new ApiBoolResponseDTO {Value = true};
        }

        [HttpGet]
        [CustomAuth(Permission = "ComputerUpdate")]
        public ApiBoolResponseDTO CreateCustomBootFiles(int id)
        {
            return new ApiBoolResponseDTO {Value = _computerService.CreateBootFiles(id)};
        }

        [CustomAuth(Permission = "ComputerDelete")]
        public ActionResultDTO Delete(int id)
        {
            var computer = _computerService.GetComputer(id);
            var result = _computerService.DeleteComputer(id);
         
            if (result.Success)
            {
                _auditLog.AuditType = AuditEntry.Type.Delete;
                _auditLog.ObjectId = result.Id;
                _auditLog.ObjectName = computer.Name;
                _auditLog.Ip = Request.GetClientIpAddress();
                _auditLogService.AddAuditLog(_auditLog);
            }
            return result;
        }

        [CustomAuth(Permission = "ComputerDelete")]
        public ActionResultDTO DeleteAllComputerLogs(int id)
        {
            var result = _computerService.DeleteComputerLogs(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [CustomAuth(Permission = "ComputerDelete")]
        public ActionResultDTO DeleteBootMenus(int id)
        {
            return _computerService.DeleteComputerBootMenus(id);
        }

        [CustomAuth(Permission = AuthorizationStrings.DeleteComputer)]
        public ApiBoolResponseDTO DeleteImageClassifications(int id)
        {
            return new ApiBoolResponseDTO {Value = _computerService.DeleteComputerImageClassifications(id)};
        }

    

        [CustomAuth(Permission = "ComputerRead")]
        [HttpGet]
        public ApiBoolResponseDTO Export(string path)
        {
            _computerService.ExportCsv(path);
            return new ApiBoolResponseDTO {Value = true};
        }

        [Authorize]
        public ComputerEntity Get(int id)
        {
            var computer = _computerService.GetComputer(id);
            if (computer == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return computer;
        }

        [Authorize]
        public IEnumerable<ComputerEntity> Get()
        {
            return _computerService.GetAll();
        }

        [CustomAuth(Permission = "ComputerRead")]
        public ActiveImagingTaskEntity GetActiveTask(int id)
        {
            return _computerService.GetTaskForComputer(id);
        }

        [CustomAuth(Permission = "ComputerRead")]
        public ComputerBootMenuEntity GetBootMenu(int id)
        {
            return _computerService.GetComputerBootMenu(id);
        }

        [CustomAuth(Permission = "ComputerSearch")]
        public ComputerEntity GetByName(string name)
        {
            return _computerService.GetComputerFromName(name);
        }

        [CustomAuth(Permission = "ComputerRead")]
        public IEnumerable<AuditLogEntity> GetComputerAuditLogs(int id, int limit)
        {
            return _computerService.GetComputerAuditLogs(id, limit);
        }

        [CustomAuth(Permission = "ComputerRead")]
        public IEnumerable<ComputerLogEntity> GetComputerLogs(int id)
        {
            var result = _computerService.SearchComputerLogs(id);
            return result;
        }

        [CustomAuth(Permission = "ComputerSearch")]
        public IEnumerable<ComputerEntity> GetComputersWithoutGroup(int limit = 0, string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? _computerService.ComputersWithoutGroup(limit)
                : _computerService.ComputersWithoutGroup(limit, searchstring);
        }

        [Authorize]
        public ApiStringResponseDTO GetCount()
        {
            return new ApiStringResponseDTO {Value = _computerService.ComputerCountUser(_userId)};
        }

        [CustomAuth(Permission = "ComputerRead")]
        public ComputerLogEntity GetLastLog(int id)
        {
            var result = _computerService.GetLastLog(id);
            return result;
        }

   
        [CustomAuth(Permission = "ComputerRead")]
        public List<GroupMembershipEntity> GetGroupMemberships(int id)
        {
            return _computerService.GetAllComputerMemberships(id);
        }

        [CustomAuth(Permission = AuthorizationStrings.ReadComputer)]
        public IEnumerable<ComputerImageClassificationEntity> GetImageClassifications(int id)
        {
            return _computerService.GetComputerImageClassifications(id);
        }

    
        [CustomAuth(Permission = "ComputerRead")]
        public ApiStringResponseDTO GetNonProxyPath(int id, bool isActiveOrCustom)
        {
            return new ApiStringResponseDTO
            {
                Value = _computerService.GetComputerNonProxyPath(id, isActiveOrCustom)
            };
        }

        [CustomAuth(Permission = "ComputerRead")]
        public ApiStringResponseDTO GetProxyPath(int id, bool isActiveOrCustom, string proxyType)
        {
            return new ApiStringResponseDTO
            {
                Value = _computerService.GetComputerProxyPath(id, isActiveOrCustom, proxyType)
            };
        }

        [CustomAuth(Permission = "ComputerRead")]
        public ComputerProxyReservationEntity GetProxyReservation(int id)
        {
            var result = _computerService.GetComputerProxyReservation(id);
            if (result == null) return new ComputerProxyReservationEntity() { BootFile = "bios_pxelinux", NextServer = "[tftp-server-ip]" };
            return result;
        }

        [Authorize]
        public ComputerWithImage GetWithImage(int id)
        {
            var computer = _computerService.GetWithImage(id);
            if (computer == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return computer;
        }

        [HttpGet]
        [Authorize]
        public IEnumerable<ComputerWithImage> GridViewSearch(int limit = 0, string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? _computerService.SearchComputersForUser(Convert.ToInt32(_userId), limit)
                : _computerService.SearchComputersForUser(Convert.ToInt32(_userId), limit, searchstring);
        }

        [CustomAuth(Permission = "ComputerCreate")]
        [HttpPost]
        public ApiIntResponseDTO Import(ApiStringResponseDTO csvContents)
        {
            return new ApiIntResponseDTO {Value = _computerService.ImportCsv(csvContents.Value)};
        }

        [HttpGet]
        [CustomAuth(Permission = "ComputerRead")]
        public ApiBoolResponseDTO IsComputerActive(int id)
        {
            return new ApiBoolResponseDTO {Value = _computerService.IsComputerActive(id)};
        }

        [CustomAuth(Permission = "ComputerCreate")]
        public ActionResultDTO Post(ComputerEntity computer)
        {
            var result = _computerService.AddComputer(computer);
            if (result.Success)
            {
                _auditLog.AuditType = AuditEntry.Type.Create;
                _auditLog.ObjectId = result.Id;
                _auditLog.ObjectName = computer.Name;
                _auditLog.Ip = Request.GetClientIpAddress();
                _auditLog.ObjectJson = JsonConvert.SerializeObject(_computerService.GetComputer(result.Id));
                _auditLogService.AddAuditLog(_auditLog);
            }
            return result;
        }

        [CustomAuth(Permission = "ComputerUpdate")]
        public ActionResultDTO Put(int id, ComputerEntity computer)
        {
            computer.Id = id;
            var result = _computerService.UpdateComputer(computer);
            if (result.Id == 0)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, result));
            if (result.Success)
            {
                _auditLog.AuditType = AuditEntry.Type.Update;
                _auditLog.ObjectId = result.Id;
                _auditLog.ObjectName = computer.Name;
                _auditLog.Ip = Request.GetClientIpAddress();
                _auditLog.ObjectJson = JsonConvert.SerializeObject(computer);
                _auditLogService.AddAuditLog(_auditLog);
            }
            return result;
        }

        [HttpGet]
        [CustomAuth(Permission = "ComputerSearch")]
        public IEnumerable<ComputerEntity> SearchByNameOnly(int limit = 0, string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? _computerService.SearchComputersForUserByName(Convert.ToInt32(_userId), limit)
                : _computerService.SearchComputersForUserByName(Convert.ToInt32(_userId), limit, searchstring);
        }

        [HttpGet]
        [CustomAuth(Permission = "ComputerSearch")]
        public IEnumerable<ComputerEntity> TestSmartQuery(string smartType, int limit = 0, string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? _computerService.SearchComputersByName("",limit,smartType)
                : _computerService.SearchComputersByName(searchstring,limit,smartType);
        }

        [HttpGet]
        [CustomAuth(Permission = "ImageTaskDeploy")]
        public ApiStringResponseDTO StartDeploy(int id)
        {
            return new ApiStringResponseDTO
            {
                Value = new Unicast(id, "deploy", _userId, Request.GetClientIpAddress()).Start()
            };
        }

        [HttpGet]
        [CustomAuth(Permission = "ImageTaskDeploy")]
        public ApiStringResponseDTO StartPermanentDeploy(int id)
        {
            return new ApiStringResponseDTO
            {
                Value = new Unicast(id, "permanentdeploy", _userId, Request.GetClientIpAddress()).Start()
            };
        }

        [HttpGet]
        [CustomAuth(Permission = "ImageTaskUpload")]
        public ApiStringResponseDTO StartUpload(int id)
        {
            return new ApiStringResponseDTO
            {
                Value = new Unicast(id, "upload", _userId, Request.GetClientIpAddress()).Start()
            };
        }

        [HttpGet]
        [CustomAuth(Permission = "ComputerUpdate")]
        public ApiBoolResponseDTO ToggleBootMenu(int id, bool status)
        {
            return new ApiBoolResponseDTO {Value = _computerService.ToggleComputerBootMenu(id, status)};
        }

        [HttpGet]
        [CustomAuth(Permission = "ComputerUpdate")]
        public ApiBoolResponseDTO ToggleProxyReservation(int id, bool status)
        {
            return new ApiBoolResponseDTO {Value = _computerService.ToggleProxyReservation(id, status)};
        }
    }
}