using System.Collections.Generic;
using System.Linq;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;

namespace CloneDeploy_Services
{
    public class BootTemplateServices
    {
        private readonly UnitOfWork _uow;

        public BootTemplateServices()
        {
            _uow = new UnitOfWork();
        }

        public ActionResultDTO AddBootTemplate(BootTemplateEntity bootTemplate)
        {
            var validationResult = ValidateTemplate(bootTemplate, true);
            var actionResult = new ActionResultDTO();
            if (validationResult.Success)
            {
                _uow.BootTemplateRepository.Insert(bootTemplate);
                _uow.Save();
                actionResult.Success = true;

                actionResult.Id = bootTemplate.Id;
            }
            else
            {
                actionResult.ErrorMessage = validationResult.ErrorMessage;
            }

            return actionResult;
        }

        public ActionResultDTO DeleteBootTemplate(int BootTemplateId)
        {
            var existingtemplate = GetBootTemplate(BootTemplateId);
            if (existingtemplate == null)
                return new ActionResultDTO {ErrorMessage = "Boot Template Not Found", Id = 0};
            var actionResult = new ActionResultDTO();
            var bootTemplate = GetBootTemplate(BootTemplateId);

            _uow.BootTemplateRepository.Delete(BootTemplateId);
            _uow.Save();
            actionResult.Success = true;
            actionResult.Id = bootTemplate.Id;

            return actionResult;
        }

        public BootTemplateEntity GetBootTemplate(int BootTemplateId)
        {
            return _uow.BootTemplateRepository.GetById(BootTemplateId);
        }

        public List<BootTemplateEntity> SearchBootTemplates(string searchString = "")
        {
            return
                _uow.BootTemplateRepository.Get(
                    s => s.Name.Contains(searchString), q => q.OrderBy(t => t.Name));
        }

        public string TotalCount()
        {
            return _uow.BootTemplateRepository.Count();
        }

        public ActionResultDTO UpdateBootTemplate(BootTemplateEntity bootTemplate)
        {
            var existingtemplate = GetBootTemplate(bootTemplate.Id);
            if (existingtemplate == null)
                return new ActionResultDTO {ErrorMessage = "Boot Template Not Found", Id = 0};
            var validationResult = ValidateTemplate(bootTemplate, false);
            var actionResult = new ActionResultDTO();
            if (validationResult.Success)
            {
                _uow.BootTemplateRepository.Update(bootTemplate, bootTemplate.Id);
                _uow.Save();
                actionResult.Success = true;
                actionResult.Id = bootTemplate.Id;
            }
            else
            {
                actionResult.ErrorMessage = validationResult.ErrorMessage;
            }

            return actionResult;
        }

        private ValidationResultDTO ValidateTemplate(BootTemplateEntity bootTemplate, bool isNewTemplate)
        {
            var validationResult = new ValidationResultDTO {Success = true};

            if (string.IsNullOrEmpty(bootTemplate.Name) || bootTemplate.Name.Contains(" "))
            {
                validationResult.Success = false;
                validationResult.ErrorMessage = "Template Name Is Not Valid";
                return validationResult;
            }

            if (isNewTemplate)
            {
                if (_uow.BootTemplateRepository.Exists(h => h.Name == bootTemplate.Name))
                {
                    validationResult.Success = false;
                    validationResult.ErrorMessage = "This Boot Template Already Exists";
                    return validationResult;
                }
            }
            else
            {
                var originalTemplate = _uow.BootTemplateRepository.GetById(bootTemplate.Id);
                if (originalTemplate.Name != bootTemplate.Name)
                {
                    if (_uow.BootTemplateRepository.Exists(h => h.Name == bootTemplate.Name))
                    {
                        validationResult.Success = false;
                        validationResult.ErrorMessage = "This Boot Template Already Exists";
                        return validationResult;
                    }
                }
            }

            return validationResult;
        }
    }
}