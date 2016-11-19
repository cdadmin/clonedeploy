using System.Collections.Generic;

namespace CloneDeploy_App.BLL
{
    public class MunkiManagedUpdate
    {

        public static bool AddManagedUpdateToTemplate(Models.MunkiManifestManagedUpdate managedUpdate)
        {
            using (var uow = new DAL.UnitOfWork())
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


                return uow.Save();

            }

        }

        public static bool DeleteManagedUpdateFromTemplate(int managedUpdateId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.MunkiManagedUpdateRepository.Delete(managedUpdateId);
                return uow.Save();
            }
        }

        public static Models.MunkiManifestManagedUpdate GetManagedUpdate(int managedUpdateId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.MunkiManagedUpdateRepository.GetById(managedUpdateId);
            }
        }

        public static  List<Models.MunkiManifestManagedUpdate> GetAllManagedUpdatesForMt(int manifestTemplateId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.MunkiManagedUpdateRepository.Get(s => s.ManifestTemplateId == manifestTemplateId);
            }
        }

        public static string TotalCount(int manifestTemplateId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.MunkiManagedUpdateRepository.Count(x => x.ManifestTemplateId == manifestTemplateId);
            }
        }
       

       
    }
}