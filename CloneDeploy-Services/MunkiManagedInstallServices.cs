using CloneDeploy_DataModel;
using CloneDeploy_Entities;

namespace CloneDeploy_Services
{
    public class MunkiManagedInstallServices
    {
        private readonly UnitOfWork _uow;

        public MunkiManagedInstallServices()
        {
            _uow = new UnitOfWork();
        }

       

        public  MunkiManifestManagedInstallEntity GetManagedInstall(int managedInstallId)
        {
            
                return _uow.MunkiManagedInstallRepository.GetById(managedInstallId);
            
        }

      
       

       
    }
}