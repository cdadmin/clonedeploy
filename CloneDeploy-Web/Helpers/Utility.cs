using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CloneDeploy_Web.Helpers
{
    public class Utility
    {
        public static string Between(string parameter)
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
                return parameter.Replace("[server-ip]", Settings.ServerIp);
            }
            return s;
        }

        public static string CreatePasswordHash(string pwd, string salt)
        {
            var saltAndPwd = string.Concat(pwd, salt);
            HashAlgorithm hash = new SHA256Managed();
            var plainTextBytes = Encoding.UTF8.GetBytes(saltAndPwd);
            var hashBytes = hash.ComputeHash(plainTextBytes);
            return Convert.ToBase64String(hashBytes);
        }

        public static string CreateSalt(int byteSize)
        {
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[byteSize];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }

        public static string EscapeFilePaths(string path)
        {
            return path != null ? path.Replace(@"\", @"\\") : string.Empty;
        }

        public static string GenerateKey()
        {
            return Guid.NewGuid().ToString();
        }

        public static List<string> GetFeLogs()
        {
            var logPath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "private" +
                          Path.DirectorySeparatorChar + "logs" + Path.DirectorySeparatorChar;

            var logFiles = Directory.GetFiles(logPath, "*.*");
            var result = new List<string>();
            for (var x = 0; x < logFiles.Length; x++)
                result.Add(Path.GetFileName(logFiles[x]));

            return result;
        }

        public static List<string> GetLogContents(string name, int limit)
        {
            var logPath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "private" +
                          Path.DirectorySeparatorChar + "logs" + Path.DirectorySeparatorChar + name;
            return File.ReadLines(logPath).Reverse().Take(limit).Reverse().ToList();
        }

        public static string SizeSuffix(long value)
        {
            string[] SizeSuffixes = {"bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"};

            if (value < 0)
            {
                return "-" + SizeSuffix(-value);
            }
            if (value == 0)
            {
                return "0.0 bytes";
            }

            var mag = (int) Math.Log(value, 1024);
            var adjustedSize = (decimal) value/(1L << (mag*10));

            return string.Format("{0:n1} {1}", adjustedSize, SizeSuffixes[mag]);
        }
    }
}