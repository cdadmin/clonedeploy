using System.Collections.Generic;
using System.Linq;
using DAL;
using Helpers;

namespace BLL
{
    public class Script
    {

        public static Models.ValidationResult AddScript(Models.Script script)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateScript(script, true);
                if (validationResult.IsValid)
                {
                    uow.ScriptRepository.Insert(script);
                    validationResult.IsValid = uow.Save();
                }

                return validationResult;
            }
        }

        public static string TotalCount()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ScriptRepository.Count();
            }
        }

        public static bool DeleteScript(int scriptId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.ScriptRepository.Delete(scriptId);
                return uow.Save();
            }
        }

        public static Models.Script GetScript(int scriptId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ScriptRepository.GetById(scriptId);
            }
        }

        public static List<Models.Script> SearchScripts(string searchString)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ScriptRepository.Get(s => s.Name.Contains(searchString));
            }
        }

        public static Models.ValidationResult UpdateScript(Models.Script script)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateScript(script, false);
                if (validationResult.IsValid)
                {
                    uow.ScriptRepository.Update(script, script.Id);
                    validationResult.IsValid = uow.Save();
                }

                return validationResult;
            }
        }

        public static Models.ValidationResult ValidateScript(Models.Script script, bool isNewScript)
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