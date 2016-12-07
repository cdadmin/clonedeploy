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
       
    }
}