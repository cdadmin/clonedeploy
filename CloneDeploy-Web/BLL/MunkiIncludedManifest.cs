using System.Collections.Generic;
using CloneDeploy_Web.Models;

namespace BLL
{
    public class MunkiIncludedManifest
    {

        public static bool AddIncludedManifestToTemplate(MunkiManifestIncludedManifest includedManifest)
        {
            using (var uow = new DAL.UnitOfWork())
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


                return uow.Save();

            }

        }

        public static bool DeleteIncludedManifestFromTemplate(int includedManifestId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.MunkiIncludedManifestRepository.Delete(includedManifestId);
                return uow.Save();
            }
        }

        public static MunkiManifestIncludedManifest GetIncludedManifest(int includedManifestId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.MunkiIncludedManifestRepository.GetById(includedManifestId);
            }
        }

        public static  List<MunkiManifestIncludedManifest> GetAllIncludedManifestsForMt(int manifestTemplateId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.MunkiIncludedManifestRepository.Get(s => s.ManifestTemplateId == manifestTemplateId);
            }
        }

        public static string TotalCount(int manifestTemplateId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.MunkiIncludedManifestRepository.Count(x => x.ManifestTemplateId == manifestTemplateId);
            }
        }
       

       
    }
}