using System.Collections.Generic;
using CloneDeploy_Web.Models;

namespace BLL
{
    public class MunkiManagedUninstall
    {

        public static bool AddManagedUnInstallToTemplate(MunkiManifestManagedUnInstall managedUnInstall)
        {
            using (var uow = new DAL.UnitOfWork())
            {

                if (
                    !uow.MunkiManagedUnInstallRepository.Exists(
                        s => s.Name == managedUnInstall.Name && s.ManifestTemplateId == managedUnInstall.ManifestTemplateId))
                    uow.MunkiManagedUnInstallRepository.Insert(managedUnInstall);
                else
                {
                    managedUnInstall.Id =
                        uow.MunkiManagedUnInstallRepository.GetFirstOrDefault(
                            s => s.Name == managedUnInstall.Name && s.ManifestTemplateId == managedUnInstall.ManifestTemplateId).Id;
                    uow.MunkiManagedUnInstallRepository.Update(managedUnInstall, managedUnInstall.Id);
                }


                return uow.Save();

            }

        }

        public static bool DeleteManagedUnInstallFromTemplate(int managedUnInstallId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.MunkiManagedUnInstallRepository.Delete(managedUnInstallId);
                return uow.Save();
            }
        }

        public static MunkiManifestManagedUnInstall GetManagedUnInstall(int managedUnInstallId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.MunkiManagedUnInstallRepository.GetById(managedUnInstallId);
            }
        }

        public static  List<MunkiManifestManagedUnInstall> GetAllManagedUnInstallsForMt(int manifestTemplateId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.MunkiManagedUnInstallRepository.Get(s => s.ManifestTemplateId == manifestTemplateId);
            }
        }

        public static string TotalCount(int manifestTemplateId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.MunkiManagedUnInstallRepository.Count(x => x.ManifestTemplateId == manifestTemplateId);
            }
        }
       

       
    }
}