using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;

namespace BLL
{

    public class ActiveImagingTask
    {

        //moved
        public static bool IsComputerActive(int computerId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ActiveImagingTaskRepository.Exists(a => a.ComputerId == computerId);
            }
        }

        //moved
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

        //move not needed
        public static void SendTaskCompletedEmail(CloneDeploy_Web.Models.ActiveImagingTask task)
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

        //move not needed
        public static void SendTaskErrorEmail(CloneDeploy_Web.Models.ActiveImagingTask task, string error)
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

        //move not needed
        public static bool AddActiveImagingTask(CloneDeploy_Web.Models.ActiveImagingTask activeImagingTask)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.ActiveImagingTaskRepository.Insert(activeImagingTask);
                return uow.Save();
            }
        }

        //move not needed
        public static void DeleteForMulticast(int multicastId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.ActiveImagingTaskRepository.DeleteRange(t => t.MulticastId == multicastId);
                uow.Save();
            }
        }

        //move not needed
        public static bool UpdateActiveImagingTask(CloneDeploy_Web.Models.ActiveImagingTask activeImagingTask)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.ActiveImagingTaskRepository.Update(activeImagingTask, activeImagingTask.Id);
                return uow.Save();
            }
        }

        //moved
        public static List<CloneDeploy_Web.Models.ActiveImagingTask> MulticastMemberStatus(int multicastId)
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


        //moved
        public static List<CloneDeploy_Web.Models.ActiveImagingTask> MulticastProgress(int multicastId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ActiveImagingTaskRepository.MulticastProgress(multicastId);
            }
        }

        //moved
        public static List<CloneDeploy_Web.Models.ActiveImagingTask> ReadAll(int userId)
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

        //moved
        public static string ActiveUnicastCount(int userId, string taskType)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return BLL.User.IsAdmin(userId)
                    ? uow.ActiveImagingTaskRepository.Count(t => t.Type == taskType)
                    : uow.ActiveImagingTaskRepository.Count(t => t.Type == taskType && t.UserId == userId);
            }
        }

        //moved
        public static string AllActiveCount(int userId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return BLL.User.IsAdmin(userId)
                    ? uow.ActiveImagingTaskRepository.Count()
                    : uow.ActiveImagingTaskRepository.Count(x => x.UserId == userId);
            }
        }

        //move not needed
        public static int AllActiveCountAdmin()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return Convert.ToInt32(uow.ActiveImagingTaskRepository.Count());
            }
        }

        //moved
        public static List<CloneDeploy_Web.Models.ActiveImagingTask> ReadUnicasts(int userId, string taskType)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                //Admins see all tasks
                List<CloneDeploy_Web.Models.ActiveImagingTask> activeImagingTasks;
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

        //moved
        public static List<CloneDeploy_Web.Models.Computer> GetMulticastComputers(int multicastId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ActiveImagingTaskRepository.MulticastComputers(multicastId);
            }
        }

        //move not needed
        public static CloneDeploy_Web.Models.ActiveImagingTask GetTask(int computerId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ActiveImagingTaskRepository.GetFirstOrDefault(x => x.ComputerId == computerId);
            }
        }

        //move not needed
        public static void DeleteAll()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.ActiveImagingTaskRepository.DeleteRange();
                uow.Save();
            }
        }

        //move not needed
        public static int GetCurrentQueue(string qType)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return
                    Convert.ToInt32(uow.ActiveImagingTaskRepository.Count(x => x.Status == "3" && x.Type == qType));

            }
        }

        //move not needed
        public static CloneDeploy_Web.Models.ActiveImagingTask GetLastQueuedTask(string qType)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return
                    uow.ActiveImagingTaskRepository.Get(x => x.Status == "2" && x.Type == qType,
                        orderBy: q => q.OrderByDescending(t => t.QueuePosition)).FirstOrDefault();
            }
        }

        //move not needed
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

        //move not needed
        public static CloneDeploy_Web.Models.ActiveImagingTask GetNextComputerInQueue(string qType)
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