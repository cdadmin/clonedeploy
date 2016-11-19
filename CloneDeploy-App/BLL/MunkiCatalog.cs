using System.Collections.Generic;

namespace CloneDeploy_App.BLL
{
    public class MunkiCatalog
    {

        public static bool AddCatalogToTemplate(Models.MunkiManifestCatalog catalog)
        {
            using (var uow = new DAL.UnitOfWork())
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
            using (var uow = new DAL.UnitOfWork())
            {
                uow.MunkiCatalogRepository.Delete(catalogId);
                return uow.Save();
            }
        }

        public static Models.MunkiManifestCatalog GetCatalog(int catalogId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.MunkiCatalogRepository.GetById(catalogId);
            }
        }

        public static  List<Models.MunkiManifestCatalog> GetAllCatalogsForMt(int manifestTemplateId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.MunkiCatalogRepository.Get(s => s.ManifestTemplateId == manifestTemplateId);
            }
        }

        public static string TotalCount(int manifestTemplateId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.MunkiCatalogRepository.Count(x => x.ManifestTemplateId == manifestTemplateId);
            }
        }
       

       
    }
}