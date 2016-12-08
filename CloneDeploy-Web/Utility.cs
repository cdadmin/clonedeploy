using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace CloneDeploy_Web
{
    public class Utility
    {

      

        public static string EscapeFilePaths(string path)
        {
            return path != null ? path.Replace(@"\", @"\\") : string.Empty;
        }

        public static string GenerateKey()
        {
            return Guid.NewGuid().ToString();
        }

        public static string CreatePasswordHash(string pwd, string salt)
        {
            var saltAndPwd = string.Concat(pwd, salt);
            HashAlgorithm hash = new SHA256Managed();
            byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(saltAndPwd);
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