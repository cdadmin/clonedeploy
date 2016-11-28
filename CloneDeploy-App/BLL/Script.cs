using System.Collections.Generic;
using System.Linq;

namespace CloneDeploy_App.BLL
{
    public class Script
    {

        public static Models.ActionResult AddScript(Models.Script script)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateScript(script, true);
                if (validationResult.Success)
                {
                    uow.ScriptRepository.Insert(script);
                    validationResult.Success = uow.Save();
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

        public static List<Models.Script> SearchScripts(string searchString="")
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ScriptRepository.Get(s => s.Name.Contains(searchString));
            }
        }

        public static Models.ActionResult UpdateScript(Models.Script script)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateScript(script, false);
                if (validationResult.Success)
                {
                    uow.ScriptRepository.Update(script, script.Id);
                    validationResult.Success = uow.Save();
                }

                return validationResult;
            }
        }

        public static Models.ActionResult ValidateScript(Models.Script script, bool isNewScript)
        {
            var validationResult = new Models.ActionResult();

            if (string.IsNullOrEmpty(script.Name) || !script.Name.All(c => char.IsLetterOrDigit(c) || c == '_'))
            {
                validationResult.Success = false;
                validationResult.Message = "Script Name Is Not Valid";
                return validationResult;
            }

            if (isNewScript)
            {
                using (var uow = new DAL.UnitOfWork())
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
                using (var uow = new DAL.UnitOfWork())
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