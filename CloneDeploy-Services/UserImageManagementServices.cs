using System.Collections.Generic;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;

namespace CloneDeploy_Services
{
    public class UserImageManagementServices
    {
        private readonly UnitOfWork _uow;

        public UserImageManagementServices()
        {
            _uow = new UnitOfWork();
        }

        public  ActionResultDTO AddUserImageManagements(List<UserImageManagementEntity> listOfImages)
        {
           
                foreach (var image in listOfImages)
                    _uow.UserImageManagementRepository.Insert(image);
                _uow.Save();
                var actionResult = new ActionResultDTO();
            actionResult.Success = true;
            return actionResult;
            

        }

      

      
    }
}