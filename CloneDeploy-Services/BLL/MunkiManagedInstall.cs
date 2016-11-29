using System.Collections.Generic;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
namespace CloneDeploy_App.BLL
{
    public class MunkiManagedInstall
    {

        public static bool AddManagedInstallToTemplate(MunkiManifestManagedInstallEntity managedInstall)
        {
            using (var uow = new UnitOfWork())
            {

                if (
                    !uow.MunkiManagedInstallRepository.Exists(
                        s => s.Name == managedInstall.Name && s.ManifestTemplateId == managedInstall.ManifestTemplateId))
                    uow.MunkiManagedInstallRepository.Insert(managedInstall);
                else
                {
                    managedInstall.Id =
                        uow.MunkiManagedInstallRepository.GetFirstOrDefault(
                            s => s.Name == managedInstall.Name && s.ManifestTemplateId == managedInstall.ManifestTemplateId).Id;
                    uow.MunkiManagedInstallRepository.Update(managedInstall, managedInstall.Id);
                }


                return uow.Save();

            }

        }

        public static bool DeleteManagedInstallFromTemplate(int managedInstallId)
        {
            using (var uow = new UnitOfWork())
            {
                uow.MunkiManagedInstallRepository.Delete(managedInstallId);
                return uow.Save();
            }
        }

        public static MunkiManifestManagedInstallEntity GetManagedInstall(int managedInstallId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.MunkiManagedInstallRepository.GetById(managedInstallId);
            }
        }

        public static List<MunkiManifestManagedInstallEntity> GetAllManagedInstallsForMt(int manifestTemplateId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.MunkiManagedInstallRepository.Get(s => s.ManifestTemplateId == manifestTemplateId);
            }
        }

        public static string TotalCount(int manifestTemplateId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.MunkiManagedInstallRepository.Count(x => x.ManifestTemplateId == manifestTemplateId);
            }
        }
       

       
    }
}