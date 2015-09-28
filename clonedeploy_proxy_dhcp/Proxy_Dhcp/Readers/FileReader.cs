using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CloneDeploy_Proxy_Dhcp.Readers
{
    internal class FileReader
    {
        public void CheckForFile()
        {
            try
            {
                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"allow"))
                {
                    File.Create(AppDomain.CurrentDomain.BaseDirectory + @"allow").Dispose();
                    string lines = null;
                    lines += ";This file is ignored unless allow-all-mac is set to false in the config.ini\r\n";
                    lines +=
                        ";This file expects one mac per line in the format of 000000000000 or 00:00:00:00:00:00 or 00-00-00-00-00-00-00\r\n";
                    lines += ";00:11:22:33:44:55";
                    using (var file = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"allow"))
                    {
                        file.WriteLine(lines);
                        file.Close();
                    }
                }
                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"deny"))
                {
                    File.Create(AppDomain.CurrentDomain.BaseDirectory + @"deny").Dispose();
                    string lines = null;
                    lines += ";The denied list always overrides the allow list\r\n";
                    lines +=
                        ";This file expects one mac per line in the format of 000000000000 or 00:00:00:00:00:00 or 00-00-00-00-00-00-00\r\n";
                    lines += ";00:11:22:33:44:55";
                    using (var file = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"deny"))
                    {
                        file.WriteLine(lines);
                        file.Close();
                    }
                }
                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"reservations"))
                {
                    File.Create(AppDomain.CurrentDomain.BaseDirectory + @"reservations").Dispose();
                    string lines = null;
                    lines += ";This can be used send specific clients to specific tftp servers or boot files\r\n";
                    lines += ";This file expects one reservation per line in the format of mac,next-server,bootfile\r\n";
                    lines += ";00:11:22:33:44:55,192.168.56.1,proxy/bios/pxelinux.0";
                    using (var file = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"reservations"))
                    {
                        file.WriteLine(lines);
                        file.Close();
                    }
                }
            }
            catch
            {
                Trace.TraceError("Could Not Locate ACL Files");
                throw;
            }
        }

        public IEnumerable<string> ReadFile(string fileName)
        {
            using (var sr = new StreamReader((AppDomain.CurrentDomain.BaseDirectory + fileName)))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    var isComment = new string(line.Take(1).ToArray());
                    if (isComment != ";")
                        yield return line;
                }
            }
        }
    }
}
