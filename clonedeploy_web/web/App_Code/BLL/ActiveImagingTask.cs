using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Workflows;
using DAL;
using Helpers;

namespace BLL
{

    public class ActiveImagingTask
    {

        public static bool IsComputerActive(int computerId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ActiveImagingTaskRepository.Exists(a => a.ComputerId == computerId);
            }
        }

        public static bool DeleteActiveImagingTask(int activeImagingTaskId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var activeImagingTask = uow.ActiveImagingTaskRepository.GetById(activeImagingTaskId);
                var computer = uow.ComputerRepository.GetById(activeImagingTask.ComputerId);

                uow.ActiveImagingTaskRepository.Delete(activeImagingTask.Id);
                if (uow.Save())
                {
                    new BLL.Workflows.CleanTaskBootFiles(computer).CleanPxeBoot();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool AddActiveImagingTask(Models.ActiveImagingTask activeImagingTask)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.ActiveImagingTaskRepository.Insert(activeImagingTask);
                return uow.Save();
            }
        }

        public static void DeleteForMulticast(int multicastId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.ActiveImagingTaskRepository.DeleteRange(t => t.MulticastId == multicastId);
                uow.Save();
            }
        }

        public static bool UpdateActiveImagingTask(Models.ActiveImagingTask activeImagingTask)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.ActiveImagingTaskRepository.Update(activeImagingTask, activeImagingTask.Id);
                return uow.Save();
            }
        }

        public static List<Models.ActiveImagingTask> MulticastMemberStatus(int multicastId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var activeImagingTasks = uow.ActiveImagingTaskRepository.Get(t => t.MulticastId == multicastId,
                    orderBy: q => q.OrderBy(t => t.ComputerId));
                foreach (var task in activeImagingTasks)
                {
                    task.Computer = BLL.Computer.GetComputer(task.ComputerId);
                }
                return activeImagingTasks;
            }
        }

        public static List<Models.ActiveImagingTask> MulticastProgress(int multicastId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ActiveImagingTaskRepository.MulticastProgress(multicastId);
            }
        }

        public static List<Models.ActiveImagingTask> ReadAll()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var activeImagingTasks = uow.ActiveImagingTaskRepository.Get(orderBy: q => q.OrderBy(t => t.Id));
                foreach (var task in activeImagingTasks)
                {
                    task.Computer = BLL.Computer.GetComputer(task.ComputerId);
                }
                return activeImagingTasks;
            }
        }

        public static string ActiveUnicastCount()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ActiveImagingTaskRepository.Count(t => t.Type == "unicast");
            }
        }

        public static string AllActiveCount()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ActiveImagingTaskRepository.Count();
            }
        }

        public static List<Models.ActiveImagingTask> ReadUnicasts()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var activeImagingTasks = uow.ActiveImagingTaskRepository.Get(t => t.Type == "unicast",
                    orderBy: q => q.OrderBy(t => t.ComputerId));
                foreach (var task in activeImagingTasks)
                {
                    task.Computer = BLL.Computer.GetComputer(task.ComputerId);
                }
                return activeImagingTasks;
            }
        }

        public static List<Models.Computer> GetMulticastComputers(int multicastId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ActiveImagingTaskRepository.MulticastComputers(multicastId);
            }
        }

        public static Models.ActiveImagingTask GetTask(int computerId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ActiveImagingTaskRepository.GetFirstOrDefault(x => x.ComputerId == computerId);
            }
        }
        public static void DeleteAll()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.ActiveImagingTaskRepository.DeleteRange();
                uow.Save();
            }
        }

        public static int GetCurrentQueue()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return
                    Convert.ToInt32(uow.ActiveImagingTaskRepository.Count(x => x.Status == "3" && x.Type == "unicast"));

            }
        }

        public static Models.ActiveImagingTask GetLastQueuedTask()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return
                    uow.ActiveImagingTaskRepository.Get(x => x.Status == "2" && x.Type == "unicast",
                        orderBy: q => q.OrderByDescending(t => t.QueuePosition)).FirstOrDefault();
            }
        }

        public static string GetQueuePosition(int computerId)
        {
            var computerTask = GetTask(computerId);
            using (var uow = new DAL.UnitOfWork())
            {
                return
                    uow.ActiveImagingTaskRepository.Count(
                        x => x.Status == "2" && x.QueuePosition < computerTask.QueuePosition);
            }
        }

        public static Models.ActiveImagingTask GetNextComputerInQueue()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return
                    uow.ActiveImagingTaskRepository.Get(x => x.Status == "2" && x.Type == "unicast",
                        orderBy: q => q.OrderBy(t => t.QueuePosition)).FirstOrDefault();
            }
        }
    }
}