using System.Collections.Generic;
using DAL;
using Helpers;

namespace BLL
{
    public class ComputerBootMenu
    {

        public static  bool AddComputerBootMenu(Models.ComputerBootMenu computerBootMenu)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                    uow.ComputerBootMenuRepository.Insert(computerBootMenu);
                    return uow.Save();
            }

        }

        public static  string TotalCount()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ComputerBootMenuRepository.Count();
            }
        }

        public static  bool DeleteComputerBootMenu(int computerBootMenuId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.ComputerBootMenuRepository.Delete(computerBootMenuId);
                return uow.Save();
            }
        }

        public static  Models.ComputerBootMenu GetComputerBootMenu(int computerBootMenuId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ComputerBootMenuRepository.GetById(computerBootMenuId);
            }
        }

        public static  bool UpdateComputerBootMenu(Models.ComputerBootMenu computerBootMenu)
        {
            using (var uow = new DAL.UnitOfWork())
            {

                uow.ComputerBootMenuRepository.Update(computerBootMenu, computerBootMenu.Id);
                return uow.Save();

            }
        }

        
      
    }
}