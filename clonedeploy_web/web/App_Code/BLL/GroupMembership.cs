using System.Collections.Generic;
using System.Linq;
using System.Web.ModelBinding;
using DAL;

namespace BLL
{
    public class GroupMembership
    {
        public static bool AddMembership(List<Models.GroupMembership> groupMemberships)
        {
            var group = new Models.Group();
            var result = false;
            using (var uow = new DAL.UnitOfWork())
            {
                foreach (var membership in groupMemberships.Where(membership => !uow.GroupMembershipRepository.Exists(
                    g => g.ComputerId == membership.ComputerId && g.GroupId == membership.GroupId)))
                {
                    uow.GroupMembershipRepository.Insert(membership);
                    group = BLL.Group.GetGroup(membership.GroupId);
                }
                result = uow.Save();
            }

            if (group.SetDefaultProperties == 1)
            {
                var groupProperty = BLL.GroupProperty.GetGroupProperty(group.Id);
                BLL.GroupProperty.UpdateComputerProperties(groupProperty);
            }

            if (group.SetDefaultBootMenu == 1)
            {
                var groupBootMenu = BLL.GroupBootMenu.GetGroupBootMenu(group.Id);
                BLL.GroupBootMenu.UpdateGroupMemberBootMenus(groupBootMenu);
            }

            return result;
        }

        public static string GetGroupMemberCount(int groupId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.GroupMembershipRepository.Count(g => g.GroupId == groupId);
            }
        }

        public static bool DeleteAllMembershipsForGroup(int groupId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.GroupMembershipRepository.DeleteRange(x => x.GroupId == groupId);
                return uow.Save();
            }
        }


        public static bool DeleteMembership(Models.GroupMembership groupMembership)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.GroupMembershipRepository.DeleteRange(
                    g => g.ComputerId == groupMembership.ComputerId && g.GroupId == groupMembership.GroupId);
                return uow.Save();
            }
        }

        public static bool DeleteComputerMemberships(int computerId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.GroupMembershipRepository.DeleteRange(x => x.ComputerId == computerId);
                return uow.Save();
            }
        }

      
    }
}