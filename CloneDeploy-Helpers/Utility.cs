using System;
using System.Security.Cryptography;
using System.Text;

namespace CloneDeploy_Common
{
    public class Utility
    {
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

        public static string GenerateKey()
        {
            return Guid.NewGuid().ToString();
        }
    }
}