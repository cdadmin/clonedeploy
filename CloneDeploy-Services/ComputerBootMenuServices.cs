using System;
using System.Collections.Generic;
using System.IO;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services.Helpers;

namespace CloneDeploy_Services
{
    public class ComputerBootMenuServices
    {
        private readonly UnitOfWork _uow;

        public ComputerBootMenuServices()
        {
            _uow = new UnitOfWork();
        }


        public void DeleteBootFiles(ComputerEntity computer)
        {
            if (new ComputerServices().IsComputerActive(computer.Id))
                return; //Files Will Be Processed When task is done
            var pxeMac = Utility.MacToPxeMac(computer.Mac);
            var list = new List<Tuple<string, string>>
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


            foreach (var ext in new[] {"", ".ipxe", ".cfg"})
                new FileOps().DeletePath(Settings.TftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                                         pxeMac + ext);
        }

        public ActionResultDTO UpdateComputerBootMenu(ComputerBootMenuEntity computerBootMenu)
        {
            if (_uow.ComputerBootMenuRepository.Exists(x => x.ComputerId == computerBootMenu.ComputerId))
            {
                computerBootMenu.Id =
                    _uow.ComputerBootMenuRepository.GetFirstOrDefault(
                        x => x.ComputerId == computerBootMenu.ComputerId).Id;
                _uow.ComputerBootMenuRepository.Update(computerBootMenu, computerBootMenu.Id);
            }
            else
                _uow.ComputerBootMenuRepository.Insert(computerBootMenu);

            _uow.Save();
            var result = new ActionResultDTO();
            result.Success = true;
            result.Id = computerBootMenu.Id;
            return result;
        }
    }
}