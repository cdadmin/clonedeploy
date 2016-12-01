using System;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;

namespace CloneDeploy_Services
{
    public class ComputerProxyReservationServices
    {
         private readonly UnitOfWork _uow;

        public ComputerProxyReservationServices()
        {
            _uow = new UnitOfWork();
        }





        public ActionResultDTO UpdateComputerProxyReservation(ComputerProxyReservationEntity computerProxyReservation)
        {

            if (_uow.ComputerProxyRepository.Exists(x => x.ComputerId == computerProxyReservation.ComputerId))
            {
                computerProxyReservation.Id =
                    _uow.ComputerProxyRepository.GetFirstOrDefault(
                        x => x.ComputerId == computerProxyReservation.ComputerId).Id;
                _uow.ComputerProxyRepository.Update(computerProxyReservation, computerProxyReservation.Id);
            }
            else
                _uow.ComputerProxyRepository.Insert(computerProxyReservation);

            _uow.Save();
            var actionResult = new ActionResultDTO();
            actionResult.Success = true;
            actionResult.Id = computerProxyReservation.Id;
            return actionResult;

        }

        public  bool DeleteComputerProxyReservation(int computerId)
        {
           
                _uow.ComputerProxyRepository.DeleteRange(x => x.ComputerId == computerId);
                _uow.Save();
                return true;
            
        }
    }
}