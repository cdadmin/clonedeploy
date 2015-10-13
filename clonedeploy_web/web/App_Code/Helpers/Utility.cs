/*  
    CrucibleWDS A Windows Deployment Solution
    Copyright (C) 2011  Jon Dolny

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/.
 */

using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Helpers
{
    public class Utility
    {
        


        public static string EscapeFilePaths(string path)
        {
            return path != null ? path.Replace(@"\", @"\\") : string.Empty;
        }

        public static string Decode(string encoded)
        {
            string decoded = null;
            try
            {
                var dBytes = Convert.FromBase64String(encoded);
                decoded = Encoding.UTF8.GetString(dBytes);
            }
            catch (Exception ex)
            {
                Logger.Log("Decoding Failed. " + ex.Message);
            }

            return decoded;
        }

        public static string Encode(string decoded)
        {
            string encoded = null;
            try
            {
                var bytes = Encoding.UTF8.GetBytes(decoded);
                encoded = Convert.ToBase64String(bytes);
            }
            catch (Exception ex)
            {
                Logger.Log("Base64 Encoding Failed Failed. " + ex.Message);
            }

            return encoded;

            
            
        }

        public static string FixMac(string mac)
        {
            if (mac.Length == 12)
            {
                var sb = new StringBuilder();
                for (var i = 0; i < mac.Length; i++)
                {
                    if (i%2 == 0 && i != 0)
                        sb.Append(':');
                    sb.Append(mac[i]);
                }
                mac = sb.ToString();
            }
            else
                mac = mac.Replace('-', ':');

            return mac.ToUpper();
        }

        public static string GenerateKey()
        {
            var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string key;
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(Encoding.Default.GetBytes(timestamp));
                key = new Guid(hash).ToString();
            }

            return key.Substring(0, 18);
        }

        public static string[] GetBootImages()
        {
            var bootImagePath = Settings.TftpPath + "images" + Path.DirectorySeparatorChar;

            string[] bootImageFiles = null;
            try
            {
                bootImageFiles = Directory.GetFiles(bootImagePath, "*.*");

                for (var x = 0; x < bootImageFiles.Length; x++)
                    bootImageFiles[x] = Path.GetFileName(bootImageFiles[x]);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
            }
            return bootImageFiles;
        }

        public static string[] GetKernels()
        {
            var kernelPath = Settings.TftpPath + "kernels" + Path.DirectorySeparatorChar;
            string[] kernelFiles = null;
            try
            {
                kernelFiles = Directory.GetFiles(kernelPath, "*.*");

                for (var x = 0; x < kernelFiles.Length; x++)
                    kernelFiles[x] = Path.GetFileName(kernelFiles[x]);
            }

            catch (Exception ex)
            {
                Logger.Log(ex.Message);
            }
            return kernelFiles;
        }

        public static string[] GetLogs()
        {
            var logPath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "data" +
                          Path.DirectorySeparatorChar + "logs" + Path.DirectorySeparatorChar;

            var logFiles = Directory.GetFiles(logPath, "*.*");

            for (var x = 0; x < logFiles.Length; x++)
                logFiles[x] = Path.GetFileName(logFiles[x]);

            return logFiles;
        }

        public static string[] GetScripts(string type)
        {
            var scriptPath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "data" +
                             Path.DirectorySeparatorChar + "clientscripts" + Path.DirectorySeparatorChar + type +Path.DirectorySeparatorChar;
            string[] scriptFiles = null;
            try
            {
                scriptFiles = Directory.GetFiles(scriptPath, "*.*");
                for (var x = 0; x < scriptFiles.Length; x++)
                    scriptFiles[x] = Path.GetFileName(scriptFiles[x]);
            }

            catch (Exception ex)
            {
                Logger.Log(ex.Message);
            }
            return scriptFiles;
        }

        public static string MacToPxeMac(string mac)
        {
            var pxeMac = "01-" + mac.ToLower().Replace(':', '-');
            return pxeMac;
        }

        public static string PxeMacToMac(string pxeMac)
        {
            var mac = pxeMac.Remove(0, 3);
            mac = mac.ToUpper().Replace('-', ':');
            return mac;
        }

        public static void WakeUp(string mac)
        {
            var pattern = new Regex("[:]");
            var wolHostMac = pattern.Replace(mac, "");

            try
            {
                var value = long.Parse(wolHostMac, NumberStyles.HexNumber, CultureInfo.CurrentCulture.NumberFormat);
                var macBytes = BitConverter.GetBytes(value);

                Array.Reverse(macBytes);
                var macAddress = new byte[6];

                for (var j = 0; j < 6; j++)
                    macAddress[j] = macBytes[j + 2];


                var packet = new byte[17 * 6];

                for (var i = 0; i < 6; i++)
                    packet[i] = 0xff;

                for (var i = 1; i <= 16; i++)
                {
                    for (var j = 0; j < 6; j++)
                        packet[i * 6 + j] = macAddress[j];
                }

                var client = new UdpClient();
                client.Connect(IPAddress.Broadcast, 9);
                client.Send(packet, packet.Length);
            }
            catch
            {
                // ignored
            }
        }
    }
}