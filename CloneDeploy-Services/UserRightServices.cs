using System.Collections.Generic;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;

namespace CloneDeploy_Services
{
    public class UserRightServices
    {
          private readonly UnitOfWork _uow;

        public UserRightServices()
        {
            _uow = new UnitOfWork();
        }

        public  ActionResultDTO AddUserRights(List<UserRightEntity> listOfRights)
        {
            
                foreach (var right in listOfRights)
                    _uow.UserRightRepository.Insert(right);

                _uow.Save();
                var actionResult = new ActionResultDTO();
            actionResult.Success = true;
            return actionResult;

        }

      
    }
}