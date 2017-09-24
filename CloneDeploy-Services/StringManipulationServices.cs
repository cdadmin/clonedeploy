using System;
using System.Text;
using CloneDeploy_Common;
using log4net;

namespace CloneDeploy_Services
{
    public class StringManipulationServices
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(StringManipulationServices));

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
                log.Error(parameter + " Base64 Decoding Failed. " + ex.Message);
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
                log.Error("Base64 Encoding Failed. " + ex.Message);
            }

            return encoded;
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

        public static string MacToPxeMac(string mac)
        {
            var pxeMac = "01-" + mac.ToLower().Replace(':', '-');
            return pxeMac;
        }

        public static string PlaceHolderReplace(string parameter)
        {
            if (string.IsNullOrEmpty(parameter)) return parameter;
            var start = parameter.IndexOf("[", StringComparison.Ordinal);
            var to = parameter.IndexOf("]", start + "[".Length, StringComparison.Ordinal);
            if (start < 0 || to < 0) return parameter;
            var s = parameter.Substring(
                start + "[".Length,
                to - start - "[".Length);
            if (s == "server-ip")
            {
                return parameter.Replace("[server-ip]", SettingServices.GetSettingValue(SettingStrings.ServerIp));
            }
            if (s == "tftp-server-ip")
            {
                return parameter.Replace("[tftp-server-ip]",
                    SettingServices.GetSettingValue(SettingStrings.TftpServerIp));
            }
            return s;
        }

        public static string WindowsToUnixFilePath(string path)
        {
            return path != null ? path.Replace("\\", "/") : string.Empty;
        }
    }
}