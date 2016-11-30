using System;
using System.Collections.Generic;
using System.Linq;
using CloneDeploy_App.Helpers;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using Newtonsoft.Json;

namespace CloneDeploy_Services
{

    public class ActiveImagingTaskServices
    {
         private readonly UnitOfWork _uow;

        public ActiveImagingTaskServices()
        {
            _uow = new UnitOfWork();
        }

        public ActionResultDTO DeleteActiveImagingTask(int activeImagingTaskId)
        {
            var activeImagingTask = _uow.ActiveImagingTaskRepository.GetById(activeImagingTaskId);
            if (activeImagingTask == null) return new ActionResultDTO() { ErrorMessage = "Task Not Found", Id = 0 };
            var computer = _uow.ComputerRepository.GetById(activeImagingTask.ComputerId);
            
            _uow.ActiveImagingTaskRepository.Delete(activeImagingTask.Id);
            _uow.Save();

            var actionResult = new ActionResultDTO();
            actionResult.Success = true;
            actionResult.Id = activeImagingTaskId;

            new CloneDeploy_App.BLL.Workflows.CleanTaskBootFiles(computer).CleanPxeBoot();

            return actionResult;

        }

        public  void SendTaskCompletedEmail(ActiveImagingTaskEntity task)
        {
            //Mail not enabled
            if (Settings.SmtpEnabled == "0") return;
            task.Computer = new ComputerServices().GetComputer(task.ComputerId);
            foreach (var user in CloneDeploy_App.BLL.User.SearchUsers("").Where(x => x.NotifyComplete == 1 && !string.IsNullOrEmpty(x.Email)))
            {
                if (task.UserId == user.Id)
                {
                    var mail = new CloneDeploy_App.Helpers.Mail
                    {
                        MailTo = user.Email,
                        Body = task.Computer.Name + " Image Task Has Completed.",
                        Subject = "Task Completed"
                    };
                    mail.Send();
                }
            }
        }

        public  void SendTaskErrorEmail(ActiveImagingTaskEntity task, string error)
        {
            //Mail not enabled
            if (Settings.SmtpEnabled == "0") return;
            task.Computer = new ComputerServices().GetComputer(task.ComputerId);
            foreach (var user in CloneDeploy_App.BLL.User.SearchUsers("").Where(x => x.NotifyError == 1 && !string.IsNullOrEmpty(x.Email)))
            {
                if (task.UserId == user.Id)
                {
                    var mail = new CloneDeploy_App.Helpers.Mail
                    {
                        MailTo = user.Email,
                        Body = task.Computer.Name + " Image Task Has Failed. " + error,
                        Subject = "Task Failed"
                    };
                    mail.Send();
                }
            }
        }

        public  bool AddActiveImagingTask(ActiveImagingTaskEntity activeImagingTask)
        {
           
                _uow.ActiveImagingTaskRepository.Insert(activeImagingTask);
                _uow.Save();
                return true;
            
        }

        public  void DeleteForMulticast(int multicastId)
        {
            
                _uow.ActiveImagingTaskRepository.DeleteRange(t => t.MulticastId == multicastId);
                _uow.Save();
            
        }

        public  bool UpdateActiveImagingTask(ActiveImagingTaskEntity activeImagingTask)
        {
           
                _uow.ActiveImagingTaskRepository.Update(activeImagingTask, activeImagingTask.Id);
                _uow.Save();
                return true;
            
        }

        public  List<ActiveImagingTaskEntity> MulticastMemberStatus(int multicastId)
        {
            
                var activeImagingTasks = _uow.ActiveImagingTaskRepository.Get(t => t.MulticastId == multicastId,
                    orderBy: q => q.OrderBy(t => t.ComputerId));
                foreach (var task in activeImagingTasks)
                {
                    task.Computer = new ComputerServices().GetComputer(task.ComputerId);
                }
                return activeImagingTasks;
            
        }

