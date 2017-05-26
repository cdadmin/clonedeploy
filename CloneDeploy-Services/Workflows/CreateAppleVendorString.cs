/*BSDP Options
*Code      Length      Values                              Name                        Client Or Server
-1-         1           1-List, 2-Select,3-Failed           Message Type                Both
-2-         2           uint16 encoding                     Version                     Client
-3-         4           ipaddr encoding                     Server Identifier           Server
-4-         2           uint16 0-65535                      Server Priority             Server
-5-         2           uint16 < 1024                       Reply Port                  Client
-7-         4           single:1-4095 cluster:4096-65535    Default Image Id            Server
-8-         4                                               Selected Image Id           Both
-9-         255 Max                                         Image List                  Server
*/

/*Example Vendor Option String Serving two netboot images. 1 named net and 1 named boot
01:01:01:03:04:A2:33:4B:A0:04:02:FF:FF:07:04:01:00:00:89:09:11:01:00:00:89:03:6E:65:74:01:00:00:88:04:62:6F:6F:74
    
01:01:                                        option 1
  01:                                     List
03:04:                                        option 3
  A2:33:4B:A0:                            Ip address
04:02:                                        option 4
  FF:FF:                                  Max Value
07:04:                                        Option 7
  01:00:00:89:                            image id always starts with 01:00 for Netboot OSX
09:11:                                        Option 9 Length = 5 * [number of images] + [sum of image names]
  01:00:00:89:                            [image id]
                  03:                     [image name length]
                        6E:65:74:         [image name]
  01:00:00:88:                            [image id]
                  04:                     [image name length]
                        62:6f:6f:74       [image name]
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using CloneDeploy_Common;
using CloneDeploy_Entities.DTOs;
using log4net;

namespace CloneDeploy_Services.Workflows
{
    public class CreateAppleVendorString
    {
        private readonly AppleVendorDTO _appleVendorDTO;

        private readonly NetBootProfileServices _netBootProfileServices;

        private readonly ILog log = LogManager.GetLogger("FrontEndLog");

        public CreateAppleVendorString()
        {
            _netBootProfileServices = new NetBootProfileServices();
            _appleVendorDTO = new AppleVendorDTO();
        }

        public AppleVendorDTO Execute(string ipAddress)
        {
            var netBootProfile = _netBootProfileServices.GetProfileFromIp(ipAddress);
            var nbiEntries = _netBootProfileServices.GetProfileNbiEntries(netBootProfile.Id);

            var directions = "";

            var publicFolder = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "public" +
                               Path.DirectorySeparatorChar + "macos_nbis";
            var tftpFolder = SettingServices.GetSettingValue(SettingStrings.TftpPath);
            var vendorOptions = new StringBuilder();
            vendorOptions.Append("01:01:01:03:04:");

            IPAddress ip;
            try
            {
                ip = IPAddress.Parse(ipAddress);
            }
            catch (Exception ex)
            {
                log.Debug("Could Not Parse IP Address. " + ipAddress + " " + ex.Message);
                _appleVendorDTO.Success = false;
                _appleVendorDTO.ErrorMessage = "Could Not Parse IP Address";
                return _appleVendorDTO;
            }

            foreach (var i in ip.GetAddressBytes())
            {
                vendorOptions.Append(i.ToString("X2") + ":");
            }

            vendorOptions.Append("04:02:FF:FF:07:04:");

            var rowCount = 0;
            var totalNameLength = 0;
            foreach (var nbi in nbiEntries)
            {
                rowCount++;
                if (rowCount == 1)
                {
                    var defaultId = nbi.NbiId;
                    vendorOptions.Append("01:00:");
                    vendorOptions.Append(AddHexColons(defaultId.ToString("X4")));
                    vendorOptions.Append(":");
                }
                var name = nbi.NbiName;
                totalNameLength += name.Length;
            }

            vendorOptions.Append("09:" + (5*rowCount + totalNameLength).ToString("X2"));
            vendorOptions.Append(":");

            var listIds = new List<string>();
            var counter = 1;
            foreach (var nbi in nbiEntries)
            {
                var imageId = nbi.NbiId;
                vendorOptions.Append("01:00:");
                var nbiIdHex = imageId.ToString("X4");
                vendorOptions.Append(AddHexColons(nbiIdHex));
                vendorOptions.Append(":");

                var name = nbi.NbiName;
                vendorOptions.Append(name.Length.ToString("X2"));
                vendorOptions.Append(":");
                vendorOptions.Append(StringToHex(name));
                if (counter != nbiEntries.Count)
                    vendorOptions.Append(":");
                listIds.Add(nbiIdHex);

                if (nbiIdHex != "0F49" && nbiIdHex != "98DB")
                {
                    directions += "Place the <b>" + name + " NetBoot.dmg</b> in <b>" + publicFolder +
                                  Path.DirectorySeparatorChar +
                                  nbiIdHex + "</b><br>";
                    directions += "Place the <b>" + name + " i386</b> folder in<b>" + tftpFolder + nbiIdHex +
                                  "</b><br><br>";
                }

                counter++;
            }

            var duplicateIds = listIds.GroupBy(x => x)
                .Where(group => group.Count() > 1)
                .Select(group => group.Key);

            if (duplicateIds.Any())
            {
                _appleVendorDTO.ErrorMessage = "The list cannot contain duplicate ids";
                _appleVendorDTO.Success = false;
                return _appleVendorDTO;
            }

            _appleVendorDTO.Success = true;
            _appleVendorDTO.VendorString = vendorOptions.ToString();
            _appleVendorDTO.Instructions = directions;
            return _appleVendorDTO;
        }

        private string AddHexColons(string hex)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < hex.Length; i++)
            {
                if (i % 2 == 0 && i != 0)
                    sb.Append(':');
                sb.Append(hex[i]);
            }
            return sb.ToString();
        }

        private string StringToHex(string hexstring)
        {
            var sb = new StringBuilder();
            foreach (var t in hexstring)
                sb.Append(Convert.ToInt32(t).ToString("X2"));
            return AddHexColons(sb.ToString());
        }
    }
}