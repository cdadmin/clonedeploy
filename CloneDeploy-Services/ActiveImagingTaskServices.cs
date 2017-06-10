using System;
using System.Collections.Generic;
using System.Linq;
using CloneDeploy_Common;
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

        public string ActiveCountNotOwnedByuser(int userId)
        {
            return _userServices.IsAdmin(userId)
                ? "0"
                : _uow.ActiveImagingTaskRepository.Count(x => x.UserId != userId);
        }

        public string ActiveUnicastCount(int userId, string taskType="")
        {
            if (taskType == "permanentdeploy")
            {
                return _userServices.IsAdmin(userId)
                    ? _uow.ActiveImagingTaskRepository.Count(t => t.Type == taskType)
                    : _uow.ActiveImagingTaskRepository.Count(t => t.Type == taskType && t.UserId == userId);
            }
            else
            {
                return _userServices.IsAdmin(userId)
                  ? _uow.ActiveImagingTaskRepository.Count(t => t.Type == "deploy" || t.Type == "upload")
                  : _uow.ActiveImagingTaskRepository.Count(t => (t.Type == "deploy" || t.Type == "upload") && t.UserId == userId);
            }
        }

        public bool AddActiveImagingTask(ActiveImagingTaskEntity activeImagingTask)
        {
            _uow.ActiveImagingTaskRepository.Insert(activeImagingTask);
            _uow.Save();
            return true;
        }

        public string AllActiveCount(int userId)
        {
            return _userServices.IsAdmin(userId)
                ? _uow.ActiveImagingTaskRepository.Count()
                : _uow.ActiveImagingTaskRepository.Count(x => x.UserId == userId);
        }

        public int AllActiveCountAdmin()
        {
            return Convert.ToInt32(_uow.ActiveImagingTaskRepository.Count());
        }

        public ActionResultDTO DeleteActiveImagingTask(int activeImagingTaskId)
        {
            var activeImagingTask = _uow.ActiveImagingTaskRepository.GetById(activeImagingTaskId);
            if (activeImagingTask == null) return new ActionResultDTO {ErrorMessage = "Task Not Found", Id = 0};
            var computer = _uow.ComputerRepository.GetById(activeImagingTask.ComputerId);

            _uow.ActiveImagingTaskRepository.Delete(activeImagingTask.Id);
            _uow.Save();

            var actionResult = new ActionResultDTO();
            actionResult.Success = true;
            actionResult.Id = activeImagingTaskId;

            new CleanTaskBootFiles(computer).Execute();

            return actionResult;
        }

        public ActionResultDTO DeleteUnregisteredOndTask(int activeImagingTaskId)
        {
            var activeImagingTask = _uow.ActiveImagingTaskRepository.GetById(activeImagingTaskId);
            if (activeImagingTask == null) return new ActionResultDTO { ErrorMessage = "Task Not Found", Id = 0 };
            if(activeImagingTask.ComputerId > -1)
                return new ActionResultDTO { ErrorMessage = "This Task Is Not An On Demand Task And Cannot Be Cancelled", Id = 0 };

         
            _uow.ActiveImagingTaskRepository.Delete(activeImagingTask.Id);
            _uow.Save();

            var actionResult = new ActionResultDTO();
            actionResult.Success = true;
            actionResult.Id = activeImagingTaskId;

            return actionResult;
        }

        public void DeleteAll()
        {
            _uow.ActiveImagingTaskRepository.DeleteRange();
            _uow.Save();
        }

        public void DeleteForMulticast(int multicastId)
        {
            _uow.ActiveImagingTaskRepository.DeleteRange(t => t.MulticastId == multicastId);
            _uow.Save();
        }

        public List<ActiveImagingTaskEntity> GetAll()
        {
            return _uow.ActiveImagingTaskRepository.Get();
        }

        public string GetQueuePosition(ActiveImagingTaskEntity task)
        {

            return
                _uow.ActiveImagingTaskRepository.Count(
                    x => x.Status == "2" && x.QueuePosition < task.QueuePosition);
        }

        public int GetCurrentQueue(ActiveImagingTaskEntity activeTask)
        {
            return
                Convert.ToInt32(
                    _uow.ActiveImagingTaskRepository.Count(
                        x => x.Status == "3" && x.Type == activeTask.Type && x.DpId == activeTask.DpId));
        }

        public ActiveImagingTaskEntity GetLastQueuedTask(ActiveImagingTaskEntity activeTask)
        {
            return
                _uow.ActiveImagingTaskRepository.Get(
                    x => x.Status == "2" && x.Type == activeTask.Type && x.DpId == activeTask.DpId,
                    q => q.OrderByDescending(t => t.QueuePosition)).FirstOrDefault();
        }

        public List<ComputerEntity> GetMulticastComputers(int multicastId)
        {
            return _uow.ActiveImagingTaskRepository.MulticastComputers(multicastId);
        }


        public ActiveImagingTaskEntity GetNextComputerInQueue(ActiveImagingTaskEntity activeTask)
        {
            return
                _uow.ActiveImagingTaskRepository.Get(
                    x => x.Status == "2" && x.Type == activeTask.Type && x.DpId == activeTask.DpId,
                    q => q.OrderBy(t => t.QueuePosition)).FirstOrDefault();
        }

        public ActiveImagingTaskEntity GetTask(int taskId)
        {
            return _uow.ActiveImagingTaskRepository.GetById(taskId);
        }

        public List<TaskWithComputer> MulticastMemberStatus(int multicastId)
        {
            return _uow.ActiveImagingTaskRepository.GetMulticastMembers(multicastId);
        }

        public List<ActiveImagingTaskEntity> MulticastProgress(int multicastId)
        {
            return _uow.ActiveImagingTaskRepository.MulticastProgress(multicastId);
        }

        public List<TaskWithComputer> ReadAll(int userId)
        {
            //Admins see all tasks
            return _userServices.IsAdmin(userId)
                ? _uow.ActiveImagingTaskRepository.GetAllTaskWithComputersForAdmin()
                : _uow.ActiveImagingTaskRepository.GetAllTaskWithComputers(userId);
        }

        public List<TaskWithComputer> ReadUnicasts(int userId)
        {
          
                //Admins see all tasks
                var activeImagingTasks = _userServices.IsAdmin(userId)
                    ? _uow.ActiveImagingTaskRepository.GetUnicastsWithComputersForAdmin()
                    : _uow.ActiveImagingTaskRepository.GetUnicastsWithComputers(userId);
           
            return activeImagingTasks;
        }

        public List<TaskWithComputer> ReadPermanentUnicasts(int userId)
        {

            //Admins see all tasks
            var activeImagingTasks = _userServices.IsAdmin(userId)
                ? _uow.ActiveImagingTaskRepository.GetPermanentUnicastsWithComputersForAdmin()
                : _uow.ActiveImagingTaskRepository.GetPermanentUnicastsWithComputers(userId);

            return activeImagingTasks;
        }

        public List<ActiveImagingTaskEntity> GetAllOnDemandUnregistered()
        {
            return _uow.ActiveImagingTaskRepository.Get(x => x.ComputerId < -1);        
        }

        public int OnDemandCount()
        {
            return Convert.ToInt32(_uow.ActiveImagingTaskRepository.Count(x => x.ComputerId < -1));
        }

        public void SendTaskCompletedEmail(ActiveImagingTaskEntity task)
        {
            //Mail not enabled
            if (SettingServices.GetSettingValue(SettingStrings.SmtpEnabled) == "0") return;
            var computer = new ComputerServices().GetComputer(task.ComputerId);
            foreach (
                var user in
                    _userServices.SearchUsers("").Where(x => x.NotifyComplete == 1 && !string.IsNullOrEmpty(x.Email)))
            {
                if (task.UserId == user.Id)
                {
                    var mail = new MailServices
                    {
                        MailTo = user.Email,
                        Body = computer.Name + " Image Task Has Completed.",
                        Subject = "Task Completed"
                    };
                    mail.Send();
                }
            }
        }

        public void SendTaskErrorEmail(ActiveImagingTaskEntity task, string error)
        {
            //Mail not enabled
            if (SettingServices.GetSettingValue(SettingStrings.SmtpEnabled) == "0") return;
            var computer = new ComputerServices().GetComputer(task.ComputerId);
            foreach (
                var user in
                    _userServices.SearchUsers("").Where(x => x.NotifyError == 1 && !string.IsNullOrEmpty(x.Email)))
            {
                if (task.UserId == user.Id)
                {
                    var mail = new MailServices
                    {
                        MailTo = user.Email,
                        Body = computer.Name + " Image Task Has Failed. " + error,
                        Subject = "Task Failed"
                    };
                    mail.Send();
                }
            }
        }

        public bool UpdateActiveImagingTask(ActiveImagingTaskEntity activeImagingTask)
        {
            _uow.ActiveImagingTaskRepository.Update(activeImagingTask, activeImagingTask.Id);
            _uow.Save();
            return true;
        }
    }
}