using System;
using System.IO;
using CloneDeploy_ApiCalls;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using log4net;

namespace CloneDeploy_Services.Workflows
{
    public class CleanTaskBootFiles
    {
        private const string ConfigFolder = "pxelinux.cfg";
        private readonly string _bootFile;
        private readonly ClusterGroupServices _clusterGroupServices;
        private readonly ComputerEntity _computer;
        private readonly ComputerServices _computerServices;
        private readonly ILog _log = LogManager.GetLogger("ApplicationLog");
        private readonly SecondaryServerServices _secondaryServerServices;

        public CleanTaskBootFiles(ComputerEntity computer)
        {
            _computer = computer;
            _bootFile = StringManipulationServices.MacToPxeMac(_computer.Mac);
            _computerServices = new ComputerServices();
            _clusterGroupServices = new ClusterGroupServices();
            _secondaryServerServices = new SecondaryServerServices();
        }

        private void DeleteProxyFile(string architecture, string extension = "")
        {
            if (SettingServices.ServerIsNotClustered)
            {
                try
                {
                    File.Delete(SettingServices.GetSettingValue(SettingStrings.TftpPath) + "proxy" +
                                Path.DirectorySeparatorChar + architecture +
                                Path.DirectorySeparatorChar + ConfigFolder + Path.DirectorySeparatorChar + _bootFile +
                                extension);
                }
                catch (Exception ex)
                {
                    _log.Debug(ex.Message);
                }
            }
            else
            {
                var clusterGroup = _computerServices.GetClusterGroup(_computer.Id);
                foreach (var tftpServer in _clusterGroupServices.GetClusterTftpServers(clusterGroup.Id))
                {
                    if (tftpServer.ServerId == -1)
                    {
                        try
                        {
                            File.Delete(SettingServices.GetSettingValue(SettingStrings.TftpPath) + "proxy" +
                                        Path.DirectorySeparatorChar + architecture +
                                        Path.DirectorySeparatorChar + ConfigFolder + Path.DirectorySeparatorChar +
                                        _bootFile +
                                        extension);
                        }
                        catch (Exception ex)
                        {
                            _log.Debug(ex.Message);
                        }
                    }
                    else
                    {
                        var secondaryServer = _secondaryServerServices.GetSecondaryServer(tftpServer.ServerId);
                        var tftpPath =
                            new APICall(_secondaryServerServices.GetToken(secondaryServer.Name))
                                .SettingApi.GetSetting("Tftp Path").Value;

                        var path = tftpPath + "proxy" + Path.DirectorySeparatorChar + architecture +
                                   Path.DirectorySeparatorChar + ConfigFolder + Path.DirectorySeparatorChar +
                                   _bootFile + extension;

                        new APICall(_secondaryServerServices.GetToken(secondaryServer.Name))
                            .ServiceAccountApi.DeleteTftpFile(path);
                    }
                }
            }
        }

        private void DeleteStandardFile(string extension = "")
        {
            if (SettingServices.ServerIsNotClustered)
            {
                try
                {
                    File.Delete(SettingServices.GetSettingValue(SettingStrings.TftpPath) + ConfigFolder +
                                Path.DirectorySeparatorChar +
                                _bootFile + extension);
                }
                catch (Exception ex)
                {
                    _log.Debug(ex.Message);
                }
            }
            else
            {
                var clusterGroup = _computerServices.GetClusterGroup(_computer.Id);
                foreach (var tftpServer in _clusterGroupServices.GetClusterTftpServers(clusterGroup.Id))
                {
                    if (tftpServer.ServerId == -1)
                    {
                        try
                        {
                            File.Delete(SettingServices.GetSettingValue(SettingStrings.TftpPath) + ConfigFolder +
                                        Path.DirectorySeparatorChar +
                                        _bootFile + extension);
                        }
                        catch (Exception ex)
                        {
                            _log.Debug(ex.Message);
                        }
                    }
                    else
                    {
                        var secondaryServer = _secondaryServerServices.GetSecondaryServer(tftpServer.ServerId);
                        var tftpPath =
                            new APICall(_secondaryServerServices.GetToken(secondaryServer.Name))
                                .SettingApi.GetSetting("Tftp Path").Value;

                        var path = tftpPath + ConfigFolder + Path.DirectorySeparatorChar +
                                   _bootFile + extension;
                        ;

                        new APICall(_secondaryServerServices.GetToken(secondaryServer.Name))
                            .ServiceAccountApi.DeleteTftpFile(path);
                    }
                }
            }
        }

        public void Execute()
        {
            if (SettingServices.GetSettingValue(SettingStrings.ProxyDhcp) == "Yes")
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
                var mode = SettingServices.GetSettingValue(SettingStrings.PxeMode);
                if (mode.Contains("ipxe"))
                    DeleteStandardFile(".ipxe");
                else if (mode.Contains("grub"))
                    DeleteStandardFile(".cfg");
                else
                    DeleteStandardFile();
            }

            //Custom Boot files for the secondary cluster will be created by the primary
            //Don't run on secondary
            if (!SettingServices.ServerIsClusterSecondary)
            {
                if (Convert.ToBoolean(_computer.CustomBootEnabled))
                    _computerServices.CreateBootFiles(_computer.Id);
            }
        }
    }
}