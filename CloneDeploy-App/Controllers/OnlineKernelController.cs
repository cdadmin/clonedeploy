using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;

namespace CloneDeploy_App.Controllers
{
    public class OnlineKernelController: ApiController
    {
         private readonly OnlineKernelServices _onlineKernelServices;

        public OnlineKernelController()
        {
            _onlineKernelServices = new OnlineKernelServices();
        }

        [CustomAuth(Permission = "AdminUpdate")]
        public IEnumerable<OnlineKernel> GetAll()
        {
            return _onlineKernelServices.GetAllOnlineKernels();
        }

        [CustomAuth(Permission = "AdminUpdate")]
        [HttpPost]
        public ApiBoolResponseDTO Download(OnlineKernel onlineKernel)
        {
            return new ApiBoolResponseDTO() { Value = _onlineKernelServices.DownloadKernel(onlineKernel) };
        }
    
    }
}