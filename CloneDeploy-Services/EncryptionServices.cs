using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using log4net;

namespace CloneDeploy_Services.Helpers
{
    /// <summary>
    ///     Summary http://www.codeproject.com/Articles/769741/Csharp-AES-bits-Encryption-Library-with-Salt
    /// </summary>
    public class EncryptionServices
    {
        private readonly ILog log = LogManager.GetLogger("ApplicationLog");

        private byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            byte[] saltBytes = {1, 2, 3, 4, 5, 6, 7, 8, 8, 7, 6, 5, 4, 3, 2, 1};

            using (var ms = new MemoryStream())
            {
                using (var AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize/8);
                    AES.IV = key.GetBytes(AES.BlockSize/8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }

        private byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            byte[] saltBytes = {1, 2, 3, 4, 5, 6, 7, 8, 8, 7, 6, 5, 4, 3, 2, 1};

            using (var ms = new MemoryStream())
            {
                using (var AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize/8);
                    AES.IV = key.GetBytes(AES.BlockSize/8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        public string DecryptText(string input)
        {
            var bytesToBeDecrypted = Convert.FromBase64String(input);
            var passwordBytes = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["DbEncryptionKey"]);
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            try
            {
                var bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, passwordBytes);
                var result = Encoding.UTF8.GetString(bytesDecrypted);

                return result;
            }
            catch (Exception)
            {
                log.Debug("Could Not Decrypt Password.  Ensure Your Encryption Key is Correct.");
                return null;
            }
        }

        public string EncryptText(string input)
        {
            var bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);
            var passwordBytes = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["DbEncryptionKey"]);

            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            var bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

            var result = Convert.ToBase64String(bytesEncrypted);

            return result;
        }
    }
}