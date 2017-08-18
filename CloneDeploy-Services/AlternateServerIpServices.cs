using System.Collections.Generic;
using System.Linq;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;

namespace CloneDeploy_Services
{
    public class AlternateServerIpServices
    {
        private readonly UnitOfWork _uow;

        public AlternateServerIpServices()
        {
            _uow = new UnitOfWork();
        }

        public ActionResultDTO AddAlternateServerIp(AlternateServerIpEntity alternateServerIp)
        {
            var validationResult = ValidateAlternateServerIp(alternateServerIp, true);
            var actionResult = new ActionResultDTO();
            if (validationResult.Success)
            {
                _uow.AlternateServerIpRepository.Insert(alternateServerIp);
                _uow.Save();
                actionResult.Success = true;
                actionResult.Id = alternateServerIp.Id;
            }
            else
            {
                actionResult.ErrorMessage = validationResult.ErrorMessage;
            }

            return actionResult;
        }

        public ActionResultDTO DeleteAlternateServerIp(int alternateServerIpId)
        {
            var alternateServerIp = GetAlternateServerIp(alternateServerIpId);
            if (alternateServerIp == null) return new ActionResultDTO {ErrorMessage = "Alternate Server Ip Not Found", Id = 0};
            _uow.AlternateServerIpRepository.Delete(alternateServerIpId);
            _uow.Save();
            var actionResult = new ActionResultDTO();
            actionResult.Success = true;
            actionResult.Id = alternateServerIp.Id;
            return actionResult;
        }

        public List<AlternateServerIpEntity> GetAll()
        {
            return _uow.AlternateServerIpRepository.Get();

        }
        public AlternateServerIpEntity GetAlternateServerIp(int alternateServerIpId)
        {
            return _uow.AlternateServerIpRepository.GetById(alternateServerIpId);
        }

        public string TotalCount()
        {
            return _uow.AlternateServerIpRepository.Count();
        }

        public ActionResultDTO UpdateAlternateServerIp(AlternateServerIpEntity alternateServerIp)
        {
            var r = GetAlternateServerIp(alternateServerIp.Id);
            if (r == null) return new ActionResultDTO {ErrorMessage = "Alternate Server Ip Not Found", Id = 0};

            var validationResult = ValidateAlternateServerIp(alternateServerIp, false);
            var actionResult = new ActionResultDTO();
            if (validationResult.Success)
            {
                _uow.AlternateServerIpRepository.Update(alternateServerIp, alternateServerIp.Id);
                _uow.Save();

                actionResult.Success = true;
                actionResult.Id = alternateServerIp.Id;
            }

            return actionResult;
        }

        private ValidationResultDTO ValidateAlternateServerIp(AlternateServerIpEntity alternateServerIp, bool isNewAlternateServerIp)
        {
            var validationResult = new ValidationResultDTO {Success = true};

            if (!alternateServerIp.ApiUrl.Trim().EndsWith("/"))
                alternateServerIp.ApiUrl += "/";

            if (string.IsNullOrEmpty(alternateServerIp.Ip) || (string.IsNullOrEmpty(alternateServerIp.ApiUrl)))
            {
                validationResult.Success = false;
                validationResult.ErrorMessage = "Empty Values Are Not Valid";
                return validationResult;
            }

            if (isNewAlternateServerIp)
            {
                if (_uow.AlternateServerIpRepository.Exists(h => h.Ip == alternateServerIp.Ip))
                {
                    validationResult.Success = false;
                    validationResult.ErrorMessage = "This Server Ip Already Exists";
                    return validationResult;
                }
            }
            else
            {
                var originalAlternateServerIp = _uow.AlternateServerIpRepository.GetById(alternateServerIp.Id);
                if (originalAlternateServerIp.Ip != alternateServerIp.Ip)
                {
                    if (_uow.AlternateServerIpRepository.Exists(h => h.Ip == alternateServerIp.Ip))
                    {
                        validationResult.Success = false;
                        validationResult.ErrorMessage = "This Server Ip Already Exists";
                        return validationResult;
                    }
                }
            }

            return validationResult;
        }
    }
}