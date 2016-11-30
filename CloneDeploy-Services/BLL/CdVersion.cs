using System;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;

namespace CloneDeploy_App.BLL
{
    public class CdVersion
    {
        public static bool FirstRunCompleted()
        {
            using (var uow = new UnitOfWork())
            {
                return Convert.ToBoolean(uow.CdVersionRepository.GetById(1).FirstRunCompleted);
            }
        }

        public static CdVersionEntity Get(int cdVersionId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.CdVersionRepository.GetById(cdVersionId);
            }
        }

        public static bool Update(CdVersionEntity cdVersion)
        {
            using (var uow = new UnitOfWork())
            {

                uow.CdVersionRepository.Update(cdVersion, cdVersion.Id);
                uow.Save();
                return true;
            }
        }



    }
}