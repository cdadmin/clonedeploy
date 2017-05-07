using CloneDeploy_DataModel;
using CloneDeploy_Entities;

namespace CloneDeploy_Services
{
    public class MunkiCatalogServices
    {
        private readonly UnitOfWork _uow;

        public MunkiCatalogServices()
        {
            _uow = new UnitOfWork();
        }


        public MunkiManifestCatalogEntity GetCatalog(int catalogId)
        {
            return _uow.MunkiCatalogRepository.GetById(catalogId);
        }
    }
}