using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CloneDeploy_ApiCalls;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Entities.DTOs.ClientImaging;
using CloneDeploy_Services.Helpers;
using log4net;


namespace CloneDeploy_Services
{
    public class ProxyDhcpServices
    {
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
                bootClientReservation.NextServer = Helpers.Utility.Between(computerReservation.NextServer);
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

        public TftpServerDTO GetAllTftpServers()
        {

            var tftpDto = new TftpServerDTO();
            tftpDto.TftpServers = new List<string>();
            if (Settings.OperationMode == "Single")
                tftpDto.TftpServers.Add(Utility.Between(Settings.TftpServerIp));
            else
            {
                if (Settings.TftpServerRole)
                    tftpDto.TftpServers.Add(Utility.Between(Settings.TftpServerIp));
                var tftpServers = new SecondaryServerServices().SearchSecondaryServers().Where(x => x.TftpRole == 1);
                foreach (var tftpServer in tftpServers)
                {
                    var tServer =
                        new APICall(new SecondaryServerServices().GetApiToken(tftpServer.Name)).ServiceAccountApi
                            .GetTftpServer();
                    if (tServer != null)
                        tftpDto.TftpServers.Add(tServer);
                }

            }

            return tftpDto;

        }

        public TftpServerDTO GetComputerTftpServers(string mac)
        {
            var tftpDto = new TftpServerDTO();
            tftpDto.TftpServers = new List<string>();
            if (Settings.OperationMode == "Single")
                tftpDto.TftpServers.Add(Utility.Between(Settings.TftpServerIp));

            else
            {
                var clusterServices = new ClusterGroupServices();
                var secondaryServerServices = new SecondaryServerServices();
                List<ClusterGroupServerEntity> clusterServers;
                var computer = new ComputerServices().GetComputerFromMac(mac);
                if (computer == null)
                {
                    var cg = new ClusterGroupServices().GetDefaultClusterGroup();
                    clusterServers = clusterServices.GetClusterServers(cg.Id);
                }
                else
                {
                    var cg = new ComputerServices().GetClusterGroup(computer.Id);
                    clusterServers = clusterServices.GetClusterServers(cg.Id);
                }

                foreach (var tftpServer in clusterServers.Where(x => x.TftpRole == 1))
                {
                    if (tftpServer.SecondaryServerId == -1)
                        tftpDto.TftpServers.Add(Utility.Between(Settings.TftpServerIp));
                    else
                    {
                        var serverIdentifier = secondaryServerServices.GetSecondaryServer(tftpServer.SecondaryServerId).Name;
                        var tServer = new APICall(new SecondaryServerServices().GetApiToken(serverIdentifier)).ServiceAccountApi
                                .GetTftpServer();
                        if (tServer != null)
                            tftpDto.TftpServers.Add(tServer);
                    }
                }

            }

            return tftpDto;


        }
    }

}
