using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class GroupMembership
    {
        //moved
        public static bool AddMembership(List<CloneDeploy_Web.Models.GroupMembership> groupMemberships)
        {
            var group = new CloneDeploy_Web.Models.Group();
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

        //moved
        public static string GetGroupMemberCount(int groupId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.GroupMembershipRepository.Count(g => g.GroupId == groupId);
            }
        }

        //move not needed
        public static bool DeleteAllMembershipsForGroup(int groupId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.GroupMembershipRepository.DeleteRange(x => x.GroupId == groupId);
                return uow.Save();
            }
        }

        //moved
        public static bool DeleteMembership(CloneDeploy_Web.Models.GroupMembership groupMembership)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.GroupMembershipRepository.DeleteRange(
                    g => g.ComputerId == groupMembership.ComputerId && g.GroupId == groupMembership.GroupId);
                return uow.Save();
            }
        }

        //move not needed
        public static bool DeleteComputerMemberships(int computerId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.GroupMembershipRepository.DeleteRange(x => x.ComputerId == computerId);
                return uow.Save();
            }
        }

        //moved
        public static List<CloneDeploy_Web.Models.GroupMembership> GetAllComputerMemberships(int computerId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.GroupMembershipRepository.Get(x => x.ComputerId == computerId);
            }
        }

      
    }
}