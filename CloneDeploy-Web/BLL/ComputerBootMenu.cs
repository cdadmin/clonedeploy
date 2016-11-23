using System;
using System.Collections.Generic;
using System.IO;
using Helpers;

namespace BLL
{
    public class ComputerBootMenu
    {
        //moved
        public static bool ToggleComputerBootMenu(CloneDeploy_Web.Models.Computer computer, bool enable)
        {
            computer.CustomBootEnabled = Convert.ToInt16(enable);
            BLL.Computer.UpdateComputer(computer);

            if(enable)
            CreateBootFiles(computer);

            return true;
        }

        //moved
        public static CloneDeploy_Web.Models.ComputerBootMenu GetComputerBootMenu(int computerId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ComputerBootMenuRepository.GetFirstOrDefault(p => p.ComputerId == computerId);
            }
        }

        //moved
        public static bool UpdateComputerBootMenu(CloneDeploy_Web.Models.ComputerBootMenu computerBootMenu)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                if (uow.ComputerBootMenuRepository.Exists(x => x.ComputerId == computerBootMenu.ComputerId))
                {
                    computerBootMenu.Id =
                        uow.ComputerBootMenuRepository.GetFirstOrDefault(
                            x => x.ComputerId == computerBootMenu.ComputerId).Id;
                    uow.ComputerBootMenuRepository.Update(computerBootMenu, computerBootMenu.Id);
                }
                else
                    uow.ComputerBootMenuRepository.Insert(computerBootMenu);

                return uow.Save();
            }
        }

        //moved
        public static bool DeleteComputerBootMenus(int computerId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.ComputerBootMenuRepository.DeleteRange(x => x.ComputerId == computerId);
                return uow.Save();
            }
        }

        //move not needed
        public static void DeleteBootFiles(CloneDeploy_Web.Models.Computer computer)
        {
            if (BLL.ActiveImagingTask.IsComputerActive(computer.Id)) return; //Files Will Be Processed When task is done
            var pxeMac = Utility.MacToPxeMac(computer.Mac);
            List<Tuple<string, string>> list = new List<Tuple<string, string>>
                {
                    Tuple.Create("bios", ""),
                    Tuple.Create("bios", ".ipxe"),
                    Tuple.Create("efi32", ""),
                    Tuple.Create("efi32", ".ipxe"),
                    Tuple.Create("efi64", ""),
                    Tuple.Create("efi64", ".ipxe"),
                    Tuple.Create("efi64", ".cfg")
                };
            foreach (var tuple in list)
            {
                new FileOps().DeletePath(Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar + tuple.Item1 +
                                         Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                                         pxeMac +
                                         tuple.Item2);
            }


            foreach(var ext in new[] {"",".ipxe",".cfg"})
            new FileOps().DeletePath(Settings.TftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                       pxeMac + ext);
        }

        //moved
        public static void CreateBootFiles(CloneDeploy_Web.Models.Computer computer)
        {
            if (BLL.ActiveImagingTask.IsComputerActive(computer.Id)) return; //Files Will Be Processed When task is done
            var bootMenu = GetComputerBootMenu(computer.Id);
            if (bootMenu == null) return;
            var pxeMac = Utility.MacToPxeMac(computer.Mac);
            string path;

            if (Settings.ProxyDhcp == "Yes")
            {
                List<Tuple<string, string, string>> list = new List<Tuple<string, string, string>>
                {
                    Tuple.Create("bios", "", bootMenu.BiosMenu),
                    Tuple.Create("bios", ".ipxe", bootMenu.BiosMenu),
                    Tuple.Create("efi32", "",bootMenu.Efi32Menu),
                    Tuple.Create("efi32", ".ipxe", bootMenu.Efi32Menu),
                    Tuple.Create("efi64", "" , bootMenu.Efi64Menu),
                    Tuple.Create("efi64", ".ipxe", bootMenu.Efi64Menu),
                    Tuple.Create("efi64", ".cfg", bootMenu.Efi64Menu)
                };
                foreach (var tuple in list)
                {
                    path = Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar + tuple.Item1 +
                           Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeMac +
                           tuple.Item2;

                        if(!string.IsNullOrEmpty(tuple.Item3))
                        new FileOps().WritePath(path, tuple.Item3);

                    
                }               
            }
            else
            {
                var mode = Settings.PxeMode;
                path = Settings.TftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                       pxeMac;

                if (mode.Contains("ipxe"))
                    path += ".ipxe";
                else if (mode.Contains("grub"))
                    path += ".cfg";
                    
                if (!string.IsNullOrEmpty(bootMenu.BiosMenu))
                    new FileOps().WritePath(path, bootMenu.BiosMenu);

            }
        }

        //moved
        public static string GetComputerNonProxyPath(CloneDeploy_Web.Models.Computer computer, bool isActiveOrCustom)
        {
            var mode = Settings.PxeMode;
            var pxeComputerMac = Utility.MacToPxeMac(computer.Mac);
            string path;

            var fileName = isActiveOrCustom ? pxeComputerMac : "default";

            if (mode.Contains("ipxe"))
                path = Settings.TftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                       fileName + ".ipxe";
            else if (mode.Contains("grub"))
            {
                
                if (isActiveOrCustom)
                {
                    path = Settings.TftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                      pxeComputerMac + ".cfg";
                }
                else
                {
                    path = Settings.TftpPath + "grub" + Path.DirectorySeparatorChar 
                      + "grub.cfg";
                }
               
            }
            else
                path = Settings.TftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                       fileName;

            return path;
        }

        //moved
        public static string GetComputerProxyPath(CloneDeploy_Web.Models.Computer computer, bool isActiveOrCustom, string proxyType)
        {
            var pxeComputerMac = Utility.MacToPxeMac(computer.Mac);
            string path = null;


            var biosFile = Settings.ProxyBiosFile;
            var efi32File = Settings.ProxyEfi32File;
            var efi64File = Settings.ProxyEfi64File;

            var fileName = isActiveOrCustom ? pxeComputerMac : "default";
            switch (proxyType)
            {
                case "bios":
                    if (biosFile.Contains("ipxe"))
                    {
                        path = Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar +
                               proxyType + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + fileName + ".ipxe";
                    }
                    else
                        path = Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar +
                               proxyType + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + fileName;
                    break;
                case "efi32":
                    if (efi32File.Contains("ipxe"))
                        path = Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar +
                               proxyType + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + fileName + ".ipxe";
                    else
                        path = Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar +
                               proxyType + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + fileName;
                    break;
                case "efi64":
                    if (efi64File.Contains("ipxe"))
                        path = Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar +
                               proxyType + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + fileName + ".ipxe";
                    else if (efi64File.Contains("grub"))
                    {
                        if (isActiveOrCustom)
                        {
                            path = Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar +
                              proxyType + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                              Path.DirectorySeparatorChar + pxeComputerMac + ".cfg";

                        }
                        else
                        {
                            path = Settings.TftpPath + "grub" +
                                  Path.DirectorySeparatorChar + "grub.cfg";
                        }
                    }
                    else
                        path = Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar +
                               proxyType + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + fileName;
                    break;
            }

            return path;
        }

    }
}