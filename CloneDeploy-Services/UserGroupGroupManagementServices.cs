using System.Collections.Generic;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;

namespace CloneDeploy_Services
{
    public class UserGroupGroupManagementServices
    {
        private readonly UnitOfWork _uow;

        public UserGroupGroupManagementServices()
        {
            _uow = new UnitOfWork();
        }

        public ActionResultDTO AddUserGroupGroupManagements(List<UserGroupGroupManagementEntity> listOfGroups)
        {
            foreach (var group in listOfGroups)
                _uow.UserGroupGroupManagementRepository.Insert(group);
            _uow.Save();
            var actionResult = new ActionResultDTO();
            actionResult.Success = true;
            return actionResult;
        }

        //check this
        public bool DeleteAllForGroup(int groupId)
        {
            _uow.UserGroupGroupManagementRepository.DeleteRange(x => x.GroupId == groupId);
            _uow.Save();
            return true;
        }
    }
}