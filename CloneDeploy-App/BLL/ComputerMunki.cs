using System.Collections.Generic;
using Newtonsoft.Json;

namespace CloneDeploy_App.BLL
{
    public static class ComputerMunki
    {
        public static Models.ActionResult AddMunkiTemplates(Models.ComputerMunki computerMunki)
        {
            var actionResult = new Models.ActionResult();
            using (var uow = new DAL.UnitOfWork())
            {
                uow.ComputerMunkiRepository.Insert(computerMunki);
                actionResult.Success = uow.Save();
                actionResult.ObjectId = computerMunki.Id;
                actionResult.Object = JsonConvert.SerializeObject(computerMunki);
                return actionResult;
            }
        }

        public static Models.ActionResult DeleteMunkiTemplates(int computerId)
        {
            var actionResult = new Models.ActionResult();
            using (var uow = new DAL.UnitOfWork())
            {
                uow.ComputerMunkiRepository.DeleteRange(x => x.ComputerId == computerId);
                actionResult.Success = uow.Save();
                actionResult.ObjectId = computerId;
                return actionResult;
            }
        }

        public static List<Models.ComputerMunki> Get(int computerId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ComputerMunkiRepository.Get(x => x.ComputerId == computerId);
            }
        }

        public static List<Models.ComputerMunki> GetComputersForManifestTemplate(int templateId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ComputerMunkiRepository.Get(x => x.MunkiTemplateId == templateId);
            }
        }

       
    }
}