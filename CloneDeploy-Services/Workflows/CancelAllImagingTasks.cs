using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using CloneDeploy_ApiCalls;
using CloneDeploy_Common;
using log4net;

namespace CloneDeploy_Services.Workflows
{
    public class CancelAllImagingTasks
    {
        private readonly ILog Log = LogManager.GetLogger("ApplicationLog");
        private readonly ComputerServices _computerServices;
        private readonly SecondaryServerServices _secondaryServerServices;

        public CancelAllImagingTasks()
        {
            _computerServices = new ComputerServices();
            _secondaryServerServices = new SecondaryServerServices();
        }

        public bool Execute()
        {
            //If a cluster primary - cancel all tasks on secondaries first, then move on
            if (SettingServices.ServerIsClusterPrimary)
            {
                foreach (var server in _secondaryServerServices.GetAllWithActiveRoles())
                {
                    new APICall(_secondaryServerServices.GetToken(server.Name))
                        .ServiceAccountApi.CancelAllImagingTasks();
                }
            }

            var tftpPath = SettingServices.GetSettingValue(SettingStrings.TftpPath);
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
                    Log.Debug(ex.ToString());
                    return false;
                }
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
                            Log.Debug(ex.ToString());
                        }
                    }
                    Thread.Sleep(200);
                }
            }


            if (!SettingServices.ServerIsClusterSecondary)
            {
                //The database tasks are only removed on a single server or cluster primary,
                //There are no database tasks in a clustered secondary.

                new ActiveImagingTaskServices().DeleteAll();
                new ActiveMulticastSessionServices().DeleteAll();

                //Recreate any custom boot menu's that were just deleted
                foreach (var computer in _computerServices.ComputersWithCustomBootMenu())
                {
                    //The create boot files method handles creating the file for the secondary servers
                    _computerServices.CreateBootFiles(computer.Id);
                }
            }
            return true;
        }
    }
}