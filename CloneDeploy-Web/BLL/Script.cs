using System.Collections.Generic;
using System.Linq;
using CloneDeploy_Web.Models;

namespace BLL
{
    public class Script
    {
        //moved
        public static ActionResult AddScript(CloneDeploy_Web.Models.Script script)
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

        //moved
        public static string TotalCount()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ScriptRepository.Count();
            }
        }

        //moved
        public static bool DeleteScript(int scriptId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.ScriptRepository.Delete(scriptId);
                return uow.Save();
            }
        }

        //moved
        public static CloneDeploy_Web.Models.Script GetScript(int scriptId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ScriptRepository.GetById(scriptId);
            }
        }

        //moved
        public static List<CloneDeploy_Web.Models.Script> SearchScripts(string searchString="")
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ScriptRepository.Get(s => s.Name.Contains(searchString));
            }
        }

        //moved
        public static ActionResult UpdateScript(CloneDeploy_Web.Models.Script script)
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

        //move not needed
        public static ActionResult ValidateScript(CloneDeploy_Web.Models.Script script, bool isNewScript)
        {
            var validationResult = new ActionResult();

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