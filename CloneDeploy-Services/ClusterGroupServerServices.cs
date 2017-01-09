using System.Collections.Generic;
using System.Linq;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;

namespace CloneDeploy_Services
{
    public  class ClusterGroupServerServices
    {
         private readonly UnitOfWork _uow;

        public ClusterGroupServerServices()
        {
            _uow = new UnitOfWork();
        }

        public ActionResultDTO AddClusterGroupServers(List<ClusterGroupServerEntity> listOfServers)
        {
            var actionResult = new ActionResultDTO();
            var clusterGroupServerEntity = listOfServers[0];
            if (clusterGroupServerEntity != null)
            {
                DeleteClusterGroupServers(clusterGroupServerEntity.ClusterGroupId);
                if (clusterGroupServerEntity.SecondaryServerId == -2)
                {
                    actionResult.Success = true;
                    return actionResult;
                }
            }
            foreach (var server in listOfServers)
                _uow.ClusterGroupServersRepository.Insert(server);
            _uow.Save();
            
            actionResult.Success = true;
            return actionResult;

        }

        private bool DeleteClusterGroupServers(int clusterGroupId)
        {
            _uow.ClusterGroupServersRepository.DeleteRange(x => x.ClusterGroupId == clusterGroupId);
            _uow.Save();
            return true;
        }
    }
}