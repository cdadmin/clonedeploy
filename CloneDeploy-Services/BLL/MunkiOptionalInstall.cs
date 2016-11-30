using System.Collections.Generic;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
namespace CloneDeploy_App.BLL
{
    public class MunkiOptionalInstall
    {

        public static bool AddOptionalInstallToTemplate(MunkiManifestOptionInstallEntity optionalInstall)
        {
            using (var uow = new UnitOfWork())
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

                uow.Save();
                return true;

            }

        }

        public static bool DeleteOptionalInstallFromTemplate(int optionalInstallId)
        {
            using (var uow = new UnitOfWork())
            {
                uow.MunkiOptionalInstallRepository.Delete(optionalInstallId);
                uow.Save();
                return true;
            }
        }

        public static MunkiManifestOptionInstallEntity GetOptionalInstall(int optionalInstallId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.MunkiOptionalInstallRepository.GetById(optionalInstallId);
            }
        }

        public static List<MunkiManifestOptionInstallEntity> GetAllOptionalInstallsForMt(int manifestTemplateId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.MunkiOptionalInstallRepository.Get(s => s.ManifestTemplateId == manifestTemplateId);
            }
        }

        public static string TotalCount(int manifestTemplateId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.MunkiOptionalInstallRepository.Count(x => x.ManifestTemplateId == manifestTemplateId);
            }
        }
       

       
    }
}