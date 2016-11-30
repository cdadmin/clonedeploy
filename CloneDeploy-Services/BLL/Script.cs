using System.Collections.Generic;
using System.Linq;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
namespace CloneDeploy_App.BLL
{
    public class Script
    {

        public static ActionResultEntity AddScript(ScriptEntity script)
        {
            using (var uow = new UnitOfWork())
            {
                var validationResult = ValidateScript(script, true);
                if (validationResult.Success)
                {
                    uow.ScriptRepository.Insert(script);
                    uow.Save();
                    validationResult.Success = true;
                }

                return validationResult;
            }
        }

        public static string TotalCount()
        {
            using (var uow = new UnitOfWork())
            {
                return uow.ScriptRepository.Count();
            }
        }

        public static bool DeleteScript(int scriptId)
        {
            using (var uow = new UnitOfWork())
            {
                uow.ScriptRepository.Delete(scriptId);
                uow.Save();
                return true;
            }
        }

        public static ScriptEntity GetScript(int scriptId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.ScriptRepository.GetById(scriptId);
            }
        }

        public static List<ScriptEntity> SearchScripts(string searchString = "")
        {
            using (var uow = new UnitOfWork())
            {
                return uow.ScriptRepository.Get(s => s.Name.Contains(searchString));
            }
        }

        public static ActionResultEntity UpdateScript(ScriptEntity script)
        {
            using (var uow = new UnitOfWork())
            {
                var validationResult = ValidateScript(script, false);
                if (validationResult.Success)
                {
                    uow.ScriptRepository.Update(script, script.Id);
                    uow.Save();
                    validationResult.Success = true;
                }

                return validationResult;
            }
        }

        public static ActionResultEntity ValidateScript(ScriptEntity script, bool isNewScript)
        {
            var validationResult = new ActionResultEntity();

            if (string.IsNullOrEmpty(script.Name) || !script.Name.All(c => char.IsLetterOrDigit(c) || c == '_'))
            {
                validationResult.Success = false;
                validationResult.Message = "Script Name Is Not Valid";
                return validationResult;
            }

            if (isNewScript)
            {
                using (var uow = new UnitOfWork())
                {
                    if (uow.ScriptRepository.Exists(h => h.Name == script.Name))
                    {
                        validationResult.Success = false;
                        validationResult.Message = "This Script Already Exists";
                        return validationResult;
                    }
                }
            }
            else
            {
                using (var uow = new UnitOfWork())
                {
                    var originalScript = uow.ScriptRepository.GetById(script.Id);
                    if (originalScript.Name != script.Name)
                    {
                        if (uow.ScriptRepository.Exists(h => h.Name == script.Name))
                        {
                            validationResult.Success = false;
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