using System.Collections.Generic;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;

namespace CloneDeploy_Services
{
    public class MunkiOptionalInstallServices
    {
         private readonly UnitOfWork _uow;

        public MunkiOptionalInstallServices()
        {
            _uow = new UnitOfWork();
        }

       

        public  MunkiManifestOptionInstallEntity GetOptionalInstall(int optionalInstallId)
        {
            
                return _uow.MunkiOptionalInstallRepository.GetById(optionalInstallId);
            
        }

      
       

       
    }
}