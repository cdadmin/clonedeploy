using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public static class ComputerLog
    {
        public static bool AddComputerLog(Models.ComputerLog computerLog)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.ComputerLogRepository.Insert(computerLog);
                return uow.Save();
            }
        }

        public static string TotalCount()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ComputerLogRepository.Count();
            }
        }

        public static bool DeleteComputerLog(int computerLogId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.ComputerLogRepository.Delete(computerLogId);
                return uow.Save();
            }
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

        public static List<Models.ComputerLog> SearchOnDemand()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ComputerLogRepository.Get(x => x.ComputerId == -1, orderBy: (q => q.OrderByDescending(x => x.LogTime)));
            }
        }

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