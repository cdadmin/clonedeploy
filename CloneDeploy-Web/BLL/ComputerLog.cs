using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public static class ComputerLog
    {
        //moved
        public static bool AddComputerLog(CloneDeploy_Web.Models.ComputerLog computerLog)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.ComputerLogRepository.Insert(computerLog);
                return uow.Save();
            }
        }

        //moved
        public static string TotalCount()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ComputerLogRepository.Count();
            }
        }

        //moved
        public static bool DeleteComputerLog(int computerLogId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.ComputerLogRepository.Delete(computerLogId);
                return uow.Save();
            }
        }

        //moved
        public static CloneDeploy_Web.Models.ComputerLog GetComputerLog(int computerLogId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ComputerLogRepository.GetById(computerLogId);
            }
        }

      
        //moved
        public static List<CloneDeploy_Web.Models.ComputerLog> Search(int computerId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ComputerLogRepository.Get(x => x.ComputerId == computerId, orderBy:(q => q.OrderByDescending(x => x.LogTime)));
            }
        }

        //moved
        public static List<CloneDeploy_Web.Models.ComputerLog> SearchOnDemand(int limit)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ComputerLogRepository.Get(x => x.ComputerId == -1, orderBy: (q => q.OrderByDescending(x => x.LogTime))).Take(limit).ToList();
            }
        }

        //moved
        public static bool DeleteComputerLogs(int computerId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.ComputerLogRepository.DeleteRange(x => x.ComputerId == computerId);
                return uow.Save();
            }
        }
    }
}