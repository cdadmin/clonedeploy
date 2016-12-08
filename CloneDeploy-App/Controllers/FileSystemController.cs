using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_App.DTOs;
using CloneDeploy_App.Helpers;
using CloneDeploy_Entities.DTOs;


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
                Value = new Helpers.FileOps().FileExists(Settings.TftpPath + Path.DirectorySeparatorChar + "boot" +
                                                         Path.DirectorySeparatorChar + "boot.sdi")
            };
            
        }

        [HttpGet]
        [ComputerAuth(Permission = "ComputerSearch")]
        public ApiStringResponseDTO ReadFileText(string path)
        {

            return new ApiStringResponseDTO
            {
                Value = new Helpers.FileOps().ReadAllText(path)
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
        public ApiStringArrResponseDTO GetBootImages()
        {

            return new ApiStringArrResponseDTO
            {
                Value = Utility.GetBootImages()
            };

        }

        [HttpGet]
        [ComputerAuth(Permission = "ComputerSearch")]
        public ApiStringArrResponseDTO GetLogs()
        {

            return new ApiStringArrResponseDTO
            {
                Value = Utility.GetLogs()
            };

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
        public ApiStringArrResponseDTO GetScripts(string type)
        {

            return new ApiStringArrResponseDTO
            {
                Value = Utility.GetScripts(type)
            };

        }

        [HttpGet]
        [ComputerAuth(Permission = "ComputerSearch")]
        public ApiStringArrResponseDTO GetThinImages()
        {

            return new ApiStringArrResponseDTO
            {
                Value = Utility.GetThinImages()
            };

        }
        
    }
}