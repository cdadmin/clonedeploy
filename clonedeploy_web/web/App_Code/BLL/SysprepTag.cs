using System.Collections.Generic;
using System.Linq;
using Helpers;

namespace BLL
{
    public class SysprepTag
    {
        private DAL.UnitOfWork _unitOfWork;
        public SysprepTag()
        {
            _unitOfWork = new DAL.UnitOfWork();
        }
        public Models.ValidationResult AddSysprepTag(Models.SysprepTag sysprepTag)
        {
            var validationResult = ValidateSysprepTag(sysprepTag, true);
            if (validationResult.IsValid)
            {
                _unitOfWork.SysprepTagRepository.Insert(sysprepTag);
                validationResult.IsValid = _unitOfWork.Save();
            }

            return validationResult;
        }

        public string TotalCount()
        {
            return _unitOfWork.SysprepTagRepository.Count();
        }

        public bool DeleteScript(int sysprepTagId)
        {
            _unitOfWork.SysprepTagRepository.Delete(sysprepTagId);
            return _unitOfWork.Save();
        }

        public Models.SysprepTag GetSysprepTag(int sysprepTagId)
        {
            return _unitOfWork.SysprepTagRepository.GetById(sysprepTagId);
        }

        public List<Models.SysprepTag> SearchSysprepTags(string searchString)
        {
            return
                _unitOfWork.SysprepTagRepository.Get(
                    s => s.Name.Contains(searchString) || s.OpeningTag.Contains(searchString),
                    orderBy: (q => q.OrderBy(s => s.OpeningTag)));
        }

        public Models.ValidationResult UpdateSysprepTag(Models.SysprepTag sysprepTag)
        {
            var validationResult = ValidateSysprepTag(sysprepTag, false);
            if (validationResult.IsValid)
            {
                _unitOfWork.SysprepTagRepository.Update(sysprepTag, sysprepTag.Id);
                validationResult.IsValid = _unitOfWork.Save();
            }

            return validationResult;

           
            
        }

        public Models.ValidationResult ValidateSysprepTag(Models.SysprepTag sysprepTag, bool isNewSysprepTag)
        {
            var validationResult = new Models.ValidationResult();

            if (string.IsNullOrEmpty(sysprepTag.Name) || sysprepTag.Name.All(c => char.IsLetterOrDigit(c) || c == '_'))
            {
                validationResult.IsValid = false;
                validationResult.Message = "Sysprep Tag Name Is Not Valid";
                return validationResult;
            }

            if (isNewSysprepTag)
            {
                using (var uow = new DAL.UnitOfWork())
                {
                    if (uow.SysprepTagRepository.Exists(h => h.Name == sysprepTag.Name))
                    {
                        validationResult.IsValid = false;
                        validationResult.Message = "This Sysprep Tag Already Exists";
                        return validationResult;
                    }
                }
            }
            else
            {
                using (var uow = new DAL.UnitOfWork())
                {
                    var originalSysprepTag = uow.SysprepTagRepository.GetById(sysprepTag.Id);
                    if (originalSysprepTag.Name != sysprepTag.Name)
                    {
                        if (uow.SysprepTagRepository.Exists(h => h.Name == sysprepTag.Name))
                        {
                            validationResult.IsValid = false;
                            validationResult.Message = "This Sysprep Tag Already Exists";
                            return validationResult;
                        }
                    }
                }
            }

            return validationResult;
        }

      
    }
}