using System.Collections.Generic;
using System.Linq;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using Newtonsoft.Json;

namespace CloneDeploy_App.BLL
{
    public class BootTemplate
    {

        public static ActionResultEntity AddBootTemplate(BootTemplateEntity bootTemplate)
        {
            using (var uow = new UnitOfWork())
            {
                var validationResult = ValidateTemplate(bootTemplate, true);
                if (validationResult.Success)
                {
                    uow.BootTemplateRepository.Insert(bootTemplate);
                    validationResult.Success = uow.Save();
                    validationResult.Object = JsonConvert.SerializeObject(bootTemplate);
                    validationResult.ObjectId = bootTemplate.Id;
                }

                return validationResult;
            }
        }

        public static string TotalCount()
        {
            using (var uow = new UnitOfWork())
            {
                return uow.BootTemplateRepository.Count();
            }
        }

        public static ActionResultEntity DeleteBootTemplate(int BootTemplateId)
        {
            var actionResult = new ActionResultEntity();
            var bootTemplate = GetBootTemplate(BootTemplateId);
            using (var uow = new UnitOfWork())
            {
                uow.BootTemplateRepository.Delete(BootTemplateId);
                actionResult.Success = uow.Save();
                actionResult.Object = JsonConvert.SerializeObject(bootTemplate);
                actionResult.ObjectId = bootTemplate.Id;
            }

            return actionResult;
        }

        public static BootTemplateEntity GetBootTemplate(int BootTemplateId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.BootTemplateRepository.GetById(BootTemplateId);
            }
        }

        public static List<BootTemplateEntity> SearchBootTemplates(string searchString = "")
        {
            using (var uow = new UnitOfWork())
            {
                return
                    uow.BootTemplateRepository.Get(
                        s => s.Name.Contains(searchString), orderBy: (q => q.OrderBy(t => t.Name)));
            }
        }

        public static ActionResultEntity UpdateBootTemplate(BootTemplateEntity bootTemplate)
        {
            using (var uow = new UnitOfWork())
            {
                var validationResult = ValidateTemplate(bootTemplate, false);
                if (validationResult.Success)
                {
                    uow.BootTemplateRepository.Update(bootTemplate, bootTemplate.Id);
                    validationResult.Success = uow.Save();
                    validationResult.ObjectId = bootTemplate.Id;
                    validationResult.Object = JsonConvert.SerializeObject(bootTemplate);
                }

                return validationResult;
            }
        }

        public static ActionResultEntity ValidateTemplate(BootTemplateEntity bootTemplate, bool isNewTemplate)
        {
            var validationResult = new ActionResultEntity();

            if (string.IsNullOrEmpty(bootTemplate.Name) || bootTemplate.Name.Contains(" "))
            {
                validationResult.Success = false;
                validationResult.Message = "Template Name Is Not Valid";
                return validationResult;
            }

            if (isNewTemplate)
            {
                using (var uow = new UnitOfWork())
                {
                    if (uow.BootTemplateRepository.Exists(h => h.Name == bootTemplate.Name))
                    {
                        validationResult.Success = false;
                        validationResult.Message = "This Boot Template Already Exists";
                        return validationResult;
                    }
                }
            }
            else
            {
                using (var uow = new UnitOfWork())
                {
                    var originalTemplate = uow.BootTemplateRepository.GetById(bootTemplate.Id);
                    if (originalTemplate.Name != bootTemplate.Name)
                    {
                        if (uow.BootTemplateRepository.Exists(h => h.Name == bootTemplate.Name))
                        {
                            validationResult.Success = false;
                            validationResult.Message = "This Boot Template Already Exists";
                            return validationResult;
                        }
                    }
                }
            }

            return validationResult;
        }

    }
}