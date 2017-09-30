using System.Collections.Generic;
using System.Linq;
using CloneDeploy_ApiCalls;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Entities.DTOs.ClientImaging;

namespace CloneDeploy_Services
{
    public class ProxyDhcpServices
    {
        public TftpServerDTO GetAllTftpServers()
        {
            var tftpDto = new TftpServerDTO();
            tftpDto.TftpServers = new List<string>();
            if (SettingServices.ServerIsNotClustered)
                tftpDto.TftpServers.Add(
                    StringManipulationServices.PlaceHolderReplace(
                        SettingServices.GetSettingValue(SettingStrings.TftpServerIp)));
            else
            {
                if (SettingServices.TftpServerRole)
                    tftpDto.TftpServers.Add(
                        StringManipulationServices.PlaceHolderReplace(
                            SettingServices.GetSettingValue(SettingStrings.TftpServerIp)));
                var tftpServers = new SecondaryServerServices().SearchSecondaryServers().Where(x => x.TftpRole == 1 && x.IsActive == 1);
                foreach (var tftpServer in tftpServers)
                {
                    var tServer =
                        new APICall(new SecondaryServerServices().GetToken(tftpServer.Name)).ServiceAccountApi
                            .GetTftpServer();
                    if (!string.IsNullOrEmpty(tServer))
                    {
                        tftpDto.TftpServers.Add(tServer);
                    }
                }
            }

            return tftpDto;
        }

        public TftpServerDTO GetComputerTftpServers(string mac)
        {
            var tftpDto = new TftpServerDTO();
            tftpDto.TftpServers = new List<string>();
            if (SettingServices.ServerIsNotClustered)
                tftpDto.TftpServers.Add(
                    StringManipulationServices.PlaceHolderReplace(
                        SettingServices.GetSettingValue(SettingStrings.TftpServerIp)));

            else
            {
                var clusterServices = new ClusterGroupServices();
                var secondaryServerServices = new SecondaryServerServices();
                List<ClusterGroupServerEntity> clusterServers;
                var computer = new ComputerServices().GetComputerFromMac(mac);
                if (computer == null)
                {
                    var cg = new ClusterGroupServices().GetDefaultClusterGroup();
                    clusterServers = clusterServices.GetActiveClusterServers(cg.Id);
                }
                else
                {
                    var cg = new ComputerServices().GetClusterGroup(computer.Id);
                    clusterServers = clusterServices.GetActiveClusterServers(cg.Id);
                }

                foreach (var tftpServer in clusterServers.Where(x => x.TftpRole == 1))
                {
                    if (tftpServer.ServerId == -1)
                        tftpDto.TftpServers.Add(
                            StringManipulationServices.PlaceHolderReplace(
                                SettingServices.GetSettingValue(SettingStrings.TftpServerIp)));
                    else
                    {
                        var serverIdentifier =
                            secondaryServerServices.GetSecondaryServer(tftpServer.ServerId).Name;
                        var tServer =
                            new APICall(new SecondaryServerServices().GetToken(serverIdentifier)).ServiceAccountApi
                                .GetTftpServer();
                        if (tServer != null)
                            tftpDto.TftpServers.Add(tServer);
                    }
                }
            }

            return tftpDto;
        }

        public ProxyReservationDTO GetProxyReservation(string mac)
        {
            var bootClientReservation = new ProxyReservationDTO();

            var computer = new ComputerServices().GetComputerFromMac(mac);
            if (computer == null)
            {
                bootClientReservation.BootFile = "NotFound";
                return bootClientReservation;
            }
            if (computer.ProxyReservation == 0)
            {
                bootClientReservation.BootFile = "NotEnabled";
                return bootClientReservation;
            }

            var computerReservation = new ComputerServices().GetComputerProxyReservation(computer.Id);

            if (!string.IsNullOrEmpty(computerReservation.NextServer))
            {
                bootClientReservation.NextServer =
                    StringManipulationServices.PlaceHolderReplace(computerReservation.NextServer);
            }

            switch (computerReservation.BootFile)
            {
                case "bios_pxelinux":
                    bootClientReservation.BootFile = @"proxy/bios/pxelinux.0";
                    break;
                case "bios_ipxe":
                    bootClientReservation.BootFile = @"proxy/bios/undionly.kpxe";
                    break;
                case "bios_x86_winpe":
                    bootClientReservation.BootFile = @"proxy/bios/pxeboot.n12";
                    bootClientReservation.BcdFile = @"/boot/BCDx86";
                    break;
                case "bios_x64_winpe":
                    bootClientReservation.BootFile = @"proxy/bios/pxeboot.n12";
                    bootClientReservation.BcdFile = @"/boot/BCDx64";
                    break;
                case "efi_x86_syslinux":
                    bootClientReservation.BootFile = @"proxy/efi32/syslinux.efi";
                    break;
                case "efi_x86_ipxe":
                    bootClientReservation.BootFile = @"proxy/efi32/ipxe.efi";
                    break;
                case "efi_x86_winpe":
                    bootClientReservation.BootFile = @"proxy/efi32/bootmgfw.efi";
                    bootClientReservation.BcdFile = @"/boot/BCDx86";
                    break;
                case "efi_x64_syslinux":
                    bootClientReservation.BootFile = @"proxy/efi64/syslinux.efi";
                    break;
                case "efi_x64_ipxe":
                    bootClientReservation.BootFile = @"proxy/efi64/ipxe.efi";
                    break;
                case "efi_x64_winpe":
                    bootClientReservation.BootFile = @"proxy/efi64/bootmgfw.efi";
                    bootClientReservation.BcdFile = @"/boot/BCDx64";
                    break;
                case "efi_x64_grub":
                    bootClientReservation.BootFile = @"proxy/efi64/bootx64.efi";
                    break;
            }

            return bootClientReservation;
        }
    }
}