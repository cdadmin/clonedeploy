using System.Collections.Generic;
using CloneDeploy_Web.Models;

namespace BLL
{
    public class MunkiCatalog
    {
        //moved
        public static bool AddCatalogToTemplate(MunkiManifestCatalog catalog)
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

        //moved
        public static bool DeleteCatalogFromTemplate(int catalogId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.MunkiCatalogRepository.Delete(catalogId);
                return uow.Save();
            }
        }

        //moved
        public static MunkiManifestCatalog GetCatalog(int catalogId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.MunkiCatalogRepository.GetById(catalogId);
            }
        }

        //moved
        public static  List<MunkiManifestCatalog> GetAllCatalogsForMt(int manifestTemplateId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.MunkiCatalogRepository.Get(s => s.ManifestTemplateId == manifestTemplateId);
            }
        }

        //moved
        public static string TotalCount(int manifestTemplateId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.MunkiCatalogRepository.Count(x => x.ManifestTemplateId == manifestTemplateId);
            }
        }
       

       
    }
}