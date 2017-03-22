using System.Collections.Generic;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;


namespace CloneDeploy_App.Controllers
{
    public class ClusterGroupDistributionPointsController: ApiController
    {
        private readonly ClusterGroupDistributionPointServices _clusterGroupDistributionPointsServices;

        public ClusterGroupDistributionPointsController()
        {
            _clusterGroupDistributionPointsServices = new ClusterGroupDistributionPointServices();
        }

        [CustomAuth(Permission = "AdminUpdate")]
        public ActionResultDTO Post(List<ClusterGroupDistributionPointEntity> listOfDps)
        {
            return _clusterGroupDistributionPointsServices.AddClusterGroupDistributionPoints(listOfDps);
        }

        
    }
}