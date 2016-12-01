using System.Collections.Generic;
using System.Linq;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;

namespace CloneDeploy_Services
{
    public class GroupMembershipServices
    {
         private readonly UnitOfWork _uow;

        public GroupMembershipServices()
        {
            _uow = new UnitOfWork();
        }

        public ActionResultDTO AddMembership(List<GroupMembershipEntity> groupMemberships)
        {
            var group = new GroupEntity();
            var actionResult = new ActionResultDTO();
            
                foreach (var membership in groupMemberships.Where(membership => !_uow.GroupMembershipRepository.Exists(
                    g => g.ComputerId == membership.ComputerId && g.GroupId == membership.GroupId)))
                {
                    _uow.GroupMembershipRepository.Insert(membership);
                    group = new GroupServices().GetGroup(membership.GroupId);
                }
                _uow.Save();
                actionResult.Success = true;
            

            if (group.SetDefaultProperties == 1)
            {
                var groupProperty = GroupPropertyServices.GetGroupProperty(group.Id);
                GroupPropertyServices.UpdateComputerProperties(groupProperty);
            }

            if (group.SetDefaultBootMenu == 1)
            {
                var gbmServices = new GroupBootMenuServices();
                var groupBootMenu = gbmServices.GetGroupBootMenu(group.Id);
                gbmServices.UpdateGroupMemberBootMenus(groupBootMenu);
            }

            return actionResult;
        }

      

        public  bool DeleteAllMembershipsForGroup(int groupId)
        {
            
                _uow.GroupMembershipRepository.DeleteRange(x => x.GroupId == groupId);
                _uow.Save();
                return true;
            
        }


        

        public  bool DeleteComputerMemberships(int computerId)
        {
           
                _uow.GroupMembershipRepository.DeleteRange(x => x.ComputerId == computerId);
                _uow.Save();
                return true;
            
        }

     

      
    }
}