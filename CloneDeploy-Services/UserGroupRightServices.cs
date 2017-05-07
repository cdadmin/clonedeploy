using System.Collections.Generic;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;

namespace CloneDeploy_Services
{
    public class UserGroupRightServices
    {
        private readonly UnitOfWork _uow;

        public UserGroupRightServices()
        {
            _uow = new UnitOfWork();
        }

        public ActionResultDTO AddUserGroupRights(List<UserGroupRightEntity> listOfRights)
        {
            foreach (var right in listOfRights)
                _uow.UserGroupRightRepository.Insert(right);
            _uow.Save();
            var actionResult = new ActionResultDTO();
            actionResult.Success = true;
            return actionResult;
        }
    }
}