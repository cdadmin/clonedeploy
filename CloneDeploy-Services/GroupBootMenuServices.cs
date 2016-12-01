using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;

namespace CloneDeploy_Services
{
    public class GroupBootMenuServices
    {
         private readonly UnitOfWork _uow;

        public GroupBootMenuServices()
        {
            _uow = new UnitOfWork();
        }

       

        public  void UpdateGroupMemberBootMenus(GroupBootMenuEntity groupBootMenu)
        {
            foreach (var computer in new GroupServices().GetGroupMembers(groupBootMenu.GroupId))
            {
                var computerBootMenu = new ComputerBootMenuEntity
                {
                    ComputerId = computer.Id,
                    BiosMenu = groupBootMenu.BiosMenu,
                    Efi32Menu = groupBootMenu.Efi32Menu,
                    Efi64Menu = groupBootMenu.Efi64Menu
                };

                var computerServices = new ComputerBootMenuServices();
                computerServices.UpdateComputerBootMenu(computerBootMenu);
                computerServices.ToggleComputerBootMenu(computer.Id, true);
            }
        }

        public ActionResultDTO UpdateGroupBootMenu(GroupBootMenuEntity groupBootMenu)
        {
           
                if (_uow.GroupBootMenuRepository.Exists(x => x.GroupId == groupBootMenu.GroupId))
                {
                    groupBootMenu.Id =
                        _uow.GroupBootMenuRepository.GetFirstOrDefault(
                            x => x.GroupId == groupBootMenu.GroupId).Id;
                    _uow.GroupBootMenuRepository.Update(groupBootMenu, groupBootMenu.Id);
                }
                else
                    _uow.GroupBootMenuRepository.Insert(groupBootMenu);

                _uow.Save();
                var actionResult = new ActionResultDTO();
            actionResult.Success = true;
            actionResult.Id = groupBootMenu.Id;
            

            UpdateGroupMemberBootMenus(groupBootMenu);


            return actionResult;
        }

        public  bool DeleteAllForGroup(int groupId)
        {
            
                _uow.GroupBootMenuRepository.DeleteRange(x => x.GroupId == groupId);
                _uow.Save();
                return true;
            
        }


    }
}