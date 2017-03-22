using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Claunia.PropertyList;
using CloneDeploy_Entities;
using log4net;

namespace CloneDeploy_Services.Helpers
{
    public class Utility
    {
        private static readonly ILog log = LogManager.GetLogger("ApplicationLog");

        public static string Between(string parameter)
        {
            if (String.IsNullOrEmpty(parameter)) return parameter;
            int start = parameter.IndexOf("[", StringComparison.Ordinal);
            int to = parameter.IndexOf("]", start + "[".Length, StringComparison.Ordinal);
            if (start < 0 || to < 0) return parameter;
            string s = parameter.Substring(
                start + "[".Length,
                to - start - "[".Length);
            if (s == "server-ip")
            {
                return parameter.Replace("[server-ip]", Settings.ServerIp);
            }
            else if (s == "tftp-server-ip")
            {
                return parameter.Replace("[tftp-server-ip]", Settings.TftpServerIp);
            }
            return s;
        }


        public static string EscapeCharacter(string str, string[] charArray)
        {
            string escapedString = null;
            foreach (var c in charArray)
            {
                escapedString = str.Replace(c, "\\" + c);
                str = escapedString;
            }
            return escapedString;
        }

        public static string EscapeFilePaths(string path)
        {
            return path != null ? path.Replace(@"\", @"\\") : String.Empty;
        }

        public static string WindowsToUnixFilePath(string path)
        {
            return path != null ? path.Replace("\\", "/") : String.Empty;
        }

        public static string CreatePasswordHash(string pwd, string salt)
        {
            var saltAndPwd = String.Concat(pwd, salt);
            HashAlgorithm hash = new SHA256Managed();
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(saltAndPwd);
            byte[] hashBytes = hash.ComputeHash(plainTextBytes);
            return Convert.ToBase64String(hashBytes);
        }

        public static string CreateSalt(int byteSize)
        {
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[byteSize];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }

        public static string Decode(string encoded, string parameter)
        {
            string decoded = null;
            try
            {
                var dBytes = Convert.FromBase64String(encoded);
                decoded = Encoding.UTF8.GetString(dBytes);
            }
            catch (Exception ex)
            {
                log.Debug(parameter + " Base64 Decoding Failed. " + ex.Message);
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
                log.Debug("Base64 Encoding Failed. " + ex.Message);
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
            return Guid.NewGuid().ToString();
        }

        public static List<string> GetBootImages()
        {
            var bootImagePath = Settings.TftpPath + "images" + Path.DirectorySeparatorChar;

            var bootImageFiles = new List<string>();
            try
            {
                var files = Directory.GetFiles(bootImagePath, "*.*");

                for (var x = 0; x < files.Length; x++)
                    bootImageFiles.Add(Path.GetFileName(files[x]));
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return bootImageFiles;
        }

        public static List<string> GetKernels()
        {
            var kernelPath = Settings.TftpPath + "kernels" + Path.DirectorySeparatorChar;
            var result = new List<string>();
            try
            {
                var kernelFiles = Directory.GetFiles(kernelPath, "*.*");

                for (var x = 0; x < kernelFiles.Length; x++)
                    result.Add(Path.GetFileName(kernelFiles[x]));
            }

            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return result;
        }

        public static List<string> GetThinImages()
        {
            var thinImagePath = Settings.PrimaryStoragePath + "thin_images" + Path.DirectorySeparatorChar;
            var result = new List<string>();
            try
            {
                var dmgs = Directory.GetFiles(thinImagePath, "*.dmg*");

                for (var x = 0; x < dmgs.Length; x++)
                    result.Add(Path.GetFileName(dmgs[x]));
            }

            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return result;
        }

        public static List<string> GetLogs()
        {
            var logPath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "private" +
                          Path.DirectorySeparatorChar + "logs" + Path.DirectorySeparatorChar;

            var logFiles = Directory.GetFiles(logPath, "*.*");
            var result = new List<string>();
            for (var x = 0; x < logFiles.Length; x++)
                result.Add(Path.GetFileName(logFiles[x]));

            return result;
        }

        public static List<string> GetScripts(string type)
        {
            var scriptPath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "private" +
                             Path.DirectorySeparatorChar + "clientscripts" + Path.DirectorySeparatorChar;
            var result = new List<string>();
            try
            {
                var scriptFiles = Directory.GetFiles(scriptPath, "*.*");
                for (var x = 0; x < scriptFiles.Length; x++)
                    result.Add(Path.GetFileName(scriptFiles[x]));
            }

            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return result;
        }

        public MunkiPackageInfoEntity ReadPlist(string fileName)
        {
            try
            {
                NSDictionary rootDict = (NSDictionary)PropertyListParser.Parse(fileName);
                var plist = new MunkiPackageInfoEntity();
                plist.Name = rootDict.ObjectForKey("name").ToString();
                plist.Version = rootDict.ObjectForKey("version").ToString();
                return plist;

            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
                return null;
            }
        }

        public List<FileInfo> GetMunkiResources(string type)
        {
            FileInfo[] directoryFiles = null;
            string pkgInfoFiles = Settings.MunkiBasePath + Path.DirectorySeparatorChar + type + Path.DirectorySeparatorChar;
            if (Settings.MunkiPathType == "Local")
            {
                DirectoryInfo di = new DirectoryInfo(pkgInfoFiles);
                try
                {
                    directoryFiles = di.GetFiles("*.*");
                }
                catch (Exception ex)
                {
                    log.Debug(ex.Message);

                }
            }

            else
            {
                using (UNCAccessWithCredentials unc = new UNCAccessWithCredentials())
                {
                    var smbPassword = new Encryption().DecryptText(Settings.MunkiSMBPassword);
                    var smbDomain = string.IsNullOrEmpty(Settings.MunkiSMBDomain) ? "" : Settings.MunkiSMBDomain;
                    if (unc.NetUseWithCredentials(Settings.MunkiBasePath, Settings.MunkiSMBUsername, smbDomain, smbPassword) || unc.LastError == 1219)
                    {

                        DirectoryInfo di = new DirectoryInfo(pkgInfoFiles);
                        try
                        {
                            directoryFiles = di.GetFiles("*.*");
                        }
                        catch (Exception ex)
                        {
                            log.Debug(ex.Message);

                        }
                    }
                    else
                    {
                        log.Debug("Failed to connect to " + Settings.MunkiBasePath + "\r\nLastError = " + unc.LastError);
                    }
                }
            }

            return directoryFiles.ToList();

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
            var wolComputerMac = pattern.Replace(mac, "");

            try
            {
                var value = Int64.Parse(wolComputerMac, NumberStyles.HexNumber, CultureInfo.CurrentCulture.NumberFormat);
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