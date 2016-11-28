using System.Collections.Generic;
using CloneDeploy_Web.Models;

namespace BLL
{
    public class MunkiManagedInstall
    {
        //moved
        public static bool AddManagedInstallToTemplate(MunkiManifestManagedInstall managedInstall)
        {
            using (var uow = new DAL.UnitOfWork())
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

        //moved
        public static bool DeleteManagedInstallFromTemplate(int managedInstallId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.MunkiManagedInstallRepository.Delete(managedInstallId);
                return uow.Save();
            }
        }

        //moved
        public static MunkiManifestManagedInstall GetManagedInstall(int managedInstallId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.MunkiManagedInstallRepository.GetById(managedInstallId);
            }
        }

        //moved
        public static  List<MunkiManifestManagedInstall> GetAllManagedInstallsForMt(int manifestTemplateId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.MunkiManagedInstallRepository.Get(s => s.ManifestTemplateId == manifestTemplateId);
            }
        }

        //moved
        public static string TotalCount(int manifestTemplateId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.MunkiManagedInstallRepository.Count(x => x.ManifestTemplateId == manifestTemplateId);
            }
        }
       

       
    }
}