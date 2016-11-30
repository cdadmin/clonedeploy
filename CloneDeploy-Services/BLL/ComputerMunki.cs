using System.Collections.Generic;
using CloneDeploy_DataModel;
using Newtonsoft.Json;
using CloneDeploy_Entities;

namespace CloneDeploy_App.BLL
{
    public static class ComputerMunki
    {
        public static ActionResultEntity AddMunkiTemplates(ComputerMunkiEntity computerMunki)
        {
            var actionResult = new ActionResultEntity();
            using (var uow = new UnitOfWork())
            {
                uow.ComputerMunkiRepository.Insert(computerMunki);
                uow.Save();
                actionResult.Success = true;
                actionResult.ObjectId = computerMunki.Id;
                actionResult.Object = JsonConvert.SerializeObject(computerMunki);
                return actionResult;
            }
        }

        public static ActionResultEntity DeleteMunkiTemplates(int computerId)
        {
            var actionResult = new ActionResultEntity();
            using (var uow = new UnitOfWork())
            {
                uow.ComputerMunkiRepository.DeleteRange(x => x.ComputerId == computerId);
                uow.Save();
                actionResult.Success = true;
                actionResult.ObjectId = computerId;
                return actionResult;
            }
        }

        public static List<ComputerMunkiEntity> Get(int computerId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.ComputerMunkiRepository.Get(x => x.ComputerId == computerId);
            }
        }

        public static List<ComputerMunkiEntity> GetComputersForManifestTemplate(int templateId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.ComputerMunkiRepository.Get(x => x.MunkiTemplateId == templateId);
            }
        }

       
    }
}