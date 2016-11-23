using System.Collections.Generic;

namespace CloneDeploy_App.BLL
{
    public static class GroupMunki
    {
        public static bool AddMunkiTemplates(Models.GroupMunki template)
        {
            using (var uow = new DAL.UnitOfWork())
            {
               
                    uow.GroupMunkiRepository.Insert(template);

                return uow.Save();
            }
        }

        public static bool DeleteMunkiTemplates(int groupId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.GroupMunkiRepository.DeleteRange(x => x.GroupId == groupId);
                return uow.Save();
            }
        }

        public static List<Models.GroupMunki> Get(int groupId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.GroupMunkiRepository.Get(x => x.GroupId == groupId);
            }
        }

        public static List<Models.GroupMunki> GetGroupsForManifestTemplate(int templateId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.GroupMunkiRepository.Get(x => x.MunkiTemplateId == templateId);
            }
        }

       
    }
}