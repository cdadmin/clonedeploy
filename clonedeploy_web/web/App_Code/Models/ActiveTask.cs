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
using Global;
using Pxe;

namespace Models
{
    [Table("activetasks", Schema = "public")]
    public class ActiveTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("taskid", Order = 1)]
        public string Id { get; set; }

        [Column("taskname", Order = 2)]
        public string Name { get; set; }

        [Column("taskstatus", Order = 3)]
        public string Status { get; set; }

        [Column("tasktype", Order = 4)]
        public string Type { get; set; }

        [Column("taskmulticastname", Order = 5)]
        public string MulticastName { get; set; }

        [Column("queueposition", Order = 6)]
        public int QueuePosition { get; set; }

        [Column("taskelapsed", Order = 7)]
        public string Elapsed { get; set; }

        [Column("taskremaining", Order = 8)]
        public string Remaining { get; set; }

        [Column("taskcompleted", Order = 9)]
        public string Completed { get; set; }

        [Column("taskrate", Order = 10)]
        public string Rate { get; set; }

        [Column("taskpartition", Order = 11)]
        public string Partition { get; set; }

        [Column("taskarguments", Order = 12)]
        public string Arguments { get; set; }

        public void Delete()
        {
            string hostMac;
            using (var db = new DB())
            {
                hostMac =  (from h in db.Hosts
                            join t in db.ActiveTasks on h.Name equals t.Name
                            where (t.Id == Id)
                            select h.Mac).FirstOrDefault();

                var task = db.ActiveTasks.Find(Id);
                db.ActiveTasks.Remove(task);
                db.SaveChanges();
            }

            var pxeHostMac = Utility.MacToPxeMac(hostMac);
            if (new PxeFileOps().CleanPxeBoot(pxeHostMac))
                Utility.Message = "Successfully Deleted Task ";
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

        public List<ActiveTask> MulticastMemberStatus()
        {
            using (var db = new DB())
            {
                return
                    (from t in db.ActiveTasks where t.MulticastName == MulticastName orderby t.Name select t).ToList();
            }
        }

        public List<ActiveTask> MulticastProgress()
        {
            using (var db = new DB())
            {
                return
                    (from t in db.ActiveTasks where t.MulticastName == MulticastName && t.Status == "3" orderby t.Name select t).Take(1).ToList();
            }
        }

        public static List<ActiveTask> ReadAll()
        {
            using (var db = new DB())
            {
                return db.ActiveTasks.OrderBy(t => t.Name).ToList();
            }
        }

        public static List<ActiveTask> ReadUnicasts()
        {
            using (var db = new DB())
            {
                return (from t in db.ActiveTasks where t.Type == "unicast" orderby t.Name select t).ToList();
            }
        }

        public bool Create()
        {
            using (var db = new DB())
            {
                try
                {
                    if (db.ActiveTasks.Any(t => t.Name == Name))
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

        private void GetId()
        {
            using (var db = new DB())
            {
                var activeTask = db.ActiveTasks.First(a => a.Name == Name);
                Id = activeTask.Id;
            }
        }

        public bool Update(string updateType)
        {
            if (string.IsNullOrEmpty(Id))
                GetId();
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
                        var host = new Computer { Mac = Utility.PxeMacToMac(fileName) };
                        host.Read();
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