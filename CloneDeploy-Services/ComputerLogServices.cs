using System;
using System.Collections.Generic;
using System.Linq;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;

namespace CloneDeploy_Services
{
    public class ComputerLogServices
    {
        private readonly UnitOfWork _uow;

        public ComputerLogServices()
        {
            _uow = new UnitOfWork();
        }

        public ActionResultDTO AddComputerLog(ComputerLogEntity computerLog)
        {
            var actionResult = new ActionResultDTO();
            computerLog.LogTime = DateTime.Now;
            _uow.ComputerLogRepository.Insert(computerLog);
            _uow.Save();
            actionResult.Success = true;
            actionResult.Id = computerLog.Id;

            return actionResult;
        }

        public ActionResultDTO DeleteComputerLog(int computerLogId)
        {
            var computerLog = GetComputerLog(computerLogId);
            if (computerLog == null)
                return new ActionResultDTO {ErrorMessage = "Computer Log Not Found", Id = 0};

            var actionResult = new ActionResultDTO();

            _uow.ComputerLogRepository.Delete(computerLogId);
            _uow.Save();
            actionResult.Success = true;
            actionResult.Id = computerLogId;

            return actionResult;
        }

        public ComputerLogEntity GetComputerLog(int computerLogId)
        {
            return _uow.ComputerLogRepository.GetById(computerLogId);
        }

        public List<ComputerLogEntity> SearchOnDemand(int limit)
        {
            return
                _uow.ComputerLogRepository.Get(x => x.ComputerId <= -1, q => q.OrderByDescending(x => x.LogTime))
                    .Take(limit)
                    .ToList();
        }
    }
}