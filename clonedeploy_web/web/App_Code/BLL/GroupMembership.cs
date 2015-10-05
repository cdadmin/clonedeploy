using System.Collections.Generic;

namespace BLL
{
    public class GroupMembership
    {
        private readonly DAL.GroupMembership _da = new DAL.GroupMembership();

        public bool AddMembership(Models.GroupMembership groupMembership)
        {
            var count = 0;
            if (_da.Exists(groupMembership))
            {
               
                return false;
            }
            if (_da.Create(groupMembership))
            {
             
                return true;
            }
            else
            {
               
                return false;
            }
        }

        public string GetGroupMemberCount(int groupId)
        {
            return _da.GetTotalCount(groupId);
        }


        public bool DeleteMembership(Models.GroupMembership groupMembership)
        {
            return _da.Delete(groupMembership);
        }

        public List<Models.Computer> GetGroupMembers(int groupId, string searchString)
        {
            return _da.Find(groupId, searchString);
        }
    }
}