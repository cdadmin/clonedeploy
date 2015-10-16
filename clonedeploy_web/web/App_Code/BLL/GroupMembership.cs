using System.Collections.Generic;
using System.Linq;
using DAL;

namespace BLL
{
    public class GroupMembership
    {
        public static bool AddMembership(Models.GroupMembership groupMembership)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                if (
                    uow.GroupMembershipRepository.Exists(
                        g => g.ComputerId == groupMembership.ComputerId && g.GroupId == groupMembership.GroupId))
                {
                    return false;
                }
                uow.GroupMembershipRepository.Insert(groupMembership);
                return uow.Save();
            }
        }

        public static string GetGroupMemberCount(int groupId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.GroupMembershipRepository.Count(g => g.GroupId == groupId);
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

      
    }
}