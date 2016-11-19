using System;

namespace CloneDeploy_App.BLL
{
    public class CdVersion
    {
        public static bool FirstRunCompleted()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return Convert.ToBoolean(uow.CdVersionRepository.GetById(1).FirstRunCompleted);
            }
        }

        public static Models.CdVersion Get(int cdVersionId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.CdVersionRepository.GetById(cdVersionId);
            }
        }

        public static bool Update(Models.CdVersion cdVersion)
        {
            using (var uow = new DAL.UnitOfWork())
            {

                uow.CdVersionRepository.Update(cdVersion, cdVersion.Id);
                return uow.Save();
            }
        }



    }
}