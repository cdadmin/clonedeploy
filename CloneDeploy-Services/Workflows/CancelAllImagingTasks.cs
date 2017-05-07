using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using CloneDeploy_ApiCalls;
using CloneDeploy_Services.Helpers;
using log4net;

namespace CloneDeploy_Services.Workflows
{
    public class CancelAllImagingTasks
    {
        private static readonly ILog log = LogManager.GetLogger("ApplicationLog");

        public static bool Run()
        {
            if (Settings.OperationMode == "Cluster Primary")
            {
                var secondaryServers =
                    new SecondaryServerServices().SearchSecondaryServers()
                        .Where(x => x.MulticastRole == 1 || x.TftpRole == 1);
                foreach (var server in secondaryServers)
                {
                    new APICall(new SecondaryServerServices().GetApiToken(server.Name))
                        .WorkflowApi.CancelAllImagingTasks();
                }
            }

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

            foreach (var pxePath in pxePaths)
            {
                var pxeFiles = Directory.GetFiles(pxePath, "01*");
                try
                {
                    foreach (var pxeFile in pxeFiles)
                    {
                        File.Delete(pxeFile);
                    }
                }
                catch (Exception ex)
                {
                    log.Debug(ex.ToString());
                    return false;
                }
            }

            if (Settings.OperationMode != "Cluster Secondary")
            {
                new ActiveImagingTaskServices().DeleteAll();
                new ActiveMulticastSessionServices().DeleteAll();
            }

            if (Environment.OSVersion.ToString().Contains("Unix"))
            {
                for (var x = 1; x <= 10; x++)
                {
                    try
                    {
                        var killProcInfo = new ProcessStartInfo
                        {
                            FileName = "killall",
                            Arguments = " -s SIGKILL udp-sender"
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
                for (var x = 1; x <= 10; x++)
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
                            log.Debug(ex.ToString());
                        }
                    }
                    Thread.Sleep(200);
                }
            }

            if (Settings.OperationMode != "Cluster Secondary")
            {
                //Recreate any custom boot menu's that were just deleted
                foreach (var computer in new ComputerServices().ComputersWithCustomBootMenu())
                {
                    new ComputerServices().CreateBootFiles(computer.Id);
                }
            }
            return true;
        }
    }
}