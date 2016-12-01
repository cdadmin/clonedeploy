using System.Collections.Generic;
using System.Linq;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;

namespace CloneDeploy_Services
{
    public class ImageProfileSysprepTagServices
    {
        private readonly UnitOfWork _uow;

        public ImageProfileSysprepTagServices()
        {
            _uow = new UnitOfWork();
        }

        public  ActionResultDTO AddImageProfileSysprepTag(ImageProfileSysprepTagEntity imageProfileSysprepTag)
        {
           
                _uow.ImageProfileSysprepRepository.Insert(imageProfileSysprepTag);
                _uow.Save();
                var actionResult = new ActionResultDTO();
            actionResult.Success = true;
            actionResult.Id = imageProfileSysprepTag.Id;
            return actionResult;

        }

      

       
    }
}