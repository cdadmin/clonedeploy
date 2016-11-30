using System;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Services;

namespace CloneDeploy_App.BLL
{
    public class ComputerProxyReservation
    {
        public static bool ToggleProxyReservation(int computerId, bool enable)
        {
            var computerServices = new ComputerServices();
            var computer = computerServices.GetComputer(computerId);
            computer.ProxyReservation = Convert.ToInt16(enable);
            computerServices.UpdateComputer(computer);
            return true;
        }
        public static ComputerProxyReservationEntity GetComputerProxyReservation(int computerId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.ComputerProxyRepository.GetFirstOrDefault(p => p.ComputerId == computerId);
            }
        }

        public static bool UpdateComputerProxyReservation(ComputerProxyReservationEntity computerProxyReservation)
        {
            using (var uow = new UnitOfWork())
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

                uow.Save();
                return true;
            }
        }

        public static bool DeleteComputerProxyReservation(int computerId)
        {
            using (var uow = new UnitOfWork())
            {
                uow.ComputerProxyRepository.DeleteRange(x => x.ComputerId == computerId);
                uow.Save();
                return true;
            }
        }
    }
}