        public  List<ActiveImagingTaskEntity> MulticastProgress(int multicastId)
        {
           
                return _uow.ActiveImagingTaskRepository.MulticastProgress(multicastId);
            
        }

        public  List<ActiveImagingTaskEntity> ReadAll(int userId)
        {

            
                //Admins see all tasks
                var activeImagingTasks = CloneDeploy_App.BLL.User.IsAdmin(userId)
                    ? _uow.ActiveImagingTaskRepository.Get(orderBy: q => q.OrderBy(t => t.Id))
                    : _uow.ActiveImagingTaskRepository.Get(x => x.UserId == userId, orderBy: q => q.OrderBy(t => t.Id));
                foreach (var task in activeImagingTasks)
                {
                    task.Computer = new ComputerServices().GetComputer(task.ComputerId);
                }
                return activeImagingTasks;
            
        }

        public  string ActiveUnicastCount(int userId, string taskType)
        {
           
                return CloneDeploy_App.BLL.User.IsAdmin(userId)
                    ? _uow.ActiveImagingTaskRepository.Count(t => t.Type == taskType)
                    : _uow.ActiveImagingTaskRepository.Count(t => t.Type == taskType && t.UserId == userId);
            
        }

        public  string AllActiveCount(int userId)
        {
           
                return CloneDeploy_App.BLL.User.IsAdmin(userId)
                    ? _uow.ActiveImagingTaskRepository.Count()
                    : _uow.ActiveImagingTaskRepository.Count(x => x.UserId == userId);
            
        }

        public  int AllActiveCountAdmin()
        {
            
                return Convert.ToInt32(_uow.ActiveImagingTaskRepository.Count());
            
        }

        public  List<ActiveImagingTaskEntity> ReadUnicasts(int userId, string taskType)
        {
           
                //Admins see all tasks
                List<ActiveImagingTaskEntity> activeImagingTasks;
                if (CloneDeploy_App.BLL.User.IsAdmin(userId))
                {
                    activeImagingTasks = _uow.ActiveImagingTaskRepository.Get(t => t.Type == taskType,
                        orderBy: q => q.OrderBy(t => t.ComputerId));
                }
                else
                {
                    activeImagingTasks = _uow.ActiveImagingTaskRepository.Get(t => t.Type == taskType && t.UserId == userId,
                        orderBy: q => q.OrderBy(t => t.ComputerId));
                }
                foreach (var task in activeImagingTasks)
                {
                    task.Computer = new ComputerServices().GetComputer(task.ComputerId);
                }
                return activeImagingTasks;
            
        }

        public  List<ComputerEntity> GetMulticastComputers(int multicastId)
        {
           
                return _uow.ActiveImagingTaskRepository.MulticastComputers(multicastId);
            
        }

     

        public  ActiveImagingTaskEntity GetTask(int taskId)
        {
           
                return _uow.ActiveImagingTaskRepository.GetById(taskId);
            
        }
        public  void DeleteAll()
        {
           
                _uow.ActiveImagingTaskRepository.DeleteRange();
                _uow.Save();
            
        }

        public  int GetCurrentQueue(string qType)
        {
            
                return
                    Convert.ToInt32(_uow.ActiveImagingTaskRepository.Count(x => x.Status == "3" && x.Type == qType));

            
        }

        public  ActiveImagingTaskEntity GetLastQueuedTask(string qType)
        {
           
                return
                    _uow.ActiveImagingTaskRepository.Get(x => x.Status == "2" && x.Type == qType,
                        orderBy: q => q.OrderByDescending(t => t.QueuePosition)).FirstOrDefault();
            
        }

       

        public  ActiveImagingTaskEntity GetNextComputerInQueue(string qType)
        {
            
                return
                    _uow.ActiveImagingTaskRepository.Get(x => x.Status == "2" && x.Type == qType,
                        orderBy: q => q.OrderBy(t => t.QueuePosition)).FirstOrDefault();
            
        }
    }
}