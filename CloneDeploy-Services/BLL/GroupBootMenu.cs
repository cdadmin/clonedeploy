using CloneDeploy_DataModel;
using CloneDeploy_Entities;
namespace CloneDeploy_App.BLL
{
    public class GroupBootMenu
    {

        public static GroupBootMenuEntity GetGroupBootMenu(int groupId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.GroupBootMenuRepository.GetFirstOrDefault(p => p.GroupId == groupId);
            }
        }

        public static void UpdateGroupMemberBootMenus(GroupBootMenuEntity groupBootMenu)
        {
            foreach (var computer in BLL.Group.GetGroupMembers(groupBootMenu.GroupId))
            {
                var computerBootMenu = new ComputerBootMenuEntity
                {
                    ComputerId = computer.Id,
                    BiosMenu = groupBootMenu.BiosMenu,
                    Efi32Menu = groupBootMenu.Efi32Menu,
                    Efi64Menu = groupBootMenu.Efi64Menu
                };

                BLL.ComputerBootMenu.UpdateComputerBootMenu(computerBootMenu);
                BLL.ComputerBootMenu.ToggleComputerBootMenu(computer.Id, true);
            }
        }

        public static bool UpdateGroupBootMenu(GroupBootMenuEntity groupBootMenu)
        {
            using (var uow = new UnitOfWork())
            {
                if (uow.GroupBootMenuRepository.Exists(x => x.GroupId == groupBootMenu.GroupId))
                {
                    groupBootMenu.Id =
                        uow.GroupBootMenuRepository.GetFirstOrDefault(
                            x => x.GroupId == groupBootMenu.GroupId).Id;
                    uow.GroupBootMenuRepository.Update(groupBootMenu, groupBootMenu.Id);
                }
                else
                    uow.GroupBootMenuRepository.Insert(groupBootMenu);

                uow.Save();
                

            }

            UpdateGroupMemberBootMenus(groupBootMenu);
           
           
            return true;
        }

        public static bool DeleteGroup(int groupId)
        {
            using (var uow = new UnitOfWork())
            {
                uow.GroupBootMenuRepository.DeleteRange(x => x.GroupId == groupId);
                uow.Save();
                return true;
            }
        }


    }
}