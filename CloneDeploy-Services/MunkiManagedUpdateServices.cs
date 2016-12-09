using CloneDeploy_DataModel;
using CloneDeploy_Entities;

namespace CloneDeploy_Services
{
    public class MunkiManagedUpdateServices
    {
        private readonly UnitOfWork _uow;

        public MunkiManagedUpdateServices()
        {
            _uow = new UnitOfWork();
        }

       

        public  MunkiManifestManagedUpdateEntity GetManagedUpdate(int managedUpdateId)
        {
            
                return _uow.MunkiManagedUpdateRepository.GetById(managedUpdateId);
            
        }

     
       

       
    }
}