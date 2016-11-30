using System.Collections.Generic;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
namespace CloneDeploy_App.BLL
{
    public static class GroupMunki
    {
        public static bool AddMunkiTemplates(GroupMunkiEntity template)
        {
            using (var uow = new UnitOfWork())
            {
               
                    uow.GroupMunkiRepository.Insert(template);

                uow.Save();
                return true;

            }
        }

        public static bool DeleteMunkiTemplates(int groupId)
        {
            using (var uow = new UnitOfWork())
            {
                uow.GroupMunkiRepository.DeleteRange(x => x.GroupId == groupId);
                uow.Save();
                return true;
            }
        }

        public static List<GroupMunkiEntity> Get(int groupId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.GroupMunkiRepository.Get(x => x.GroupId == groupId);
            }
        }

        public static List<GroupMunkiEntity> GetGroupsForManifestTemplate(int templateId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.GroupMunkiRepository.Get(x => x.MunkiTemplateId == templateId);
            }
        }

       
    }
}