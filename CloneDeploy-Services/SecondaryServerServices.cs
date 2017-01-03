using System.Collections.Generic;
using System.Linq;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;

namespace CloneDeploy_Services
{
    public class SecondaryServerServices
    {
        private readonly UnitOfWork _uow;

        public SecondaryServerServices()
        {
            _uow = new UnitOfWork();
        }

        public  ActionResultDTO AddSecondaryServer(SecondaryServerEntity secondaryServer)
        {
            var actionResult = new ActionResultDTO();
                var validationResult = ValidateSecondaryServer(secondaryServer, true);
                if (validationResult.Success)
                {
                    _uow.SecondaryServerRepository.Insert(secondaryServer);
                    _uow.Save();
                    actionResult.Success = true;
                    actionResult.Id = secondaryServer.Id;
                }

            return actionResult;

        }

        public  string TotalCount()
        {
            
                return _uow.SecondaryServerRepository.Count();
            
        }

        public ActionResultDTO DeleteSecondaryServer(int secondaryServerId)
        {
            var secondaryServer = GetSecondaryServer(secondaryServerId);
            if (secondaryServer == null) return new ActionResultDTO() { ErrorMessage = "Secondary Server Not Found", Id = 0 };
                _uow.SecondaryServerRepository.Delete(secondaryServerId);
                _uow.Save();
                var actionResult = new ActionResultDTO();
                actionResult.Success = true;
                actionResult.Id = secondaryServer.Id;
                return actionResult;
            
        }

        public  SecondaryServerEntity GetSecondaryServer(int secondaryServerId)
        {
            
                return _uow.SecondaryServerRepository.GetById(secondaryServerId);
            
        }

        public  List<SecondaryServerEntity> SearchSecondaryServers(string searchString = "")
        {
            
                return _uow.SecondaryServerRepository.Get(s => s.Name.Contains(searchString));
            
        }

        public  ActionResultDTO UpdateSecondaryServer(SecondaryServerEntity secondaryServer)
        {
            var s = GetSecondaryServer(secondaryServer.Id);
            if (s == null) return new ActionResultDTO() { ErrorMessage = "Secondary Server Not Found", Id = 0 };
                var validationResult = ValidateSecondaryServer(secondaryServer, false);
            var actionResult = new ActionResultDTO();
                if (validationResult.Success)
                {
                    _uow.SecondaryServerRepository.Update(secondaryServer, secondaryServer.Id);
                    _uow.Save();
                   actionResult.Success = true;
                    actionResult.Id = secondaryServer.Id;
                    
                }
                else
                {
                    actionResult.ErrorMessage = validationResult.ErrorMessage;
                }

            return actionResult;

        }

        private ValidationResultDTO ValidateSecondaryServer(SecondaryServerEntity secondaryServer, bool isNewSecondaryServer)
        {
            var validationResult = new ValidationResultDTO() { Success = true };

            if (string.IsNullOrEmpty(secondaryServer.Name) || !secondaryServer.Name.All(c => char.IsLetterOrDigit(c) || c == '_'))
            {
                validationResult.Success = false;
                validationResult.ErrorMessage = "Secondary Server Name Is Not Valid";
                return validationResult;
            }

            if (isNewSecondaryServer)
            {
               
                    if (_uow.SecondaryServerRepository.Exists(h => h.Name == secondaryServer.Name))
                    {
                        validationResult.Success = false;
                        validationResult.ErrorMessage = "This Secondary Server Already Exists";
                        return validationResult;
                    }
                
            }
            else
            {
               
                    var originalSecondaryServer = _uow.SecondaryServerRepository.GetById(secondaryServer.Id);
                    if (originalSecondaryServer.Name != secondaryServer.Name)
                    {
                        if (_uow.SecondaryServerRepository.Exists(h => h.Name == secondaryServer.Name))
                        {
                            validationResult.Success = false;
                            validationResult.ErrorMessage = "This Secondary Server Already Exists";
                            return validationResult;
                        }
                    }
                
            }

            return validationResult;
        }
      
    }
}