using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Helpers;

namespace BLL.Workflows
{
    public class CancelAllImagingTasks
    {
        public static bool Run()
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
                        var host = new Computer().GetComputerFromMac(Utility.PxeMacToMac(fileName));

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
                    return false;
                }
            }

            new DAL.ActiveImagingTask().DeleteAll();

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

            return true;
        }
    }
}