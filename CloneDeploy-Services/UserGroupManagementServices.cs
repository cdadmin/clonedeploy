using System.Collections.Generic;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;

namespace CloneDeploy_Services
{
    public class UserGroupManagementServices
    {
        private readonly UnitOfWork _uow;

        public UserGroupManagementServices()
        {
            _uow = new UnitOfWork();
        }

        public ActionResultDTO AddUserGroupManagements(List<UserGroupManagementEntity> listOfGroups)
        {
            foreach (var group in listOfGroups)
                _uow.UserGroupManagementRepository.Insert(group);
            _uow.Save();
            var actionResult = new ActionResultDTO();
            actionResult.Success = true;
            return actionResult;
        }
    }
}