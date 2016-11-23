using System;

namespace CloneDeploy_App.BLL
{
    public class ComputerProxyReservation
    {
        public static bool ToggleProxyReservation(int computerId, bool enable)
        {
            var computer = BLL.Computer.GetComputer(computerId);
            computer.ProxyReservation = Convert.ToInt16(enable);
            BLL.Computer.UpdateComputer(computer);
            return true;
        }
        public static Models.ComputerProxyReservation GetComputerProxyReservation(int computerId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ComputerProxyRepository.GetFirstOrDefault(p => p.ComputerId == computerId);
            }
        }

        public static bool UpdateComputerProxyReservation(Models.ComputerProxyReservation computerProxyReservation)
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