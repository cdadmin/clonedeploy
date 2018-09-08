using System.Collections.Generic;
using CloneDeploy_Common.Enum;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;

namespace CloneDeploy_Services
{
    public class ImageProfileTemplateServices
    {
        private readonly UnitOfWork _uow;

        public ImageProfileTemplateServices()
        {
            _uow = new UnitOfWork();
        }

        public ImageProfileTemplate GetTemplate(EnumProfileTemplate.TemplateType templateType)
        {
            return _uow.ImageProfileTemplateRepository.GetFirstOrDefault(x => x.TemplateType == templateType);
        }

        public ActionResultDTO UpdateTemplate(ImageProfileTemplate template)
        {
            var actionResult = new ActionResultDTO();
            var existingTemplate = GetTemplate(template.TemplateType);
            if (existingTemplate == null)
                return new ActionResultDTO {ErrorMessage = "Template Not Found", Id = 0};

            template.Id = existingTemplate.Id;
            _uow.ImageProfileTemplateRepository.Update(template, existingTemplate.Id);
            _uow.Save();
            actionResult.Success = true;
            actionResult.Id = existingTemplate.Id;
            return actionResult;
        }
    }
}