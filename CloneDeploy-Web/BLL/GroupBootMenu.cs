namespace BLL
{
    public class GroupBootMenu
    {
        //moved
        public static CloneDeploy_Web.Models.GroupBootMenu GetGroupBootMenu(int groupId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.GroupBootMenuRepository.GetFirstOrDefault(p => p.GroupId == groupId);
            }
        }

        //move not needed
        public static void UpdateGroupMemberBootMenus(CloneDeploy_Web.Models.GroupBootMenu groupBootMenu)
        {
            foreach (var computer in BLL.Group.GetGroupMembers(groupBootMenu.GroupId))
            {
                var computerBootMenu = new CloneDeploy_Web.Models.ComputerBootMenu
                {
                    ComputerId = computer.Id,
                    BiosMenu = groupBootMenu.BiosMenu,
                    Efi32Menu = groupBootMenu.Efi32Menu,
                    Efi64Menu = groupBootMenu.Efi64Menu
                };

                BLL.ComputerBootMenu.UpdateComputerBootMenu(computerBootMenu);
                BLL.ComputerBootMenu.ToggleComputerBootMenu(computer, true);
            }
        }

        //moved
        public static bool UpdateGroupBootMenu(CloneDeploy_Web.Models.GroupBootMenu groupBootMenu)
        {
            using (var uow = new DAL.UnitOfWork())
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

                if (!uow.Save()) return false;
              
            }

            UpdateGroupMemberBootMenus(groupBootMenu);
           
           
            return true;
        }

        //move not needed
        public static bool DeleteGroup(int groupId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.GroupBootMenuRepository.DeleteRange(x => x.GroupId == groupId);
                return uow.Save();
            }
        }


    }
}