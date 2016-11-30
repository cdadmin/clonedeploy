using System.Collections.Generic;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
namespace CloneDeploy_App.BLL
{
    public class MunkiManagedUpdate
    {

        public static bool AddManagedUpdateToTemplate(MunkiManifestManagedUpdateEntity managedUpdate)
        {
            using (var uow = new UnitOfWork())
            {

                if (
                    !uow.MunkiManagedUpdateRepository.Exists(
                        s => s.Name == managedUpdate.Name && s.ManifestTemplateId == managedUpdate.ManifestTemplateId))
                    uow.MunkiManagedUpdateRepository.Insert(managedUpdate);
                else
                {
                    managedUpdate.Id =
                        uow.MunkiManagedUpdateRepository.GetFirstOrDefault(
                            s => s.Name == managedUpdate.Name && s.ManifestTemplateId == managedUpdate.ManifestTemplateId).Id;
                    uow.MunkiManagedUpdateRepository.Update(managedUpdate, managedUpdate.Id);
                }

                uow.Save();
                return true;

            }

        }

        public static bool DeleteManagedUpdateFromTemplate(int managedUpdateId)
        {
            using (var uow = new UnitOfWork())
            {
                uow.MunkiManagedUpdateRepository.Delete(managedUpdateId);
                uow.Save();
                return true;
            }
        }

        public static MunkiManifestManagedUpdateEntity GetManagedUpdate(int managedUpdateId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.MunkiManagedUpdateRepository.GetById(managedUpdateId);
            }
        }

        public static List<MunkiManifestManagedUpdateEntity> GetAllManagedUpdatesForMt(int manifestTemplateId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.MunkiManagedUpdateRepository.Get(s => s.ManifestTemplateId == manifestTemplateId);
            }
        }

        public static string TotalCount(int manifestTemplateId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.MunkiManagedUpdateRepository.Count(x => x.ManifestTemplateId == manifestTemplateId);
            }
        }
       

       
    }
}