using System.Collections.Generic;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;

namespace CloneDeploy_Services
{
    public class UserGroupImageManagementServices
    {
        private readonly UnitOfWork _uow;

        public UserGroupImageManagementServices()
        {
            _uow = new UnitOfWork();
        }

        public ActionResultDTO AddUserGroupImageManagements(List<UserGroupImageManagementEntity> listOfImages)
        {
            foreach (var image in listOfImages)
                _uow.UserGroupImageManagementRepository.Insert(image);

            _uow.Save();
            var actionResult = new ActionResultDTO();
            actionResult.Success = true;
            return actionResult;
        }

        //check this
        public bool DeleteAllForImage(int imageId)
        {
            _uow.UserGroupImageManagementRepository.DeleteRange(x => x.ImageId == imageId);
            _uow.Save();
            return true;
        }
    }
}