using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;
using BLL;
using DAL;
using Global;
using Pxe;

namespace Models
{
    [Table("active_imaging_tasks", Schema = "public")]
    public class ActiveImagingTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("active_task_id", Order = 1)]
        public int Id { get; set; }

        [Column("computer_id", Order = 2)]
        public int ComputerId { get; set; }

        [Column("task_status", Order = 3)]
        public string Status { get; set; }

        [Column("task_queue_position", Order = 4)]
        public int QueuePosition { get; set; }

        [Column("task_elapsed", Order = 5)]
        public string Elapsed { get; set; }

        [Column("task_remaining", Order = 6)]
        public string Remaining { get; set; }

        [Column("task_completed", Order = 7)]
        public string Completed { get; set; }

        [Column("task_rate", Order = 8)]
        public string Rate { get; set; }

        [Column("task_partition", Order = 9)]
        public string Partition { get; set; }

        [Column("task_arguments", Order = 10)]
        public string Arguments { get; set; }

        [Column("task_type", Order = 11)]
        public string Type { get; set; }

        [Column("multicast_id", Order = 12)]
        public int MulticastId { get; set; }

        [NotMapped]
        public string MulticastName { get; set; }
      

      

       

        public void Delete()
        {
            CloneDeployDbContext context = new CloneDeployDbContext();
            string hostMac;
            using (var db = new DB())
            {
                hostMac =  (from h in context.Computers
                            join t in db.ActiveTasks on h.Id equals t.ComputerId
                            where (t.Id == Id)
                            select h.Mac).FirstOrDefault();

                var task = db.ActiveTasks.Find(Id);
                db.ActiveTasks.Remove(task);
                db.SaveChanges();
            }

            var pxeHostMac = Utility.MacToPxeMac(hostMac);
            new PxeFileOps().CleanPxeBoot(pxeHostMac);

        }

        public void DeleteForMulticast()
        {
            using (var db = new DB())
            {
                var tasks = from t in db.ActiveTasks where t.MulticastName == MulticastName select t;
                db.ActiveTasks.RemoveRange(tasks);
                db.SaveChanges();
            }
        }

        public List<ActiveImagingTask> MulticastMemberStatus()
        {
            using (var db = new DB())
            {
                return
                    (from t in db.ActiveTasks where t.MulticastName == MulticastName orderby t.ComputerId select t).ToList();
            }
        }

        public List<ActiveImagingTask> MulticastProgress()
        {
            using (var db = new DB())
            {
                return
                    (from t in db.ActiveTasks where t.MulticastName == MulticastName && t.Status == "3" orderby t.ComputerId select t).Take(1).ToList();
            }
        }

        public static List<ActiveImagingTask> ReadAll()
        {
            using (var db = new DB())
            {
                return db.ActiveTasks.OrderBy(t => t.Id).ToList();
            }
        }

        public static List<ActiveImagingTask> ReadUnicasts()
        {
            using (var db = new DB())
            {
                return (from t in db.ActiveTasks where t.Type == "unicast" orderby t.ComputerId select t).ToList();
            }
        }

        public bool Create()
        {
            using (var db = new DB())
            {
                try
                {
                    if (db.ActiveTasks.Any(t => t.ComputerId == ComputerId))
                    {
                        Utility.Message += "This Host Is Already Part Of An Active Task <br>";
                        return false;
                    }
                    db.ActiveTasks.Add(this);
                    db.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    Logger.Log(ex.InnerException.InnerException.Message);
                    Utility.Message += "Could Not Create Task.  Check The Exception Log For More Info <br>";
                    return false;
                }
            }
            return true;
        }

        public bool Update(string updateType)
        {

            using (var db = new DB())
            {
                try
                {
                    var activeTask = db.ActiveTasks.Find(Id);
                    if (activeTask != null)
                    {
                        switch (updateType)
                        {
                            case "status":
                                activeTask.Status = Status;
                                break;
                            case "task":
                                activeTask.Status = Status;
                                activeTask.QueuePosition = QueuePosition;
                                break;
                            case "arguments":
                                activeTask.Arguments = Arguments;
                                break;
                            case "progress":
                                activeTask.Elapsed = Elapsed;
                                activeTask.Remaining = Remaining;
                                activeTask.Completed = Completed;
                                activeTask.Rate = Rate;
                                break;
                            case "partition":
                                activeTask.Elapsed = Elapsed;
                                activeTask.Remaining = Remaining;
                                activeTask.Completed = Completed;
                                activeTask.Rate = Rate;
                                activeTask.Partition = Partition;
                                break;
                        }


                        db.SaveChanges();
                    }
                }
                catch (DbUpdateException ex)
                {
                    Logger.Log(ex.InnerException.InnerException.Message);
                    Utility.Message = "Could Not Update Task.  Check The Exception Log For More Info.";
                    return false;
                }
            }

            return true;
        }

        public static void CancelAll()
        {
            var tftpPath = Settings.TftpPath;
            var pxePaths = new List<string>
            {
                tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar,
                tftpPath + "proxy" + Path.DirectorySeparatorChar + "bios" +
                Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar,
                tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi32" +
                Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar,
                tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar
            };


            var doNotRemove = new List<string>();

            foreach (var pxePath in pxePaths)
            {
                var pxeFiles = Directory.GetFiles(pxePath, "01*");
                try
                {
                    var fileOps = new FileOps();
                    foreach (var pxeFile in pxeFiles)
                    {
                        var ext = Path.GetExtension(pxeFile);

                        if (ext == ".custom") continue;
                        var fileName = Path.GetFileNameWithoutExtension(pxeFile);
                        var host = new BLL.Computer().GetComputerFromMac(Utility.PxeMacToMac(fileName));

                        var isCustomBootTemplate = Convert.ToBoolean(Convert.ToInt16(host.CustomBootEnabled));
                        if (isCustomBootTemplate)
                        {
                            if (File.Exists((pxePath + fileName + ".custom")))
                            {
                                fileOps.MoveFile(pxePath + fileName + ".custom", pxeFile);
                                doNotRemove.Add(pxeFile);
                            }
                            if (File.Exists((pxePath + fileName + ".ipxe.custom")))
                            {
                                fileOps.MoveFile(pxePath + fileName + ".ipxe.custom", pxeFile);
                                doNotRemove.Add(pxeFile);
                            }
                            if (!File.Exists((pxePath + fileName + ".cfg.custom"))) continue;
                            fileOps.MoveFile(pxePath + fileName + ".cfg.custom", pxeFile);
                            doNotRemove.Add(pxeFile);
                        }
                        else if (!doNotRemove.Contains(pxeFile))
                            File.Delete(pxeFile);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.ToString());
                    Utility.Message += "Could Not Delete PXE Files<br>";
                }
            }

            using (var db = new DB())
            {
                db.Database.ExecuteSqlCommand("TRUNCATE TABLE activetasks;");
                db.Database.ExecuteSqlCommand("TRUNCATE TABLE activemctasks;");
            }

            if (Environment.OSVersion.ToString().Contains("Unix"))
            {
                for (var x = 1; x < 10; x++)
                {
                    try
                    {
                        var killProcInfo = new ProcessStartInfo
                        {
                            FileName = ("killall"),
                            Arguments = (" udp-sender udp-receiver")
                        };
                        Process.Start(killProcInfo);
                    }
                    catch
                    {
                        // ignored
                    }

                    Thread.Sleep(200);
                }
            }

            else
            {
                for (var x = 1; x < 10; x++)
                {
                    foreach (var p in Process.GetProcessesByName("udp-sender"))
                    {
                        try
                        {
                            p.Kill();
                            p.WaitForExit();
                        }
                        catch (Exception ex)
                        {
                            Logger.Log(ex.ToString());
                        }
                    }
                    Thread.Sleep(200);
                }

                for (var x = 1; x < 10; x++)
                {
                    foreach (var p in Process.GetProcessesByName("udp-receiver"))
                    {
                        try
                        {
                            p.Kill();
                            p.WaitForExit();
                        }
                        catch (Exception ex)
                        {
                            Logger.Log(ex.ToString());
                        }
                    }
                    Thread.Sleep(200);
                }
            }
            Utility.Message = "Complete";
        }

        public static void WakeUp(string mac)
        {
            var pattern = new Regex("[:]");
            var wolHostMac = pattern.Replace(mac, "");

            try
            {
                var value = long.Parse(wolHostMac, NumberStyles.HexNumber, CultureInfo.CurrentCulture.NumberFormat);
                var macBytes = BitConverter.GetBytes(value);

                Array.Reverse(macBytes);
                var macAddress = new byte[6];

                for (var j = 0; j < 6; j++)
                    macAddress[j] = macBytes[j + 2];


                var packet = new byte[17 * 6];

                for (var i = 0; i < 6; i++)
                    packet[i] = 0xff;

                for (var i = 1; i <= 16; i++)
                {
                    for (var j = 0; j < 6; j++)
                        packet[i * 6 + j] = macAddress[j];
                }

                var client = new UdpClient();
                client.Connect(IPAddress.Broadcast, 9);
                client.Send(packet, packet.Length);
            }
            catch
            {
                // ignored
            }
        }
    }
}