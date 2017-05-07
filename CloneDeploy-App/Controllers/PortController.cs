using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;

namespace CloneDeploy_App.Controllers
{
    public class PortController : ApiController
    {
        private readonly PortServices _portServices;

        public PortController()
        {
            _portServices = new PortServices();
        }


        [CustomAuth(Permission = "AdminUpdate")]
        public ApiBoolResponseDTO Post(PortEntity port)
        {
            return new ApiBoolResponseDTO {Value = _portServices.AddPort(port)};
        }
    }
}