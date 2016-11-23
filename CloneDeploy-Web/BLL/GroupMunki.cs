using System.Collections.Generic;

namespace BLL
{
    public static class GroupMunki
    {
        //moved
        public static bool AddMunkiTemplates(CloneDeploy_Web.Models.GroupMunki template)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.GroupMunkiRepository.Insert(template);
                return uow.Save();
            }
        }

        //moved
        public static bool DeleteMunkiTemplates(int groupId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.GroupMunkiRepository.DeleteRange(x => x.GroupId == groupId);
                return uow.Save();
            }
        }

        //moved
        public static List<CloneDeploy_Web.Models.GroupMunki> Get(int groupId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.GroupMunkiRepository.Get(x => x.GroupId == groupId);
            }
        }

        //move not needed
        public static List<CloneDeploy_Web.Models.GroupMunki> GetGroupsForManifestTemplate(int templateId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.GroupMunkiRepository.Get(x => x.MunkiTemplateId == templateId);
            }
        }

       
    }
}