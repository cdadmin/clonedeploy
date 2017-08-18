using System.Collections.Generic;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;

namespace CloneDeploy_App.Controllers
{
    public class OnlineKernelController : ApiController
    {
        private readonly OnlineKernelServices _onlineKernelServices;

        public OnlineKernelController()
        {
            _onlineKernelServices = new OnlineKernelServices();
        }

        [CustomAuth(Permission = "AdminUpdate")]
        [HttpPost]
        public ApiBoolResponseDTO Download(OnlineKernel onlineKernel)
        {
            return new ApiBoolResponseDTO {Value = _onlineKernelServices.DownloadKernel(onlineKernel)};
        }

        [CustomAuth(Permission = "AdminUpdate")]
        public IEnumerable<OnlineKernel> Get()
        {
            return _onlineKernelServices.GetAllOnlineKernels();
        }
    }
}