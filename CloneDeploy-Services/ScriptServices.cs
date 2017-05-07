using System.Collections.Generic;
using System.Linq;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;

namespace CloneDeploy_Services
{
    public class ScriptServices
    {
        private readonly UnitOfWork _uow;

        public ScriptServices()
        {
            _uow = new UnitOfWork();
        }

        public ActionResultDTO AddScript(ScriptEntity script)
        {
            var actionResult = new ActionResultDTO();
            var validationResult = ValidateScript(script, true);
            if (validationResult.Success)
            {
                _uow.ScriptRepository.Insert(script);
                _uow.Save();
                actionResult.Success = true;
                actionResult.Id = script.Id;
            }

            return actionResult;
        }

        public ActionResultDTO DeleteScript(int scriptId)
        {
            var script = GetScript(scriptId);
            if (script == null) return new ActionResultDTO {ErrorMessage = "Script Not Found", Id = 0};
            _uow.ScriptRepository.Delete(scriptId);
            _uow.Save();
            var actionResult = new ActionResultDTO();
            actionResult.Success = true;
            actionResult.Id = script.Id;
            return actionResult;
        }

        public ScriptEntity GetScript(int scriptId)
        {
            return _uow.ScriptRepository.GetById(scriptId);
        }

        public List<ScriptEntity> SearchScripts(string searchString = "")
        {
            return _uow.ScriptRepository.Get(s => s.Name.Contains(searchString));
        }

        public string TotalCount()
        {
            return _uow.ScriptRepository.Count();
        }

        public ActionResultDTO UpdateScript(ScriptEntity script)
        {
            var s = GetScript(script.Id);
            if (s == null) return new ActionResultDTO {ErrorMessage = "Script Not Found", Id = 0};
            var validationResult = ValidateScript(script, false);
            var actionResult = new ActionResultDTO();
            if (validationResult.Success)
            {
                _uow.ScriptRepository.Update(script, script.Id);
                _uow.Save();
                actionResult.Success = true;
                actionResult.Id = script.Id;
            }
            else
            {
                actionResult.ErrorMessage = validationResult.ErrorMessage;
            }

            return actionResult;
        }

        private ValidationResultDTO ValidateScript(ScriptEntity script, bool isNewScript)
        {
            var validationResult = new ValidationResultDTO {Success = true};

            if (string.IsNullOrEmpty(script.Name) || !script.Name.All(c => char.IsLetterOrDigit(c) || c == '_'))
            {
                validationResult.Success = false;
                validationResult.ErrorMessage = "Script Name Is Not Valid";
                return validationResult;
            }

            if (isNewScript)
            {
                if (_uow.ScriptRepository.Exists(h => h.Name == script.Name))
                {
                    validationResult.Success = false;
                    validationResult.ErrorMessage = "This Script Already Exists";
                    return validationResult;
                }
            }
            else
            {
                var originalScript = _uow.ScriptRepository.GetById(script.Id);
                if (originalScript.Name != script.Name)
                {
                    if (_uow.ScriptRepository.Exists(h => h.Name == script.Name))
                    {
                        validationResult.Success = false;
                        validationResult.ErrorMessage = "This Script Already Exists";
                        return validationResult;
                    }
                }
            }

            return validationResult;
        }
    }
}