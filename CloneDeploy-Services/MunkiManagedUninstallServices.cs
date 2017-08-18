using CloneDeploy_DataModel;
using CloneDeploy_Entities;

namespace CloneDeploy_Services
{
    public class MunkiManagedUninstallServices
    {
        private readonly UnitOfWork _uow;

        public MunkiManagedUninstallServices()
        {
            _uow = new UnitOfWork();
        }

        public MunkiManifestManagedUnInstallEntity GetManagedUnInstall(int managedUnInstallId)
        {
            return _uow.MunkiManagedUnInstallRepository.GetById(managedUnInstallId);
        }
    }
}