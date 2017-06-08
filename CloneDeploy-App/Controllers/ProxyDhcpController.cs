using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Entities.DTOs.ClientImaging;
using CloneDeploy_Services;
using CloneDeploy_Services.Workflows;

namespace CloneDeploy_App.Controllers
{
    [EnableCorsAttribute("*","*","*")]
    public class ProxyDhcpController : ApiController
    {
        private readonly ProxyDhcpServices _proxyDhcpServices;

        public ProxyDhcpController()
        {
            _proxyDhcpServices = new ProxyDhcpServices();
        }

        [HttpGet]
        public TftpServerDTO GetAllTftpServers()
        {
            var allTftpServers = _proxyDhcpServices.GetAllTftpServers();
            if (allTftpServers == null)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return allTftpServers;
        }

        [HttpGet]
        public TftpServerDTO GetComputerTftpServers(string mac)
        {
            var computerTftpServers = _proxyDhcpServices.GetComputerTftpServers(mac);
            if (computerTftpServers == null)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return computerTftpServers;
        }

        [HttpGet]
        public ProxyReservationDTO GetProxyReservation(string mac)
        {
            var proxyReservation = _proxyDhcpServices.GetProxyReservation(mac);
            if (proxyReservation == null)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return proxyReservation;
        }

        [HttpGet]
        public ApiBoolResponseDTO Test()
        {
            return new ApiBoolResponseDTO {Value = true};
        }

        [HttpGet]
        public AppleVendorDTO GetAppleVendorString(string ip)
        {
            return new CreateAppleVendorString().Execute(ip);
        }
    }
}