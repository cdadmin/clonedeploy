using System.Collections.Generic;
using System.Linq;
using CloneDeploy_App.Helpers;
using CloneDeploy_App.Models;
using Newtonsoft.Json;

namespace CloneDeploy_App.BLL
{
    public static class ComputerLog
    {
        public static ActionResult AddComputerLog(Models.ComputerLog computerLog)
        {
            var actionResult = new ActionResult();
            using (var uow = new DAL.UnitOfWork())
            {
                uow.ComputerLogRepository.Insert(computerLog);
                actionResult.Success = uow.Save();
                actionResult.ObjectId = computerLog.Id;
            }
            return actionResult;
        }

        public static ActionResult DeleteComputerLog(int computerLogId)
        {
            var computerLog = GetComputerLog(computerLogId);
            if (computerLog == null)
            {
                var message = string.Format("Could Not Delete Computer Log With Id {0}.  The Computer Log Was not Found", computerLogId);
                Logger.Log(message);
                return new ActionResult() { Success = false, Message = message, ObjectId = computerLogId };
            }
            var actionResult = new ActionResult();
            using (var uow = new DAL.UnitOfWork())
            {
                uow.ComputerLogRepository.Delete(computerLogId);
                actionResult.Success = uow.Save();
                actionResult.ObjectId = computerLogId;
            }
            return actionResult;
        }

        public static Models.ComputerLog GetComputerLog(int computerLogId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ComputerLogRepository.GetById(computerLogId);
            }
        }

      
        public static List<Models.ComputerLog> Search(int computerId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ComputerLogRepository.Get(x => x.ComputerId == computerId, orderBy:(q => q.OrderByDescending(x => x.LogTime)));
            }
        }

        public static List<Models.ComputerLog> SearchOnDemand(int limit)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ComputerLogRepository.Get(x => x.ComputerId == -1, orderBy: (q => q.OrderByDescending(x => x.LogTime))).Take(limit).ToList();
            }
        }

        public static ActionResult DeleteComputerLogs(int computerId)
        {
            var computer = GetComputerLog(computerId);
            if (computer == null)
            {
                var message = string.Format("Could Not Delete Computer Logs For Computer With Id {0}.  The Computer Was not Found", computerId);
                Logger.Log(message);
                return new ActionResult() { Success = false, Message = message, ObjectId = computerId };
            }
            var actionResult = new ActionResult();
            using (var uow = new DAL.UnitOfWork())
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