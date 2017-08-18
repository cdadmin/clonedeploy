using System.Collections.Generic;
using System.Linq;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;

namespace CloneDeploy_Services
{
    public class SysprepTagServices
    {
        private readonly UnitOfWork _uow;

        public SysprepTagServices()
        {
            _uow = new UnitOfWork();
        }

        public ActionResultDTO AddSysprepTag(SysprepTagEntity sysprepTag)
        {
            var validationResult = ValidateSysprepTag(sysprepTag, true);
            var actionResult = new ActionResultDTO();
            if (validationResult.Success)
            {
                _uow.SysprepTagRepository.Insert(sysprepTag);
                _uow.Save();
                actionResult.Success = true;
                actionResult.Id = sysprepTag.Id;
            }
            else
            {
                actionResult.ErrorMessage = validationResult.ErrorMessage;
            }

            return actionResult;
        }

        public ActionResultDTO DeleteSysprepTag(int sysprepTagId)
        {
            var tag = GetSysprepTag(sysprepTagId);
            if (tag == null) return new ActionResultDTO {ErrorMessage = "Sysprep Tag Not Found", Id = 0};
            _uow.SysprepTagRepository.Delete(sysprepTagId);
            _uow.Save();
            var actionResult = new ActionResultDTO();
            actionResult.Success = true;
            actionResult.Id = tag.Id;
            return actionResult;
        }

        public SysprepTagEntity GetSysprepTag(int sysprepTagId)
        {
            return _uow.SysprepTagRepository.GetById(sysprepTagId);
        }

        public List<SysprepTagEntity> SearchSysprepTags(string searchString = "")
        {
            return
                _uow.SysprepTagRepository.Get(
                    s => s.Name.Contains(searchString) || s.OpeningTag.Contains(searchString),
                    q => q.OrderBy(s => s.OpeningTag));
        }

        public string TotalCount()
        {
            return _uow.SysprepTagRepository.Count();
        }

        public ActionResultDTO UpdateSysprepTag(SysprepTagEntity sysprepTag)
        {
            var tag = GetSysprepTag(sysprepTag.Id);
            if (tag == null) return new ActionResultDTO {ErrorMessage = "Sysprep Tag Not Found", Id = 0};
            var validationResult = ValidateSysprepTag(sysprepTag, false);
            var actionResult = new ActionResultDTO();
            if (validationResult.Success)
            {
                _uow.SysprepTagRepository.Update(sysprepTag, sysprepTag.Id);
                _uow.Save();
                actionResult.Success = true;
                actionResult.Id = sysprepTag.Id;
            }
            else
            {
                actionResult.ErrorMessage = validationResult.ErrorMessage;
            }

            return actionResult;
        }

        private ValidationResultDTO ValidateSysprepTag(SysprepTagEntity sysprepTag, bool isNewSysprepTag)
        {
            var validationResult = new ValidationResultDTO {Success = true};

            if (string.IsNullOrEmpty(sysprepTag.Name) || !sysprepTag.Name.All(c => char.IsLetterOrDigit(c) || c == '_'))
            {
                validationResult.Success = false;
                validationResult.ErrorMessage = "Sysprep Tag Name Is Not Valid";
                return validationResult;
            }

            if (isNewSysprepTag)
            {
                if (_uow.SysprepTagRepository.Exists(h => h.Name == sysprepTag.Name))
                {
                    validationResult.Success = false;
                    validationResult.ErrorMessage = "This Sysprep Tag Already Exists";
                    return validationResult;
                }
            }
            else
            {
                var originalSysprepTag = _uow.SysprepTagRepository.GetById(sysprepTag.Id);
                if (originalSysprepTag.Name != sysprepTag.Name)
                {
                    if (_uow.SysprepTagRepository.Exists(h => h.Name == sysprepTag.Name))
                    {
                        validationResult.Success = false;
                        validationResult.ErrorMessage = "This Sysprep Tag Already Exists";
                        return validationResult;
                    }
                }
            }

            return validationResult;
        }
    }
}