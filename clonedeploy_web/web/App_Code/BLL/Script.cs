using System.Collections.Generic;
using System.Linq;
using DAL;
using Helpers;

namespace BLL
{
    public class Script
    {
        private readonly DAL.UnitOfWork _unitOfWork;

        public Script()
        {
            _unitOfWork = new UnitOfWork();
        }

        public Models.ValidationResult AddScript(Models.Script script)
        {

            var validationResult = ValidateScript(script, true);
            if (validationResult.IsValid)
            {
                _unitOfWork.ScriptRepository.Insert(script);
                validationResult.IsValid = _unitOfWork.Save();
            }

            return validationResult;
        }

        public string TotalCount()
        {
            return _unitOfWork.ScriptRepository.Count();
        }

        public bool DeleteScript(int scriptId)
        {
            _unitOfWork.ScriptRepository.Delete(scriptId);
            return _unitOfWork.Save();
        }

        public Models.Script GetScript(int scriptId)
        {
            return _unitOfWork.ScriptRepository.GetById(scriptId);
        }

        public List<Models.Script> SearchScripts(string searchString)
        {
            return _unitOfWork.ScriptRepository.Get(s => s.Name.Contains(searchString));
        }

        public Models.ValidationResult UpdateScript(Models.Script script)
        {
            var validationResult = ValidateScript(script, false);
            if (validationResult.IsValid)
            {
                _unitOfWork.ScriptRepository.Update(script, script.Id);
                validationResult.IsValid = _unitOfWork.Save();
            }

            return validationResult;
        }

        public Models.ValidationResult ValidateScript(Models.Script script, bool isNewScript)
        {
            var validationResult = new Models.ValidationResult();

            if (string.IsNullOrEmpty(script.Name) || script.Name.All(c => char.IsLetterOrDigit(c) || c == '_'))
            {
                validationResult.IsValid = false;
                validationResult.Message = "Script Name Is Not Valid";
                return validationResult;
            }

            if (isNewScript)
            {
                using (var uow = new DAL.UnitOfWork())
                {
                    if (uow.ScriptRepository.Exists(h => h.Name == script.Name))
                    {
                        validationResult.IsValid = false;
                        validationResult.Message = "This Script Already Exists";
                        return validationResult;
                    }
                }
            }
            else
            {
                using (var uow = new DAL.UnitOfWork())
                {
                    var originalScript = uow.ScriptRepository.GetById(script.Id);
                    if (originalScript.Name != script.Name)
                    {
                        if (uow.ScriptRepository.Exists(h => h.Name == script.Name))
                        {
                            validationResult.IsValid = false;
                            validationResult.Message = "This Script Already Exists";
                            return validationResult;
                        }
                    }
                }
            }

            return validationResult;
        }
      
    }
}