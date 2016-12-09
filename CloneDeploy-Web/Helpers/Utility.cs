using System;
using System.Security.Cryptography;
using System.Text;

namespace CloneDeploy_Web.Helpers
{
    public class Utility
    {
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
            return s;
        }

        public static string EscapeFilePaths(string path)
        {
            return path != null ? path.Replace(@"\", @"\\") : String.Empty;
        }

        public static string GenerateKey()
        {
            return Guid.NewGuid().ToString();
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
    }
}