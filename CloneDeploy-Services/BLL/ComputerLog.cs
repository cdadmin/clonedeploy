using System.Collections.Generic;
using System.Linq;
using CloneDeploy_App.Helpers;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using Newtonsoft.Json;

namespace CloneDeploy_App.BLL
{
    public static class ComputerLog
    {
        public static ActionResultEntity AddComputerLog(ComputerLogEntity computerLog)
        {
            var actionResult = new ActionResultEntity();
            using (var uow = new UnitOfWork())
            {
                uow.ComputerLogRepository.Insert(computerLog);
                actionResult.Success = uow.Save();
                actionResult.ObjectId = computerLog.Id;
            }
            return actionResult;
        }

        public static ActionResultEntity DeleteComputerLog(int computerLogId)
        {
            var computerLog = GetComputerLog(computerLogId);
            if (computerLog == null)
            {
                var message = string.Format("Could Not Delete Computer Log With Id {0}.  The Computer Log Was not Found", computerLogId);
                Logger.Log(message);
                return new ActionResultEntity() { Success = false, Message = message, ObjectId = computerLogId };
            }
            var actionResult = new ActionResultEntity();
            using (var uow = new UnitOfWork())
            {
                uow.ComputerLogRepository.Delete(computerLogId);
                actionResult.Success = uow.Save();
                actionResult.ObjectId = computerLogId;
            }
            return actionResult;
        }

        public static ComputerLogEntity GetComputerLog(int computerLogId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.ComputerLogRepository.GetById(computerLogId);
            }
        }


        public static List<ComputerLogEntity> Search(int computerId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.ComputerLogRepository.Get(x => x.ComputerId == computerId, orderBy:(q => q.OrderByDescending(x => x.LogTime)));
            }
        }

        public static List<ComputerLogEntity> SearchOnDemand(int limit)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.ComputerLogRepository.Get(x => x.ComputerId == -1, orderBy: (q => q.OrderByDescending(x => x.LogTime))).Take(limit).ToList();
            }
        }

        public static ActionResultEntity DeleteComputerLogs(int computerId)
        {
            var computer = GetComputerLog(computerId);
            if (computer == null)
            {
                var message = string.Format("Could Not Delete Computer Logs For Computer With Id {0}.  The Computer Was not Found", computerId);
                Logger.Log(message);
                return new ActionResultEntity() { Success = false, Message = message, ObjectId = computerId };
            }
            var actionResult = new ActionResultEntity();
            using (var uow = new UnitOfWork())
            {
                uow.ComputerLogRepository.DeleteRange(x => x.ComputerId == computerId);
                actionResult.Success = uow.Save();
                actionResult.ObjectId = computerId;
                actionResult.Object = JsonConvert.SerializeObject(computer);
            }

            return actionResult;
        }
    }
}