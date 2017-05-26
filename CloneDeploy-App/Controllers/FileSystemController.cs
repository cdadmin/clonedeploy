using System.Collections.Generic;
using System.IO;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;
using CloneDeploy_Services.Helpers;

namespace CloneDeploy_App.Controllers
{
    public class FileSystemController : ApiController
    {
        [HttpGet]
        [Authorize]
        public ApiBoolResponseDTO BootSdiExists()
        {
            return new ApiBoolResponseDTO
            {
                Value = new FileOpsServices().FileExists(SettingServices.GetSettingValue(SettingStrings.TftpPath) + Path.DirectorySeparatorChar + "boot" +
                                                 Path.DirectorySeparatorChar + "boot.sdi")
            };
        }

        [CustomAuth(Permission = "AdminUpdate")]
        [HttpPost]
        public ApiBoolResponseDTO EditDefaultBootMenu(CoreScriptDTO menu)
        {
            return new ApiBoolResponseDTO
            {
                Value = new FilesystemServices().EditDefaultBootMenu(menu.Name, menu.Contents)
            };
        }

        [HttpGet]
        [Authorize]
        public IEnumerable<string> GetBootImages()
        {
            return FilesystemServices.GetBootImages();
        }

        [CustomAuth(Permission = "AdminUpdate")]
        public ApiStringResponseDTO GetDefaultBootFilePath(string type)
        {
            return new ApiStringResponseDTO {Value = new FilesystemServices().GetDefaultBootMenuPath(type)};
        }

        [Authorize]
        public DpFreeSpaceDTO GetDpFreeSpace()
        {
            return new FilesystemServices().GetDpFreeSpace();
        }

        [HttpGet]
        [Authorize]
        public IEnumerable<string> GetKernels()
        {
            return FilesystemServices.GetKernels();
        }

        [CustomAuth(Permission = "AdminRead")]
        public List<string> GetLogContents(string name, int limit)
        {
            return new FilesystemServices().GetLogContents(name, limit);
        }

        [HttpGet]
        [Authorize]
        public List<string> GetLogs()
        {
            return FilesystemServices.GetLogs();
        }

        [HttpGet]
        [Authorize]
        public List<FileInfo> GetMunkiResources(string resourceType)
        {
            return new MunkiManifestTemplateServices().GetMunkiResources(resourceType);
        }

        [HttpGet]
        [Authorize]
        public MunkiPackageInfoEntity GetPlist(string file)
        {
            return new MunkiManifestTemplateServices().ReadPlist(file);
        }

        [HttpGet]
        [Authorize]
        public List<string> GetScripts(string type)
        {
            return FilesystemServices.GetScripts(type);
        }


        [CustomAuth(Permission = "AdminRead")]
        public ApiStringResponseDTO GetServerPaths(string type, string subType)
        {
            return new ApiStringResponseDTO {Value = new FilesystemServices().GetServerPaths(type, subType)};
        }

      

        [HttpGet]
        [Authorize]
        public ApiStringResponseDTO ReadFileText(string path)
        {
            return new ApiStringResponseDTO
            {
                Value = new FileOpsServices().ReadAllText(path)
            };
        }


        [HttpGet]
        [Authorize]
        public ApiBoolResponseDTO SetUnixPermissions(string path)
        {
            new FileOpsServices().SetUnixPermissions(path);
            return new ApiBoolResponseDTO {Value = true};
        }

        [CustomAuth(Permission = "Administrator")]
        [HttpPost]
        public ApiBoolResponseDTO WriteCoreScript(CoreScriptDTO script)
        {
            return new ApiBoolResponseDTO
            {
                Value = new FilesystemServices().WriteCoreScript(script.Name, script.Contents)
            };
        }
    }
}