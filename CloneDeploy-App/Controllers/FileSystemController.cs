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
        
    }
}