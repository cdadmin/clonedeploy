using System.Collections.Generic;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;

namespace CloneDeploy_Services
{
    public class ClusterGroupServices
    {
        private readonly UnitOfWork _uow;

        public ClusterGroupServices()
        {
            _uow = new UnitOfWork();
        }

        public ActionResultDTO AddClusterGroup(ClusterGroupEntity clusterGroup)
        {
            var actionResult = new ActionResultDTO();
            var validationResult = ValidateClusterGroup(clusterGroup, true);
            if (validationResult.Success)
            {
                _uow.ClusterGroupRepository.Insert(clusterGroup);
                _uow.Save();
                actionResult.Success = true;
                actionResult.Id = clusterGroup.Id;
            }
            else
            {
                actionResult.ErrorMessage = validationResult.ErrorMessage;
            }

            return actionResult;
        }

        public ActionResultDTO DeleteClusterGroup(int clusterGroupId)
        {
            var clusterGroup = GetClusterGroup(clusterGroupId);
            if (clusterGroup == null) return new ActionResultDTO {ErrorMessage = "Cluster Group Not Found", Id = 0};
            _uow.ClusterGroupRepository.Delete(clusterGroupId);
            _uow.Save();
            var actionResult = new ActionResultDTO();
            actionResult.Success = true;
            actionResult.Id = clusterGroup.Id;
            return actionResult;
        }

        public List<ClusterGroupDistributionPointEntity> GetClusterDps(int clusterId)
        {
            return _uow.ClusterGroupDistributionPointRepository.Get(x => x.ClusterGroupId == clusterId);
        }

        public ClusterGroupEntity GetClusterGroup(int clusterGroupId)
        {
            return _uow.ClusterGroupRepository.GetById(clusterGroupId);
        }

        public List<ClusterGroupServerEntity> GetClusterServers(int clusterId)
        {
            return _uow.ClusterGroupServersRepository.Get(x => x.ClusterGroupId == clusterId);
        }

        public ClusterGroupEntity GetDefaultClusterGroup()
        {
            return _uow.ClusterGroupRepository.GetFirstOrDefault(x => x.Default == 1);
        }

        public List<ClusterGroupEntity> SearchClusterGroups(string searchString = "")
        {
            return _uow.ClusterGroupRepository.Get(s => s.Name.Contains(searchString));
        }

        public string TotalCount()
        {
            return _uow.ClusterGroupRepository.Count();
        }

        public ActionResultDTO UpdateClusterGroup(ClusterGroupEntity clusterGroup)
        {
            var s = GetClusterGroup(clusterGroup.Id);
            if (s == null) return new ActionResultDTO {ErrorMessage = "Cluster Group Not Found", Id = 0};
            var validationResult = ValidateClusterGroup(clusterGroup, false);
            var actionResult = new ActionResultDTO();
            if (validationResult.Success)
            {
                _uow.ClusterGroupRepository.Update(clusterGroup, clusterGroup.Id);
                _uow.Save();
                actionResult.Success = true;
                actionResult.Id = clusterGroup.Id;
            }
            else
            {
                actionResult.ErrorMessage = validationResult.ErrorMessage;
            }

            return actionResult;
        }

        private ValidationResultDTO ValidateClusterGroup(ClusterGroupEntity clusterGroup, bool isNewClusterGroup)
        {
            var validationResult = new ValidationResultDTO {Success = true};

            if (string.IsNullOrEmpty(clusterGroup.Name))
            {
                validationResult.Success = false;
                validationResult.ErrorMessage = "Cluster Group Name Is Not Valid";
                return validationResult;
            }

            if (clusterGroup.Default == 1)
            {
                var existingDefaultCluster = GetDefaultClusterGroup();
                if (existingDefaultCluster != null && existingDefaultCluster.Id != clusterGroup.Id)
                {
                    validationResult.Success = false;
                    validationResult.ErrorMessage = "Only 1 Default Cluster Group Can Exist";
                    return validationResult;
                }
            }

            if (isNewClusterGroup)
            {
                if (_uow.ClusterGroupRepository.Exists(h => h.Name == clusterGroup.Name))
                {
                    validationResult.Success = false;
                    validationResult.ErrorMessage = "This Cluster Group Already Exists";
                    return validationResult;
                }
            }
            else
            {
                var originalClusterGroup = _uow.ClusterGroupRepository.GetById(clusterGroup.Id);
                if (originalClusterGroup.Name != clusterGroup.Name)
                {
                    if (_uow.ClusterGroupRepository.Exists(h => h.Name == clusterGroup.Name))
                    {
                        validationResult.Success = false;
                        validationResult.ErrorMessage = "This Cluster Group Already Exists";
                        return validationResult;
                    }
                }
            }

            return validationResult;
        }
    }
}