using System.Collections.Generic;
using CloneDeploy_Web.Models;

namespace BLL
{
    public class MunkiManagedUpdate
    {

        //moved
        public static bool AddManagedUpdateToTemplate(MunkiManifestManagedUpdate managedUpdate)
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

        //moved
        public static bool DeleteManagedUpdateFromTemplate(int managedUpdateId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.MunkiManagedUpdateRepository.Delete(managedUpdateId);
                return uow.Save();
            }
        }

        //moved
        public static MunkiManifestManagedUpdate GetManagedUpdate(int managedUpdateId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.MunkiManagedUpdateRepository.GetById(managedUpdateId);
            }
        }

        //moved
        public static  List<MunkiManifestManagedUpdate> GetAllManagedUpdatesForMt(int manifestTemplateId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.MunkiManagedUpdateRepository.Get(s => s.ManifestTemplateId == manifestTemplateId);
            }
        }

        //moved
        public static string TotalCount(int manifestTemplateId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.MunkiManagedUpdateRepository.Count(x => x.ManifestTemplateId == manifestTemplateId);
            }
        }
       

       
    }
}