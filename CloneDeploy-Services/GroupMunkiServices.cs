using System.Collections.Generic;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;

namespace CloneDeploy_Services
{
    public class GroupMunkiServices
    {
        private readonly UnitOfWork _uow;

        public GroupMunkiServices()
        {
            _uow = new UnitOfWork();
        }

        public ActionResultDTO AddMunkiTemplates(GroupMunkiEntity template)
        {


            _uow.GroupMunkiRepository.Insert(template);

            _uow.Save();
            var actionResult = new ActionResultDTO();
            actionResult.Success = true;
            actionResult.Id = template.Id;
            return actionResult;
            


        }

       

      

        public List<GroupMunkiEntity> GetGroupsForManifestTemplate(int templateId)
        {

            return _uow.GroupMunkiRepository.Get(x => x.MunkiTemplateId == templateId);

        }


    }
}