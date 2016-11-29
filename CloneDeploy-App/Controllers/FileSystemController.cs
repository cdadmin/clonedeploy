using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_App.DTOs;
using CloneDeploy_App.Helpers;


namespace CloneDeploy_App.Controllers
{
    public class FileSystemController :ApiController
    {
        [HttpGet]
        [ComputerAuth(Permission = "ComputerSearch")]
        public ApiBoolDTO BootSdiExists()
        {
            var result = new ApiBoolDTO();
            result.Value = new Helpers.FileOps().FileExists(Settings.TftpPath + Path.DirectorySeparatorChar + "boot" +
                                                          Path.DirectorySeparatorChar + "boot.sdi");
            return result;
        }
    }
}