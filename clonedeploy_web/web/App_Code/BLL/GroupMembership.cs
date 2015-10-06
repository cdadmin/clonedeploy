using System.Collections.Generic;
using System.Linq;
using DAL;

namespace BLL
{
    public class GroupMembership
    {
        private readonly DAL.UnitOfWork _unitOfWork;

        public GroupMembership()
        {
            _unitOfWork = new UnitOfWork();
        }

        public bool AddMembership(Models.GroupMembership groupMembership)
        {

            if (_unitOfWork.GroupMembershipRepository.Exists(g => g.ComputerId == groupMembership.ComputerId && g.GroupId == groupMembership.GroupId))
            {           
                return false;
            }
            _unitOfWork.GroupMembershipRepository.Insert(groupMembership);
            if (_unitOfWork.Save())
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
            return _unitOfWork.GroupMembershipRepository.Count(g => g.GroupId == groupId);
        }


        public bool DeleteMembership(Models.GroupMembership groupMembership)
        {
            _unitOfWork.GroupMembershipRepository.DeleteRange(g => g.ComputerId == groupMembership.ComputerId && g.GroupId == groupMembership.GroupId);
            return _unitOfWork.Save();
        }

      
    }
}