using System;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;

namespace CloneDeploy_Services
{
    public class CdVersionServices
    {
        private readonly UnitOfWork _uow;

        public CdVersionServices()
        {
            _uow = new UnitOfWork();
        }

        public bool FirstRunCompleted()
        {
            return Convert.ToBoolean(_uow.CdVersionRepository.GetById(1).FirstRunCompleted);
        }

        public CdVersionEntity Get(int cdVersionId)
        {
            return _uow.CdVersionRepository.GetById(cdVersionId);
        }

        public ActionResultDTO Update(CdVersionEntity cdVersion)
        {
            _uow.CdVersionRepository.Update(cdVersion, cdVersion.Id);
            _uow.Save();
            var actionResult = new ActionResultDTO();
            actionResult.Success = true;
            actionResult.Id = cdVersion.Id;
            return actionResult;
        }
    }
}