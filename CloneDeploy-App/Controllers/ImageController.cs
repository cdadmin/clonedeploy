using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Entities.DTOs.ImageSchemaBE;
using CloneDeploy_Services;
using Newtonsoft.Json;

namespace CloneDeploy_App.Controllers
{
    public class ImageController : ApiController
    {
        private readonly AuditLogEntity _auditLog;
        private readonly AuditLogServices _auditLogService;
        private readonly ImageServices _imageServices;
        private readonly int _userId;

        public ImageController()
        {
            _imageServices = new ImageServices();
            _auditLogService = new AuditLogServices();
            _userId = Convert.ToInt32(((ClaimsIdentity) User.Identity).Claims.Where(c => c.Type == "user_id")
                .Select(c => c.Value).SingleOrDefault());
            _auditLog = new AuditLogEntity();
            _auditLog.ObjectType = "Image";
            _auditLog.UserId = _userId;
            var user = new UserServices().GetUser(_userId);
            if (user != null)
                _auditLog.UserName = user.Name;
        }

        [CustomAuth(Permission = "ImageDelete")]
        public ActionResultDTO Delete(int id)
        {
            var image = _imageServices.GetImage(id);
            var result = _imageServices.DeleteImage(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            if (result.Success)
            {
                _auditLog.AuditType = AuditEntry.Type.Delete;
                _auditLog.ObjectId = result.Id;
                _auditLog.ObjectName = image.Name;
                _auditLog.Ip = Request.GetClientIpAddress();
                _auditLogService.AddAuditLog(_auditLog);
            }
            return result;
        }

        [CustomAuth(Permission = "ImageRead")]
        [HttpGet]
        public ApiBoolResponseDTO Export(string path)
        {
            _imageServices.ExportCsv(path);
            return new ApiBoolResponseDTO {Value = true};
        }

        [Authorize]
        public ImageEntity Get(int id)
        {
            var result = _imageServices.GetImage(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [Authorize]
        public IEnumerable<ImageWithDate> Get(string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? _imageServices.SearchImagesForUser(_userId)
                : _imageServices.SearchImagesForUser(Convert.ToInt32(_userId), searchstring);
        }

        [CustomAuth(Permission = "ImageSearch")]
        public ApiStringResponseDTO GetCount()
        {
            return new ApiStringResponseDTO {Value = _imageServices.ImageCountUser(_userId)};
        }

        [CustomAuth(Permission = "ImageRead")]
        public IEnumerable<AuditLogEntity> GetImageAuditLogs(int id, int limit)
        {
            return _imageServices.GetImageAuditLogs(id, limit);
        }

        [Authorize]
        public IEnumerable<ImageProfileEntity> GetImageProfiles(int id)
        {
            return _imageServices.SearchProfiles(id);
        }

        [CustomAuth(Permission = "ImageSearch")]
        public ApiStringResponseDTO GetImageSizeOnServer(string imageName, string hdNumber)
        {
            return new ApiStringResponseDTO {Value = _imageServices.ImageSizeOnServerForGridView(imageName, hdNumber)};
        }

        [CustomAuth(Permission = "ImageRead")]
        public IEnumerable<ImageFileInfo> GetPartitionFileInfo(int id, string selectedHd, string selectedPartition)
        {
            return _imageServices.GetPartitionImageFileInfoForGridView(id, selectedHd, selectedPartition);
        }

        [CustomAuth(Permission = "ComputerCreate")]
        [HttpPost]
        public ApiIntResponseDTO Import(ApiStringResponseDTO csvContents)
        {
            return new ApiIntResponseDTO {Value = _imageServices.ImportCsv(csvContents.Value)};
        }

        [CustomAuth(Permission = "ImageCreate")]
        public ActionResultDTO Post(ImageEntity image)
        {
            var result = _imageServices.AddImage(image);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            if (result.Success)
            {
                _auditLog.AuditType = AuditEntry.Type.Create;
                _auditLog.ObjectId = result.Id;
                _auditLog.ObjectName = image.Name;
                _auditLog.Ip = Request.GetClientIpAddress();
                _auditLog.ObjectJson = JsonConvert.SerializeObject(_imageServices.GetImage(result.Id));
                _auditLogService.AddAuditLog(_auditLog);
            }
            return result;
        }

        [CustomAuth(Permission = "ImageUpdate")]
        public ActionResultDTO Put(int id, ImageEntity image)
        {
            image.Id = id;
            var result = _imageServices.UpdateImage(image);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            if (result.Success)
            {
                _auditLog.AuditType = AuditEntry.Type.Update;
                _auditLog.ObjectId = result.Id;
                _auditLog.ObjectName = image.Name;
                _auditLog.Ip = Request.GetClientIpAddress();
                _auditLog.ObjectJson = JsonConvert.SerializeObject(image);
                _auditLogService.AddAuditLog(_auditLog);
            }
            return result;
        }

        [CustomAuth(Permission = "ImageSearch")]
        public IEnumerable<ImageWithDate> Search(string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? _imageServices.SearchImages()
                : _imageServices.SearchImages(searchstring);
        }

        [HttpGet]
        [CustomAuth(Permission = "ImageCreate")]
        public ImageProfileEntity SeedDefaultProfile(int id)
        {
            return _imageServices.SeedDefaultImageProfile(id);
        }

        [Authorize]
        [HttpGet]
        public ApiBoolResponseDTO SendImageApprovedMail(int id)
        {
            _imageServices.SendImageApprovedEmail(id);
            return new ApiBoolResponseDTO {Value = true};
        }
    }
}