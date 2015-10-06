using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Threading;
using DAL;
using Helpers;
using Pxe;

namespace BLL
{
    public class ActiveMulticastSession
    {

        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public bool AddActiveMulticastSession(Models.ActiveMulticastSession activeMulticastSession)
        {
            if (_unitOfWork.ActiveMulticastSessionRepository.Exists(h => h.Name == activeMulticastSession.Name))
            {
                Message.Text = "A Multicast Is Already Running For This Group";
                return false;
            }
            _unitOfWork.ActiveMulticastSessionRepository.Insert(activeMulticastSession);
            if (_unitOfWork.Save())
            {
                Message.Text = "Successfully Created Multicast";
                return true;
            }
            else
            {
                Message.Text = "Could Not Create Multicast";
                return false;
            }
        }

        public bool Delete(int multicastId)
        {
            var multicast = _unitOfWork.ActiveMulticastSessionRepository.GetById(multicastId);
            var computers = _unitOfWork.ActiveImagingTaskRepository.MulticastComputers(multicastId);

            _unitOfWork.ActiveMulticastSessionRepository.Delete(multicastId);
            if (_unitOfWork.Save())
            {
                new ActiveImagingTask().DeleteForMulticast(multicastId);

                foreach (var computer in computers)
                    new PxeFileOps().CleanPxeBoot(Utility.MacToPxeMac(computer.Mac));

                try
                {
                    var prs = Process.GetProcessById(Convert.ToInt32(multicast.Pid));
                    var processName = prs.ProcessName;
                    if (Environment.OSVersion.ToString().Contains("Unix"))
                    {
                        while (!prs.HasExited)
                        {
                            KillProcessLinux(Convert.ToInt32(multicast.Pid));
                            Thread.Sleep(1000);
                        }
                    }
                    else
                    {
                        if (processName == "cmd")
                            KillProcess(Convert.ToInt32(multicast.Pid));
                    }
                    Message.Text = "Successfully Deleted Task";
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.ToString());
                    Message.Text = "Could Not Kill Process.  Check The Exception Log For More Info";
                }
                return true;
            }
            else
            {
                Message.Text = "Could Not Delete Task";
                return false;
            }
        }

        public bool UpdateActiveMulticastSession(Models.ActiveMulticastSession activeMulticastSession)
        {
            _unitOfWork.ActiveMulticastSessionRepository.Update(activeMulticastSession, activeMulticastSession.Id);
            return _unitOfWork.Save();
        }

        public List<Models.ActiveMulticastSession> GetAllMulticastSessions()
        {
            return _unitOfWork.ActiveMulticastSessionRepository.Get(orderBy: (q => q.OrderBy(t => t.Name)));
        }

        public void KillProcess(int pid)
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
                Message.Text = "Could Not Kill Process.  Check The Exception Log For More Info";
            }
        }

        public void KillProcessLinux(int pid)
        {
            try
            {
                string dist = null;
                var distInfo = new ProcessStartInfo
                {
                    UseShellExecute = false,
                    FileName = "uname",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                using (var process = Process.Start(distInfo))
                {
                    if (process != null) dist = process.StandardOutput.ReadToEnd();
                }

                var shell = dist != null && dist.ToLower().Contains("bsd") ? "/bin/csh" : "/bin/bash";

                var killProcInfo = new ProcessStartInfo
                {
                    FileName = (shell),
                    Arguments = (" -c \"pkill -TERM -P " + pid + "\"")
                };
                Process.Start(killProcInfo);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString());
                Message.Text = "Could Not Kill Process.  Check The Exception Log For More Info";
            }
        }
    }
}