using System.Net;
using System.Net.Http;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;


namespace CloneDeploy_App.Controllers
{
    public class ComputerProxyReservationController : ApiController
    {
        private readonly ComputerProxyReservationServices _computerProxyReservationServices;

        public ComputerProxyReservationController()
        {
            _computerProxyReservationServices = new ComputerProxyReservationServices();
        }

        [CustomAuth(Permission = "ComputerSearch")]
        public ActionResultDTO Post(ComputerProxyReservationEntity computerProxyReservation)
        {
        
            var result = _computerProxyReservationServices.UpdateComputerProxyReservation(computerProxyReservation);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }
    }
}