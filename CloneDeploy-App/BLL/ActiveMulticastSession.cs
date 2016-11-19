using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Threading;
using CloneDeploy_App.Helpers;

namespace CloneDeploy_App.BLL
{
    public class ActiveMulticastSession
    {

        public static bool AddActiveMulticastSession(Models.ActiveMulticastSession activeMulticastSession)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                if (uow.ActiveMulticastSessionRepository.Exists(h => h.Name == activeMulticastSession.Name))
                {
                    //Message.Text = "A Multicast Is Already Running For This Group";
                    return false;
                }
                uow.ActiveMulticastSessionRepository.Insert(activeMulticastSession);
                if (uow.Save())
                {
                    //Message.Text = "Successfully Created Multicast";
                    return true;
                }
                else
                {
                    //Message.Text = "Could Not Create Multicast";
                    return false;
                }
            }
        }

        public static void SendMulticastCompletedEmail(Models.ActiveMulticastSession session)
        {
            //Mail not enabled
            if (Settings.SmtpEnabled == "0") return;

            foreach (var user in BLL.User.SearchUsers("").Where(x => x.NotifyComplete == 1 && !string.IsNullOrEmpty(x.Email)))
            {
                if (session.UserId == user.Id)
                {
                    var mail = new Helpers.Mail
                    {
                        MailTo = user.Email,
                        Body = session.Name + " Multicast Task Has Completed.",
                        Subject = "Multicast Completed"
                    };
                    mail.Send();
                }
            }
        }
        public static Models.ActiveMulticastSession GetFromPort(int port)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ActiveMulticastSessionRepository.GetFirstOrDefault(x => x.Port == port);
            }
        }

        public static bool Delete(int multicastId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var multicast = uow.ActiveMulticastSessionRepository.GetById(multicastId);
                var computers = uow.ActiveImagingTaskRepository.MulticastComputers(multicastId);

                uow.ActiveMulticastSessionRepository.Delete(multicastId);
                if (uow.Save())
                {
                    ActiveImagingTask.DeleteForMulticast(multicastId);

                    foreach (var computer in computers)
                        new BLL.Workflows.CleanTaskBootFiles(computer).CleanPxeBoot();

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
                    }
                    return true;
                }
                else
                {
                    //Message.Text = "Could Not Delete Task";
                    return false;
                }
            }
        }

        public static Models.ActiveMulticastSession Get(int multicastId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ActiveMulticastSessionRepository.GetById(multicastId);
            }
        }

        public static List<Models.ActiveMulticastSession> GetOnDemandList()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ActiveMulticastSessionRepository.Get(x => x.ImageProfileId != -1, orderBy: (q => q.OrderBy(t => t.Name)));      
            }
        }

        public static bool UpdateActiveMulticastSession(Models.ActiveMulticastSession activeMulticastSession)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.ActiveMulticastSessionRepository.Update(activeMulticastSession, activeMulticastSession.Id);
                return uow.Save();
            }
        }

        public static List<Models.ActiveMulticastSession> GetAllMulticastSessions(int userId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                if(BLL.User.IsAdmin(userId))
                return uow.ActiveMulticastSessionRepository.Get(orderBy: (q => q.OrderBy(t => t.Name)));
                else
                {
                    return uow.ActiveMulticastSessionRepository.Get(x => x.UserId == userId, orderBy: (q => q.OrderBy(t => t.Name)));
                }
            }
        }

        public static string ActiveCount(int userId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return BLL.User.IsAdmin(userId)
                    ? uow.ActiveMulticastSessionRepository.Count()
                    : uow.ActiveMulticastSessionRepository.Count(x => x.UserId == userId);

            }
        }

        public static void DeleteAll()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.ActiveMulticastSessionRepository.DeleteRange();
                uow.Save();
            }
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