using System.Collections.Generic;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
namespace CloneDeploy_App.BLL
{
    public class MunkiCatalog
    {

        public static bool AddCatalogToTemplate(MunkiManifestCatalogEntity catalog)
        {
            using (var uow = new UnitOfWork())
            {

                if (
                    !uow.MunkiCatalogRepository.Exists(
                        s => s.Name == catalog.Name && s.ManifestTemplateId == catalog.ManifestTemplateId))
                    uow.MunkiCatalogRepository.Insert(catalog);
                else
                {
                    catalog.Id =
                        uow.MunkiCatalogRepository.GetFirstOrDefault(
                            s => s.Name == catalog.Name && s.ManifestTemplateId == catalog.ManifestTemplateId).Id;
                    uow.MunkiCatalogRepository.Update(catalog, catalog.Id);
                }


                return uow.Save();

            }

        }

        public static bool DeleteCatalogFromTemplate(int catalogId)
        {
            using (var uow = new UnitOfWork())
            {
                uow.MunkiCatalogRepository.Delete(catalogId);
                return uow.Save();
            }
        }

        public static MunkiManifestCatalogEntity GetCatalog(int catalogId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.MunkiCatalogRepository.GetById(catalogId);
            }
        }

        public static List<MunkiManifestCatalogEntity> GetAllCatalogsForMt(int manifestTemplateId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.MunkiCatalogRepository.Get(s => s.ManifestTemplateId == manifestTemplateId);
            }
        }

        public static string TotalCount(int manifestTemplateId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.MunkiCatalogRepository.Count(x => x.ManifestTemplateId == manifestTemplateId);
            }
        }
       

       
    }
}