using System.Collections.Generic;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;
using CloneDeploy_Services.Helpers;
using CloneDeploy_Services.Workflows;

namespace CloneDeploy_App.Controllers
{
    public class ServiceAccountController : ApiController
    {
        [CustomAuth(Permission = "ServiceAccount")]
        [HttpGet]
        public ApiBoolResponseDTO CancelAllImagingTasks()
        {
            return new ApiBoolResponseDTO {Value = CloneDeploy_Services.Workflows.CancelAllImagingTasks.Run()};
        }

        [CustomAuth(Permission = "ServiceAccount")]
        [HttpGet]
        public ApiBoolResponseDTO CopyPxeBinaries()
        {
            return new ApiBoolResponseDTO { Value = new CopyPxeBinaries().CopyFiles() };
        }

        [CustomAuth(Permission = "ServiceAccount")]
        [HttpGet]
        public ApiBoolResponseDTO DeleteTftpFile(string path)
        {
            return new ApiBoolResponseDTO {Value = new FilesystemServices().DeleteTftpFile(path)};
        }

        [CustomAuth(Permission = "ServiceAccount")]
        [HttpPost]
        public ApiIntResponseDTO GetMulticastSenderArgs(MulticastArgsDTO multicastArgs)
        {
            return new ApiIntResponseDTO {Value = new MulticastArguments().GenerateProcessArguments(multicastArgs)};
        }

        [CustomAuth(Permission = "ServiceAccount")]
        public ServerRoleDTO GetServerRoles()
        {
            return new SettingServices().GetServerRoles();
        }

        [CustomAuth(Permission = "ServiceAccount")]
        [HttpGet]
        public ApiStringResponseDTO GetTftpServer()
        {
            return new ApiStringResponseDTO {Value = Utility.Between(Settings.TftpServerIp)};
        }

        [HttpGet]
        [CustomAuth(Permission = "ServiceAccount")]
        public ApiBoolResponseDTO Test()
        {
            return new ApiBoolResponseDTO {Value = true};
        }

        [CustomAuth(Permission = "ServiceAccount")]
        [HttpPost]
        public ApiBoolResponseDTO WriteTftpFile(TftpFileDTO tftpFile)
        {
            return new ApiBoolResponseDTO {Value = new FileOps().WritePath(tftpFile.Path, tftpFile.Contents)};
        }

        [CustomAuth(Permission = "ServiceAccount")]
        [HttpPost]
        public ApiBoolResponseDTO UpdateSettings(List<SettingEntity> listSettings)
        {
            return new ApiBoolResponseDTO { Value = new SettingServices().UpdateSetting(listSettings) };
        }
    }
}