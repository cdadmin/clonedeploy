using System.Collections.Generic;
using System.IO;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services.Helpers;


namespace CloneDeploy_App.Controllers
{
    public class FileSystemController :ApiController
    {
        [HttpGet]
        [ComputerAuth(Permission = "ComputerSearch")]
        public ApiBoolResponseDTO BootSdiExists()
        {

            return new ApiBoolResponseDTO
            {
                Value = new FileOps().FileExists(Settings.TftpPath + Path.DirectorySeparatorChar + "boot" +
                                                         Path.DirectorySeparatorChar + "boot.sdi")
            };
            
        }

        [HttpGet]
        [ComputerAuth(Permission = "ComputerSearch")]
        public ApiStringResponseDTO ReadFileText(string path)
        {

            return new ApiStringResponseDTO
            {
                Value = new FileOps().ReadAllText(path)
            };

        }

        [HttpGet]
        [ComputerAuth(Permission = "ComputerSearch")]
        public IEnumerable<string> GetKernels()
        {


            return Utility.GetKernels();

        }

        [HttpGet]
        [ComputerAuth(Permission = "ComputerSearch")]
        public IEnumerable<string> GetBootImages()
        {

            return Utility.GetBootImages();
        }

        [HttpGet]
        [ComputerAuth(Permission = "ComputerSearch")]
        public List<string> GetLogs()
        {


            return Utility.GetLogs();


        }

        [HttpGet]
        [ComputerAuth(Permission = "ComputerSearch")]
        public MunkiPackageInfoEntity GetPlist(string file)
        {
            return new Utility().ReadPlist(file);
        }

        [HttpGet]
        [ComputerAuth(Permission = "ComputerSearch")]
        public List<FileInfo> GetMunkiResources(string resourceType)
        {
            return new Utility().GetMunkiResources(resourceType);
        }


        [HttpGet]
        [ComputerAuth(Permission = "ComputerSearch")]
        public ApiBoolResponseDTO SetUnixPermissions(string path)
        {
           new FileOps().SetUnixPermissions(path);
            return new ApiBoolResponseDTO() {Value = true};
        }

        [HttpGet]
        [ComputerAuth(Permission = "ComputerSearch")]
        public List<string> GetScripts(string type)
        {


            return Utility.GetScripts(type);


        }

        [HttpGet]
        [ComputerAuth(Permission = "ComputerSearch")]
        public List<string> GetThinImages()
        {


            return Utility.GetThinImages();


        }
        
    }
}