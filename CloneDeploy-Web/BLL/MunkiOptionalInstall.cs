using System.Collections.Generic;
using CloneDeploy_Web.Models;

namespace BLL
{
    public class MunkiOptionalInstall
    {

        public static bool AddOptionalInstallToTemplate(MunkiManifestOptionInstall optionalInstall)
        {
            using (var uow = new DAL.UnitOfWork())
            {

                if (
                    !uow.MunkiOptionalInstallRepository.Exists(
                        s => s.Name == optionalInstall.Name && s.ManifestTemplateId == optionalInstall.ManifestTemplateId))
                    uow.MunkiOptionalInstallRepository.Insert(optionalInstall);
                else
                {
                    optionalInstall.Id =
                        uow.MunkiOptionalInstallRepository.GetFirstOrDefault(
                            s => s.Name == optionalInstall.Name && s.ManifestTemplateId == optionalInstall.ManifestTemplateId).Id;
                    uow.MunkiOptionalInstallRepository.Update(optionalInstall, optionalInstall.Id);
                }


                return uow.Save();

            }

        }

        public static bool DeleteOptionalInstallFromTemplate(int optionalInstallId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.MunkiOptionalInstallRepository.Delete(optionalInstallId);
                return uow.Save();
            }
        }

        public static MunkiManifestOptionInstall GetOptionalInstall(int optionalInstallId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.MunkiOptionalInstallRepository.GetById(optionalInstallId);
            }
        }

        public static  List<MunkiManifestOptionInstall> GetAllOptionalInstallsForMt(int manifestTemplateId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.MunkiOptionalInstallRepository.Get(s => s.ManifestTemplateId == manifestTemplateId);
            }
        }

        public static string TotalCount(int manifestTemplateId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.MunkiOptionalInstallRepository.Count(x => x.ManifestTemplateId == manifestTemplateId);
            }
        }
       

       
    }
}