using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Threading;
using Global;
using Pxe;

namespace Models
{
    [Table("activemctasks", Schema = "public")]
    public class ActiveMcTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("mctaskid", Order = 1)]
        public string Id { get; set; }

        [Column("mctaskname", Order = 2)]
        public string Name { get; set; }

        [Column("mcpid", Order = 3)]
        public string Pid { get; set; }

        [Column("mcportbase", Order = 4)]
        public string Port { get; set; }

        [Column("mcimage", Order = 5)]
        public string Image { get; set; }

        public bool Create()
        {
            using (var db = new DB())
            {
                try
                {
                    if (db.ActiveMcTasks.Any(t => t.Name == Name))
                    {
                        Utility.Message += "A Multicast Is Already Running For This Group";
                        return false;
                    }
                    db.ActiveMcTasks.Add(this);
                    db.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    Logger.Log(ex.InnerException.InnerException.Message);
                    Utility.Message += "Could Not Create Multicast.  Check The Exception Log For More Info <br>";
                    return false;
                }
            }
            return true;
        }

        public void Delete()
        {
            using (var db = new DB())
            {
                var hostMacs = (from h in db.Hosts
                           join t in db.ActiveTasks on h.Name equals t.Name
                           where (t.MulticastName == Name)
                           select h.Mac).ToList();

                var mcTask = db.ActiveMcTasks.Find(Id);
                db.ActiveMcTasks.Remove(mcTask);
                db.SaveChanges();

                var activeTask = new ActiveTask {MulticastName = Name};
                activeTask.DeleteForMulticast();

                foreach (var mac in hostMacs)
                {
                    var pxeHostMac = Utility.MacToPxeMac(mac);
                    new PxeFileOps().CleanPxeBoot(pxeHostMac);
                }
            }

            try
            {
                var prs = Process.GetProcessById(Convert.ToInt32(Pid));
                var processName = prs.ProcessName;
                if (Environment.OSVersion.ToString().Contains("Unix"))
                {
                    while (!prs.HasExited)
                    {
                        KillProcessLinux(Convert.ToInt32(Pid));
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    if (processName == "cmd")
                        KillProcess(Convert.ToInt32(Pid));
                }
                Utility.Message = "Successfully Deleted Task";
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString());
                Utility.Message = "Could Not Kill Process.  Check The Exception Log For More Info";
            }

        }

        public void Update()
        {
            using (var db = new DB())
            {
                try
                {
                    var activeMcTask = db.ActiveMcTasks.Find(Id);
                    if (activeMcTask != null)
                    {
                        activeMcTask.Pid = Pid;
                        activeMcTask.Port = Port;
                        activeMcTask.Image = Image;
                        db.SaveChanges();
                    }
                }
                catch (DbUpdateException ex)
                {
                    Logger.Log(ex.InnerException.InnerException.Message);
                    Utility.Message = "Could Not Update Active Multicast Task.  Check The Exception Log For More Info.";
                }
            }
        }

        public static List<ActiveMcTask> ReadMulticasts()
        {
            using (var db = new DB())
            {
                return db.ActiveMcTasks.OrderBy(t => t.Name).ToList();
            }
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
                Utility.Message = "Could Not Kill Process.  Check The Exception Log For More Info";
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
                Utility.Message = "Could Not Kill Process.  Check The Exception Log For More Info";
            }
        }
    }
}