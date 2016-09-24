using System;
using System.Collections.Generic;
using System.Linq;
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

        public static void SendTaskCompletedEmail(Models.ActiveImagingTask task)
        {
            //Mail not enabled
            if (Settings.SmtpEnabled == "0") return;
            task.Computer = BLL.Computer.GetComputer(task.ComputerId);
            foreach (var user in BLL.User.SearchUsers("").Where(x => x.NotifyComplete == 1 && !string.IsNullOrEmpty(x.Email)))
            {
                if (task.UserId == user.Id)
                {
                    var mail = new Helpers.Mail
                    {
                        MailTo = user.Email,
                        Body = task.Computer.Name + " Image Task Has Completed.",
                        Subject = "Task Completed"
                    };
                    mail.Send();
                }
            }
        }

        public static void SendTaskErrorEmail(Models.ActiveImagingTask task, string error)
        {
            //Mail not enabled
            if (Settings.SmtpEnabled == "0") return;
            task.Computer = BLL.Computer.GetComputer(task.ComputerId);
            foreach (var user in BLL.User.SearchUsers("").Where(x => x.NotifyError == 1 && !string.IsNullOrEmpty(x.Email)))
            {
                if (task.UserId == user.Id)
                {
                    var mail = new Helpers.Mail
                    {
                        MailTo = user.Email,
                        Body = task.Computer.Name + " Image Task Has Failed. " + error,
                        Subject = "Task Failed"
                    };
                    mail.Send();
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

        public static List<Models.ActiveImagingTask> ReadAll(int userId)
        {

            using (var uow = new DAL.UnitOfWork())
            {
                //Admins see all tasks
                var activeImagingTasks = BLL.User.IsAdmin(userId)
                    ? uow.ActiveImagingTaskRepository.Get(orderBy: q => q.OrderBy(t => t.Id))
                    : uow.ActiveImagingTaskRepository.Get(x => x.UserId == userId, orderBy: q => q.OrderBy(t => t.Id));
                foreach (var task in activeImagingTasks)
                {
                    task.Computer = BLL.Computer.GetComputer(task.ComputerId);
                }
                return activeImagingTasks;
            }
        }

        public static string ActiveUnicastCount(int userId, string taskType)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return BLL.User.IsAdmin(userId)
                    ? uow.ActiveImagingTaskRepository.Count(t => t.Type == taskType)
                    : uow.ActiveImagingTaskRepository.Count(t => t.Type == taskType && t.UserId == userId);
            }
        }

        public static string AllActiveCount(int userId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return BLL.User.IsAdmin(userId)
                    ? uow.ActiveImagingTaskRepository.Count()
                    : uow.ActiveImagingTaskRepository.Count(x => x.UserId == userId);
            }
        }

        public static int AllActiveCountAdmin()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return Convert.ToInt32(uow.ActiveImagingTaskRepository.Count());
            }
        }

        public static List<Models.ActiveImagingTask> ReadUnicasts(int userId, string taskType)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                //Admins see all tasks
                List<Models.ActiveImagingTask> activeImagingTasks;
                if (BLL.User.IsAdmin(userId))
                {
                    activeImagingTasks = uow.ActiveImagingTaskRepository.Get(t => t.Type == taskType,
                        orderBy: q => q.OrderBy(t => t.ComputerId));
                }
                else
                {
                    activeImagingTasks = uow.ActiveImagingTaskRepository.Get(t => t.Type == taskType && t.UserId == userId,
                        orderBy: q => q.OrderBy(t => t.ComputerId));
                }
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

        public static int GetCurrentQueue(string qType)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return
                    Convert.ToInt32(uow.ActiveImagingTaskRepository.Count(x => x.Status == "3" && x.Type == qType));

            }
        }

        public static Models.ActiveImagingTask GetLastQueuedTask(string qType)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return
                    uow.ActiveImagingTaskRepository.Get(x => x.Status == "2" && x.Type == qType,
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

        public static Models.ActiveImagingTask GetNextComputerInQueue(string qType)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return
                    uow.ActiveImagingTaskRepository.Get(x => x.Status == "2" && x.Type == qType,
                        orderBy: q => q.OrderBy(t => t.QueuePosition)).FirstOrDefault();
            }
        }
    }
}