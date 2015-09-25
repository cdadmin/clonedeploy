using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using Global;
using Helpers;
using Pxe;

namespace BLL
{


    public class ActiveImagingTask
    {
        private readonly DAL.ActiveImagingTask _da = new DAL.ActiveImagingTask();

        public bool DeleteActiveImagingTask(int activeImagingTaskId)
        {
            var activeImagingTask = _da.Read(activeImagingTaskId);
            var computer = new DAL.Computer().Read(activeImagingTask.ComputerId);

            if (_da.Delete(activeImagingTaskId))
            {
                if (new PxeFileOps().CleanPxeBoot(Utility.MacToPxeMac(computer.Mac)))
                {
                    Message.Text = "Successfully Deleted Task";
                    return true;
                }
                else
                {
                    Message.Text = "Could Not Delete Task";
                    return false;
                }
            }
            else
            {
                Message.Text = "Could Not Delete Task";
                return false;
            }
        }

        public bool AddActiveImagingTask(Models.ActiveImagingTask activeImagingTask)
        {
            if (_da.Exists(activeImagingTask.ComputerId))
            {
                Message.Text = "A Task Is Already Running For This Computer";
                return false;
            }
            if (_da.Create(activeImagingTask))
            {
                Message.Text = "Successfully Created Task";
                return true;
            }
            else
            {
                Message.Text = "Could Not Create Task";
                return false;
            }
        }

        public void DeleteForMulticast(int multicastId)
        {
            _da.DeleteForMulticast(multicastId);
        }

        public bool UpdateActiveImagingTask(Models.ActiveImagingTask activeImagingTask)
        {
            return _da.Update(activeImagingTask);
        }

        public List<Models.ActiveImagingTask> MulticastMemberStatus(int multicastId)
        {
            return _da.MulticastMemberStatus(multicastId);
        }

        public List<Models.ActiveImagingTask> MulticastProgress(int multicastId)
        {
            return _da.MulticastProgress(multicastId);
        }

        public List<Models.ActiveImagingTask> ReadAll()
        {
            return _da.ReadAll();
        }

        public List<Models.ActiveImagingTask> ReadUnicasts()
        {
            return _da.ReadUnicasts();
        }

        public List<Models.Computer> GetMulticastComputers(int multicastId)
        {
            return _da.MulticastComputers(multicastId);
        }
        public void CancelAll()
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
                    Message.Text += "Could Not Delete PXE Files<br>";
                }
            }

            _da.DeleteAll();

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
            Message.Text = "Complete";
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