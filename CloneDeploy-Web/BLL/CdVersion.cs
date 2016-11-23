using System;

namespace BLL
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

        public static CloneDeploy_Web.Models.CdVersion Get(int cdVersionId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.CdVersionRepository.GetById(cdVersionId);
            }
        }

        public static bool Update(CloneDeploy_Web.Models.CdVersion cdVersion)
        {
            using (var uow = new DAL.UnitOfWork())
            {

                uow.CdVersionRepository.Update(cdVersion, cdVersion.Id);
                return uow.Save();
            }
        }



    }
}