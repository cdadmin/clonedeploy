using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Threading;
using System.Web;
using Global;
using Helpers;
using Pxe;

namespace BLL
{
    public class ActiveMulticastSession
    {
        private readonly DAL.ActiveMulticastSession _da = new DAL.ActiveMulticastSession();

        public bool AddActiveMulticastSession(Models.ActiveMulticastSession activeMulticastSession)
        {
            if (_da.Exists(activeMulticastSession))
            {
                Message.Text = "A Multicast Is Already Running For This Group";
                return false;
            }
            if (_da.Create(activeMulticastSession))
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
            var multicast = _da.Read(multicastId);
            var computers = new BLL.ActiveImagingTask().GetMulticastComputers(multicastId);

            if (_da.Delete(multicastId))
            {
                new BLL.ActiveImagingTask().DeleteForMulticast(multicastId);

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
            return _da.Update(activeMulticastSession);
        }

        public List<Models.ActiveMulticastSession> GetAllMulticastSessions()
        {
            return _da.ReadMulticasts();
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