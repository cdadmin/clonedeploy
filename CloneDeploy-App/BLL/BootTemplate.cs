using System.Collections.Generic;
using System.Linq;

namespace CloneDeploy_App.BLL
{
    public class BootTemplate
    {

        public static Models.ActionResult AddBootTemplate(Models.BootTemplate bootTemplate)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateTemplate(bootTemplate, true);
                if (validationResult.Success)
                {
                    uow.BootTemplateRepository.Insert(bootTemplate);
                    validationResult.Success = uow.Save();
                }

                return validationResult;
            }
        }

        public static string TotalCount()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.BootTemplateRepository.Count();
            }
        }

        public static bool DeleteBootTemplate(int BootTemplateId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.BootTemplateRepository.Delete(BootTemplateId);
                return uow.Save();
            }
        }

        public static Models.BootTemplate GetBootTemplate(int BootTemplateId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.BootTemplateRepository.GetById(BootTemplateId);
            }
        }

        public static List<Models.BootTemplate> SearchBootTemplates(string searchString = "")
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return
                    uow.BootTemplateRepository.Get(
                        s => s.Name.Contains(searchString), orderBy: (q => q.OrderBy(t => t.Name)));
            }
        }

        public static Models.ActionResult UpdateBootTemplate(Models.BootTemplate bootTemplate)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateTemplate(bootTemplate, false);
                if (validationResult.Success)
                {
                    uow.BootTemplateRepository.Update(bootTemplate, bootTemplate.Id);
                    validationResult.Success = uow.Save();
                }

                return validationResult;
            }
        }

        public static Models.ActionResult ValidateTemplate(Models.BootTemplate bootTemplate, bool isNewTemplate)
        {
            var validationResult = new Models.ActionResult();

            if (string.IsNullOrEmpty(bootTemplate.Name) || bootTemplate.Name.Contains(" "))
            {
                validationResult.Success = false;
                validationResult.Message = "Template Name Is Not Valid";
                return validationResult;
            }

            if (isNewTemplate)
            {
                using (var uow = new DAL.UnitOfWork())
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
                using (var uow = new DAL.UnitOfWork())
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