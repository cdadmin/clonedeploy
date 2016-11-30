using System.Collections.Generic;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
namespace CloneDeploy_App.BLL
{
    public class MunkiIncludedManifest
    {

        public static bool AddIncludedManifestToTemplate(MunkiManifestIncludedManifestEntity includedManifest)
        {
            using (var uow = new UnitOfWork())
            {

                if (
                    !uow.MunkiIncludedManifestRepository.Exists(
                        s => s.Name == includedManifest.Name && s.ManifestTemplateId == includedManifest.ManifestTemplateId))
                    uow.MunkiIncludedManifestRepository.Insert(includedManifest);
                else
                {
                    includedManifest.Id =
                        uow.MunkiIncludedManifestRepository.GetFirstOrDefault(
                            s => s.Name == includedManifest.Name && s.ManifestTemplateId == includedManifest.ManifestTemplateId).Id;
                    uow.MunkiIncludedManifestRepository.Update(includedManifest, includedManifest.Id);
                }

                uow.Save();
                return true;

            }

        }

        public static bool DeleteIncludedManifestFromTemplate(int includedManifestId)
        {
            using (var uow = new UnitOfWork())
            {
                uow.MunkiIncludedManifestRepository.Delete(includedManifestId);
                uow.Save();
                return true;
            }
        }

        public static MunkiManifestIncludedManifestEntity GetIncludedManifest(int includedManifestId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.MunkiIncludedManifestRepository.GetById(includedManifestId);
            }
        }

        public static List<MunkiManifestIncludedManifestEntity> GetAllIncludedManifestsForMt(int manifestTemplateId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.MunkiIncludedManifestRepository.Get(s => s.ManifestTemplateId == manifestTemplateId);
            }
        }

        public static string TotalCount(int manifestTemplateId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.MunkiIncludedManifestRepository.Count(x => x.ManifestTemplateId == manifestTemplateId);
            }
        }
       

       
    }
}