using System.Collections.Generic;

namespace BLL
{
    public static class GroupMunki
    {
        public static bool AddMunkiTemplates(List<Models.GroupMunki> listOfTemplates)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                foreach (var template in listOfTemplates)
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