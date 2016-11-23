using System;

namespace BLL
{
    public class ComputerProxyReservation
    {
        //moved
        public static bool ToggleProxyReservation(CloneDeploy_Web.Models.Computer computer, bool enable)
        {
            computer.ProxyReservation = Convert.ToInt16(enable);
            BLL.Computer.UpdateComputer(computer);
            return true;
        }

        //moved
        public static CloneDeploy_Web.Models.ComputerProxyReservation GetComputerProxyReservation(int computerId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ComputerProxyRepository.GetFirstOrDefault(p => p.ComputerId == computerId);
            }
        }

        //moved
        public static bool UpdateComputerProxyReservation(CloneDeploy_Web.Models.ComputerProxyReservation computerProxyReservation)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                if (uow.ComputerProxyRepository.Exists(x => x.ComputerId == computerProxyReservation.ComputerId))
                {
                    computerProxyReservation.Id =
                        uow.ComputerProxyRepository.GetFirstOrDefault(
                            x => x.ComputerId == computerProxyReservation.ComputerId).Id;
                    uow.ComputerProxyRepository.Update(computerProxyReservation, computerProxyReservation.Id);
                }
                else
                    uow.ComputerProxyRepository.Insert(computerProxyReservation);

                return uow.Save();
            }
        }

        //move not needed
        public static bool DeleteComputerProxyReservation(int computerId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.ComputerProxyRepository.DeleteRange(x => x.ComputerId == computerId);
                return uow.Save();
            }
        }
    }
}