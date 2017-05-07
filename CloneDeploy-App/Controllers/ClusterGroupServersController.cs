using System.Collections.Generic;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;

namespace CloneDeploy_App.Controllers
{
    public class ClusterGroupServersController : ApiController
    {
        private readonly ClusterGroupServerServices _clusterGroupServerServices;

        public ClusterGroupServersController()
        {
            _clusterGroupServerServices = new ClusterGroupServerServices();
        }

        [CustomAuth(Permission = "AdminUpdate")]
        public ActionResultDTO Post(List<ClusterGroupServerEntity> listOfServers)
        {
            return _clusterGroupServerServices.AddClusterGroupServers(listOfServers);
        }
    }
}