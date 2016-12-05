using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Threading;
using CloneDeploy_App.Helpers;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using Newtonsoft.Json;

namespace CloneDeploy_Services
{
    public class ActiveMulticastSessionServices
    {
         private readonly UnitOfWork _uow;
        private readonly UserServices _userServices;
        public ActiveMulticastSessionServices()
        {
            _uow = new UnitOfWork();
            _userServices = new UserServices();
        }

        public  bool AddActiveMulticastSession(ActiveMulticastSessionEntity activeMulticastSession)
        {
          
                if (_uow.ActiveMulticastSessionRepository.Exists(h => h.Name == activeMulticastSession.Name))
                {
                    //Message.Text = "A Multicast Is Already Running For This Group";
                    return false;
                }
                _uow.ActiveMulticastSessionRepository.Insert(activeMulticastSession);
                _uow.Save();
                return true;
               
                
            
        }

        public  void SendMulticastCompletedEmail(ActiveMulticastSessionEntity session)
        {
            //Mail not enabled
            if (Settings.SmtpEnabled == "0") return;

            foreach (var user in _userServices.SearchUsers("").Where(x => x.NotifyComplete == 1 && !string.IsNullOrEmpty(x.Email)))
            {
                if (session.UserId == user.Id)
                {
                    var mail = new CloneDeploy_App.Helpers.Mail
                    {
                        MailTo = user.Email,
                        Body = session.Name + " Multicast Task Has Completed.",
                        Subject = "Multicast Completed"
                    };
                    mail.Send();
                }
            }
        }
        public  ActiveMulticastSessionEntity GetFromPort(int port)
        {
            
                return _uow.ActiveMulticastSessionRepository.GetFirstOrDefault(x => x.Port == port);
            
        }

        public ActionResultDTO Delete(int multicastId)
        {
            var multicast = _uow.ActiveMulticastSessionRepository.GetById(multicastId);
            if (multicast == null) return new ActionResultDTO() {ErrorMessage = "Multicast Not Found", Id = 0};
            var computers = _uow.ActiveImagingTaskRepository.MulticastComputers(multicastId);

            var actionResult = new ActionResultDTO();
            _uow.ActiveMulticastSessionRepository.Delete(multicastId);
            _uow.Save();
            actionResult.Id = multicast.Id;
            actionResult.Success = true;

            new ActiveImagingTaskServices().DeleteForMulticast(multicastId);

            foreach (var computer in computers)
                new CloneDeploy_App.BLL.Workflows.CleanTaskBootFiles(computer).CleanPxeBoot();

            try
            {
                var prs = Process.GetProcessById(Convert.ToInt32(multicast.Pid));
                var processName = prs.ProcessName;
                if (Environment.OSVersion.ToString().Contains("Unix"))
                {
                    for (var x = 1; x <= 5; x++)
                    {
                        KillProcessLinux(Convert.ToInt32(multicast.Pid));
                        Thread.Sleep(200);
                    }
                }
                else
                {
                    if (processName == "cmd")
                        KillProcess(Convert.ToInt32(multicast.Pid));
                }
                //Message.Text = "Successfully Deleted Task";
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString());
                //Message.Text = "Could Not Kill Process.  Check The Exception Log For More Info";
                actionResult.Success = false;
            }


            return actionResult;


        }

        public  ActiveMulticastSessionEntity Get(int multicastId)
        {
            
                return _uow.ActiveMulticastSessionRepository.GetById(multicastId);
            
        }

        public  List<ActiveMulticastSessionEntity> GetOnDemandList()
        {
            
                return _uow.ActiveMulticastSessionRepository.Get(x => x.ImageProfileId != -1, orderBy: (q => q.OrderBy(t => t.Name)));      
            
        }

        public  bool UpdateActiveMulticastSession(ActiveMulticastSessionEntity activeMulticastSession)
        {
           
                _uow.ActiveMulticastSessionRepository.Update(activeMulticastSession, activeMulticastSession.Id);
                _uow.Save();
                return true;
            
        }

        public  List<ActiveMulticastSessionEntity> GetAllMulticastSessions(int userId)
        {
          
                if(_userServices.IsAdmin(userId))
                return _uow.ActiveMulticastSessionRepository.Get(orderBy: (q => q.OrderBy(t => t.Name)));
                else
                {
                    return _uow.ActiveMulticastSessionRepository.Get(x => x.UserId == userId, orderBy: (q => q.OrderBy(t => t.Name)));
                }
            
        }

        public  string ActiveCount(int userId)
        {
           
                return _userServices.IsAdmin(userId)
                    ? _uow.ActiveMulticastSessionRepository.Count()
                    : _uow.ActiveMulticastSessionRepository.Count(x => x.UserId == userId);

            
        }

        public  void DeleteAll()
        {
           
                _uow.ActiveMulticastSessionRepository.DeleteRange();
                _uow.Save();
            
        }

        public static void KillProcess(int pid)
        {
            var searcher =
                new ManagementObjectSearcher("Select * From Win32_Process Where ParentProcessID=" + pid);
            var moc = searcher.Get();
            foreach (var o in moc)
            {
                var mo = (ManagementObject)o;
                KillProcess(Convert.ToInt32(mo["ProcessID"]));
            }
            try
            {
                var proc = Process.GetProcessById(Convert.ToInt32(pid));
                proc.Kill();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString());
            }
        }

        public static void KillProcessLinux(int pid)
        {
            try
            {
             
                var killProcInfo = new ProcessStartInfo
                {
                    FileName = ("pkill"),
                    Arguments = (" -SIGKILL -P " + pid)
                };
                Process.Start(killProcInfo);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString());
            }
        }
    }
}