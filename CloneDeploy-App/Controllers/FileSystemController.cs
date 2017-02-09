using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;
using CloneDeploy_Services.Helpers;


namespace CloneDeploy_App.Controllers
{
    public class FileSystemController :ApiController
    {
        [HttpGet]
        [Authorize]
        public ApiBoolResponseDTO BootSdiExists()
        {

            return new ApiBoolResponseDTO
            {
                Value = new FileOps().FileExists(Settings.TftpPath + Path.DirectorySeparatorChar + "boot" +
                                                         Path.DirectorySeparatorChar + "boot.sdi")
            };
            
        }

        [HttpGet]
        [Authorize]
        public ApiStringResponseDTO ReadFileText(string path)
        {

            return new ApiStringResponseDTO
            {
                Value = new FileOps().ReadAllText(path)
            };

        }

        [HttpGet]
        [Authorize]
        public IEnumerable<string> GetKernels()
        {


            return Utility.GetKernels();

        }

        [HttpGet]
        [Authorize]
        public IEnumerable<string> GetBootImages()
        {

            return Utility.GetBootImages();
        }

        [HttpGet]
        [Authorize]
        public List<string> GetLogs()
        {


            return Utility.GetLogs();


        }

        [HttpGet]
        [Authorize]
        public MunkiPackageInfoEntity GetPlist(string file)
        {
            return new Utility().ReadPlist(file);
        }

        [HttpGet]
        [Authorize]
        public List<FileInfo> GetMunkiResources(string resourceType)
        {
            return new Utility().GetMunkiResources(resourceType);
        }


        [HttpGet]
        [Authorize]
        public ApiBoolResponseDTO SetUnixPermissions(string path)
        {
           new FileOps().SetUnixPermissions(path);
            return new ApiBoolResponseDTO() {Value = true};
        }

        [HttpGet]
        [Authorize]
        public List<string> GetScripts(string type)
        {
            return Utility.GetScripts(type);
        }

        [HttpGet]
        [Authorize]
        public List<string> GetThinImages()
        {
            return Utility.GetThinImages();
        }

        [Authorize]
        public DpFreeSpaceDTO GetDpFreeSpace()
        {
            return new FilesystemServices().GetDpFreeSpace();
        }

        [CustomAuth(Permission = "AdminUpdate")]
        public ApiStringResponseDTO GetDefaultBootFilePath(string type)
        {
            return new ApiStringResponseDTO() {Value = new FilesystemServices().GetDefaultBootMenuPath(type)};
        }

         [CustomAuth(Permission = "AdminUpdate")]
        [HttpPost]
        public ApiBoolResponseDTO EditDefaultBootMenu(CoreScriptDTO menu)
        {
            return new ApiBoolResponseDTO() { Value = new FilesystemServices().EditDefaultBootMenu(menu.Name,menu.Contents) };
        }

         [CustomAuth(Permission = "Administrator")]
         [HttpPost]
         public ApiBoolResponseDTO WriteCoreScript(CoreScriptDTO script)
         {
             return new ApiBoolResponseDTO() { Value = new FilesystemServices().WriteCoreScript(script.Name,script.Contents) };
         }


         [CustomAuth(Permission = "AdminRead")]
         public ApiStringResponseDTO GetServerPaths(string type, string subType)
         {
             return new ApiStringResponseDTO() { Value = new FilesystemServices().GetServerPaths(type,subType) };
         }

         [CustomAuth(Permission = "AdminRead")]
         public List<string> GetLogContents(string name,int limit)
         {
             return new FilesystemServices().GetLogContents(name,limit);
         }

        [CustomAuth(Permission = "ServiceAccount")]
        [HttpPost]
        public ApiBoolResponseDTO WriteTftpFile(TftpFileDTO tftpFile)
        {
            return new ApiBoolResponseDTO() {Value = new FileOps().WritePath(tftpFile.Path, tftpFile.Contents)};
        }

        [CustomAuth(Permission = "ServiceAccount")]
        [HttpGet]
        public ApiBoolResponseDTO DeleteTftpFile(string path)
        {
            return new ApiBoolResponseDTO() {Value = new FilesystemServices().DeleteTftpFile(path)};
        }
        
    }
}