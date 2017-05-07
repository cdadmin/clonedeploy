using CloneDeploy_DataModel;
using CloneDeploy_Entities;

namespace CloneDeploy_Services
{
    public class MunkiIncludedManifestServices
    {
        private readonly UnitOfWork _uow;

        public MunkiIncludedManifestServices()
        {
            _uow = new UnitOfWork();
        }


        public MunkiManifestIncludedManifestEntity GetIncludedManifest(int includedManifestId)
        {
            return _uow.MunkiIncludedManifestRepository.GetById(includedManifestId);
        }
    }
}