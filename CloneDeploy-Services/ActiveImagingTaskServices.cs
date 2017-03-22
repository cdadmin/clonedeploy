using System;
using System.Collections.Generic;
using System.Linq;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services.Helpers;
using CloneDeploy_Services.Workflows;

namespace CloneDeploy_Services
{

    public class ActiveImagingTaskServices
    {
         private readonly UnitOfWork _uow;
        private readonly UserServices _userServices;

        public ActiveImagingTaskServices()
        {
            _uow = new UnitOfWork();
            _userServices = new UserServices();
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

            new CleanTaskBootFiles(computer).CleanPxeBoot();

            return actionResult;

        }

        //be
        public  void SendTaskCompletedEmail(ActiveImagingTaskEntity task)
        {
            //Mail not enabled
            if (Settings.SmtpEnabled == "0") return;
            task.Computer = new ComputerServices().GetComputer(task.ComputerId);
            foreach (var user in _userServices.SearchUsers("").Where(x => x.NotifyComplete == 1 && !string.IsNullOrEmpty(x.Email)))
            {
                if (task.UserId == user.Id)
                {
                    var mail = new Mail
                    {
                        MailTo = user.Email,
                        Body = task.Computer.Name + " Image Task Has Completed.",
                        Subject = "Task Completed"
                    };
                    mail.Send();
                }
            }
        }
        //be
        public  void SendTaskErrorEmail(ActiveImagingTaskEntity task, string error)
        {
            //Mail not enabled
            if (Settings.SmtpEnabled == "0") return;
            task.Computer = new ComputerServices().GetComputer(task.ComputerId);
            foreach (var user in _userServices.SearchUsers("").Where(x => x.NotifyError == 1 && !string.IsNullOrEmpty(x.Email)))
            {
                if (task.UserId == user.Id)
                {
                    var mail = new Mail
                    {
                        MailTo = user.Email,
                        Body = task.Computer.Name + " Image Task Has Failed. " + error,
                        Subject = "Task Failed"
                    };
                    mail.Send();
                }
            }
        }

        //be
        public  bool AddActiveImagingTask(ActiveImagingTaskEntity activeImagingTask)
        {
           
                _uow.ActiveImagingTaskRepository.Insert(activeImagingTask);
                _uow.Save();
                return true;
            
        }

        //be
        public  void DeleteForMulticast(int multicastId)
        {
            
                _uow.ActiveImagingTaskRepository.DeleteRange(t => t.MulticastId == multicastId);
                _uow.Save();
            
        }

        //be
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
                var activeImagingTasks = _userServices.IsAdmin(userId)
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
           
                return _userServices.IsAdmin(userId)
                    ? _uow.ActiveImagingTaskRepository.Count(t => t.Type == taskType)
                    : _uow.ActiveImagingTaskRepository.Count(t => t.Type == taskType && t.UserId == userId);
            
        }

        public  string AllActiveCount(int userId)
        {
           
                return _userServices.IsAdmin(userId)
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
                if (_userServices.IsAdmin(userId))
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

        public List<ActiveImagingTaskEntity> GetAll()
        {
            return _uow.ActiveImagingTaskRepository.Get();
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

        public  int GetCurrentQueue(ActiveImagingTaskEntity activeTask)
        {
            
                return
                    Convert.ToInt32(_uow.ActiveImagingTaskRepository.Count(x => x.Status == "3" && x.Type == activeTask.Type && x.DpId == activeTask.DpId));

            
        }

        public  ActiveImagingTaskEntity GetLastQueuedTask(ActiveImagingTaskEntity activeTask)
        {
           
                return
                    _uow.ActiveImagingTaskRepository.Get(x => x.Status == "2" && x.Type == activeTask.Type && x.DpId == activeTask.DpId,
                        orderBy: q => q.OrderByDescending(t => t.QueuePosition)).FirstOrDefault();
            
        }

       

        public  ActiveImagingTaskEntity GetNextComputerInQueue(ActiveImagingTaskEntity activeTask)
        {
            
                return
                    _uow.ActiveImagingTaskRepository.Get(x => x.Status == "2" && x.Type == activeTask.Type && x.DpId == activeTask.DpId,
                        orderBy: q => q.OrderBy(t => t.QueuePosition)).FirstOrDefault();
            
        }
    }
}