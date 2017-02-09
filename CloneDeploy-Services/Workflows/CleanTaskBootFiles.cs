using System;
using System.IO;
using System.Linq;
using CloneDeploy_ApiCalls;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services.Helpers;
using log4net;

namespace CloneDeploy_Services.Workflows
{
    public class CleanTaskBootFiles
    {
        private readonly ILog log = LogManager.GetLogger("ApplicationLog");
        private readonly ComputerEntity _computer;
        private readonly string _bootFile;
        private const string ConfigFolder = "pxelinux.cfg";

        public CleanTaskBootFiles(ComputerEntity computer)
        {
            _computer = computer;
            _bootFile = Utility.MacToPxeMac(_computer.Mac);
        }

        private void DeleteProxyFile(string architecture, string extension = "")
        {
            if (Settings.OperationMode == "Single")
            {
                try
                {

                    File.Delete(Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar + architecture +
                                Path.DirectorySeparatorChar + ConfigFolder + Path.DirectorySeparatorChar + _bootFile +
                                extension);
                }
                catch (Exception ex)
                {
                    log.Debug(ex.Message);
                }
            }
            else
            {
                var clusterGroup = new ComputerServices().GetClusterGroup(_computer.Id);
                var tftpServers =
                    new ClusterGroupServices().GetClusterServers(clusterGroup.Id).Where(x => x.TftpRole == 1);
                foreach (var tftpServer in tftpServers)
                {

                    if (tftpServer.SecondaryServerId == -1)
                    {
                        try
                        {

                            File.Delete(Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar + architecture +
                                        Path.DirectorySeparatorChar + ConfigFolder + Path.DirectorySeparatorChar +
                                        _bootFile +
                                        extension);
                        }
                        catch (Exception ex)
                        {
                            log.Debug(ex.Message);
                        }
                    }
                    else
                    {
                        var secondaryServer =
                            new SecondaryServerServices().GetSecondaryServer(tftpServer.SecondaryServerId);
                        var tftpPath =
                            new APICall(new SecondaryServerServices().GetApiToken(secondaryServer.Name))
                                .SettingApi.GetSetting("Tftp Path").Value;

                        var path = tftpPath + "proxy" + Path.DirectorySeparatorChar + architecture +
                                   Path.DirectorySeparatorChar + ConfigFolder + Path.DirectorySeparatorChar +
                                   _bootFile + extension;

                        new APICall(new SecondaryServerServices().GetApiToken(secondaryServer.Name))
                            .FilesystemApi.DeleteTftpFile(path);
                    }
                }
            }
        }

        private void DeleteStandardFile(string extension = "")
        {
            if (Settings.OperationMode == "Single")
            {
                try
                {
                    File.Delete(Settings.TftpPath + ConfigFolder + Path.DirectorySeparatorChar +
                                _bootFile + extension);
                }
                catch (Exception ex)
                {
                    log.Debug(ex.Message);
                }
            }
            else
            {
                var clusterGroup = new ComputerServices().GetClusterGroup(_computer.Id);
                var tftpServers =
                    new ClusterGroupServices().GetClusterServers(clusterGroup.Id).Where(x => x.TftpRole == 1);
                foreach (var tftpServer in tftpServers)
                {

                    if (tftpServer.SecondaryServerId == -1)
                    {
                        try
                        {
                            File.Delete(Settings.TftpPath + ConfigFolder + Path.DirectorySeparatorChar +
                               _bootFile + extension);
                        }
                        catch (Exception ex)
                        {
                            log.Debug(ex.Message);
                        }
                    }
                    else
                    {
                        var secondaryServer =
                            new SecondaryServerServices().GetSecondaryServer(tftpServer.SecondaryServerId);
                        var tftpPath =
                            new APICall(new SecondaryServerServices().GetApiToken(secondaryServer.Name))
                                .SettingApi.GetSetting("Tftp Path").Value;

                        var path = tftpPath + ConfigFolder + Path.DirectorySeparatorChar +
                                   _bootFile + extension;
                        ;

                        new APICall(new SecondaryServerServices().GetApiToken(secondaryServer.Name))
                            .FilesystemApi.DeleteTftpFile(path);
                    }
                }
            }
        }

        public void CleanPxeBoot()
        {    
            if (Settings.ProxyDhcp == "Yes")
            {
                DeleteProxyFile("bios");
                DeleteProxyFile("bios", ".ipxe");
                DeleteProxyFile("efi32");
                DeleteProxyFile("efi32", ".ipxe");
                DeleteProxyFile("efi64");
                DeleteProxyFile("efi64", ".ipxe");
                DeleteProxyFile("efi64", ".cfg");
            }
            else
            {
                var mode = Settings.PxeMode;
                if (mode.Contains("ipxe"))
                    DeleteStandardFile(".ipxe");
                else if (mode.Contains("grub"))
                    DeleteStandardFile(".cfg");
                else        
                    DeleteStandardFile();
            }

            if (Settings.OperationMode != "Cluster Secondary")
            {
                if (Convert.ToBoolean(_computer.CustomBootEnabled))
                    new ComputerServices().CreateBootFiles(_computer.Id);
            }

        }
    }
}