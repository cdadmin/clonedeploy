using System.Collections.Generic;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;

namespace CloneDeploy_Services
{
    public class ClusterGroupDistributionPointServices
    {
        private readonly UnitOfWork _uow;

        public ClusterGroupDistributionPointServices()
        {
            _uow = new UnitOfWork();
        }

        public ActionResultDTO AddClusterGroupDistributionPoints(List<ClusterGroupDistributionPointEntity> listOfDps)
        {
            var actionResult = new ActionResultDTO();
            var clusterGroupDistributionPointEntity = listOfDps[0];
            if (clusterGroupDistributionPointEntity != null)
            {
                DeleteClusterGroupDistributionPoints(clusterGroupDistributionPointEntity.ClusterGroupId);
                if (clusterGroupDistributionPointEntity.DistributionPointId == -2)
                {
                    actionResult.Success = true;
                    return actionResult;
                }
            }

            var firstDp = listOfDps[0];
            if (firstDp != null)
                DeleteClusterGroupDistributionPoints(firstDp.ClusterGroupId);


            foreach (var dp in listOfDps)
                _uow.ClusterGroupDistributionPointRepository.Insert(dp);
            _uow.Save();

            actionResult.Success = true;
            return actionResult;
        }

        private bool DeleteClusterGroupDistributionPoints(int clusterGroupId)
        {
            _uow.ClusterGroupDistributionPointRepository.DeleteRange(x => x.ClusterGroupId == clusterGroupId);
            _uow.Save();
            return true;
        }
    }
}