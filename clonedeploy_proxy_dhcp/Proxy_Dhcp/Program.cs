/* 
  CWDS ProxyDHCP
  License: Microsoft Public License
  Copyright (C) 2015  Jon Dolny
  http://cruciblewds.org
  Based upon WinDHCP by Paul Wheeler
  https://windhcp.codeplex.com/
  Modified to operate as a Proxy DHCP server
*/

using System;
using System.Diagnostics;
using System.ServiceProcess;
using CloneDeploy_Proxy_Dhcp.Readers;
using CloneDeploy_Proxy_Dhcp.Server;
using CloneDeploy_Proxy_Dhcp.ServiceHost;
using Mono.Unix;
using Mono.Unix.Native;

namespace CloneDeploy_Proxy_Dhcp
{
    internal static class Program
    {


        private static void Main(string[] args)
        {
            if (args.Length > 0 && ContainsSwitch(args, "version"))
            {
                Console.WriteLine("1.1.0");
                Environment.Exit(0);
            }

            var reader = new IniReader();
            reader.CheckForConfig();

            //universal
            var networkInterface = reader.ReadConfig("interface");
            var nextServer = reader.ReadConfig("next-server");
            var listenDiscover = reader.ReadConfig("listen-discover");
            var listenProxy = reader.ReadConfig("listen-proxy");
            var allowAll = reader.ReadConfig("allow-all-mac");

            //pc options
            var biosBootFile = reader.ReadConfig("bios-bootfile");
            var efi32BootFile = reader.ReadConfig("efi32-bootfile");
            var efi64BootFile = reader.ReadConfig("efi64-bootfile");

            //apple options
            var appleBootFile = reader.ReadConfig("apple-boot-file");
            var rootPath = reader.ReadConfig("apple-root-path");
            var vendorInfo = reader.ReadConfig("apple-vendor-specific-information");
            var bsdpMode = reader.ReadConfig("apple-mode");
            var appleEfiBootFile = reader.ReadConfig("apple-efi-boot-file");
            ProxyServer proxy = new ProxyServer();
            if (listenProxy == "true")
            {

                proxy.UserNetworkInterface = networkInterface;
                proxy.UserNextServer = nextServer;
                proxy.BiosBootFile = biosBootFile;
                proxy.Efi32BootFile = efi32BootFile;
                proxy.Efi64BootFile = efi64BootFile;

                if (allowAll == "true")
                {
                    proxy.ClearAcls();
                    proxy.AllowAny = true;
                }
            }

            DiscoveryServer discovery = new DiscoveryServer();
            if (listenDiscover == "true")
            {

                discovery.UserNetworkInterface = networkInterface;
                discovery.UserNextServer = nextServer;
                discovery.AppleBootFile = appleBootFile;
                discovery.RootPath = rootPath;
                discovery.AppleEfiBootFile = appleEfiBootFile;
                discovery.BsdpMode = bsdpMode;
                discovery.VendorInfo = vendorInfo;

                if (allowAll == "true")
                {
                    discovery.ClearAcls();
                    discovery.AllowAny = true;
                }
            }



            var rdr = new FileReader();
            rdr.CheckForFile();

            if (allowAll != "true")
            {
                foreach (var mac in rdr.ReadFile("allow"))
                {
                    if (listenDiscover == "true")
                        discovery.AddAcl(PhysicalAddress.Parse(mac), false);
                    if (listenProxy == "true")
                        proxy.AddAcl(PhysicalAddress.Parse(mac), false);
                }
            }

            foreach (var mac in rdr.ReadFile("deny"))
            {
                if (listenDiscover == "true")
                    discovery.AddAcl(PhysicalAddress.Parse(mac), true);
                if (listenProxy == "true")
                    proxy.AddAcl(PhysicalAddress.Parse(mac), true);
            }

            foreach (var reservation in rdr.ReadFile("reservations"))
            {
                var arrayReservation = reservation.Split(',');

                if (listenDiscover == "true")
                {
                    DiscoveryServer.ReservationOptions options = new DiscoveryServer.ReservationOptions();
                    options.ReserveBootFile = arrayReservation[2];
                    options.ReserveNextServer = arrayReservation[1];
                    discovery.Reservations.Add(PhysicalAddress.Parse(arrayReservation[0]), options);
                }

                if (listenProxy == "true")
                {
                    ProxyServer.ReservationOptions options = new ProxyServer.ReservationOptions();
                    options.ReserveBootFile = arrayReservation[2];
                    options.ReserveNextServer = arrayReservation[1];
                    proxy.Reservations.Add(PhysicalAddress.Parse(arrayReservation[0]), options);
                }
            }

           

            if (args.Length > 0 &&
                (ContainsSwitch(args, "console") || ContainsSwitch(args, "debug") || ContainsSwitch(args, "daemon")))
            {
                DhcpHost hostDiscover = new DhcpHost(discovery, proxy);
                if (listenDiscover == "true")
                    hostDiscover.DiscoverListen = true;
                if (listenProxy == "true")
                    hostDiscover.ProxyListen = true;

                if (ContainsSwitch(args, "debug"))
                {
                    Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
                }

                //Only used for unix because the service version wasn't working correctly.
                if (ContainsSwitch(args, "daemon"))
                {
                    hostDiscover.ManualStart(args);

                    UnixSignal[] signals =
                    {
                        new UnixSignal(Signum.SIGINT),
                        new UnixSignal(Signum.SIGTERM)
                    };

                    for (var exit = false; !exit;)
                    {
                        var id = UnixSignal.WaitAny(signals);

                        if (id >= 0 && id < signals.Length)
                        {
                            if (signals[id].IsSet) exit = true;
                        }
                    }
                }
                else
                {
                    hostDiscover.ManualStart(args);
                    Console.WriteLine("DHCP Service Running.");
                    Console.WriteLine("Press [Enter] to Exit.");
                    Console.Read();
                    hostDiscover.ManualStop();
                }
            }
            else
            {
                DhcpHost hostDiscover = new DhcpHost(discovery, proxy);
                if (listenDiscover == "true")
                    hostDiscover.DiscoverListen = true;
                if (listenProxy == "true")
                    hostDiscover.ProxyListen = true;


                var servicesToRun = new ServiceBase[] {hostDiscover};

                ServiceBase.Run(servicesToRun);
            }
        }

        private static bool ContainsSwitch(string[] args, string switchStr)
        {
            foreach (var arg in args)
            {
                if (arg.StartsWith("--") && arg.Length > 2 &&
                    switchStr.StartsWith(arg.Substring(2), StringComparison.OrdinalIgnoreCase) ||
                    (arg.StartsWith("/") || arg.StartsWith("-")) && arg.Length > 1 &&
                    switchStr.StartsWith(arg.Substring(1), StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}