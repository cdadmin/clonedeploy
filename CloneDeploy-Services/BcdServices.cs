using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using CloneDeploy_Services.Helpers;

namespace CloneDeploy_Services
{
    public class BcdServices
    {
        public string GetStandardLegacy(string diskSignature)
        {
            if (diskSignature.Length >= 8)
            {
                diskSignature = diskSignature.Substring(diskSignature.Length - 8);
            }

            var hexDiskSignature = diskSignature.Reverse().ToList();
            var reorderedDiskSigHex = HexReorder(hexDiskSignature);

            var path = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "private" +
                           Path.DirectorySeparatorChar + "bcd" + Path.DirectorySeparatorChar +
                           "legacy";
            var bcd = File.ReadAllText(path);

            var regex = new Regex("DISK_SIGNATURE", RegexOptions.IgnoreCase);
            bcd = regex.Replace(bcd, reorderedDiskSigHex);
            return StringManipulationServices.Encode(bcd);
        }

        public string GetStandardEfi(string diskGuid, string recoveryGuid, string windowsGuid)
        {
            var reorderedDisk = GuidReorder(diskGuid);
            var reorderedRecovery = GuidReorder(recoveryGuid);
            var reorderedWindows = GuidReorder(windowsGuid);

            var path = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "private" +
                          Path.DirectorySeparatorChar + "bcd" + Path.DirectorySeparatorChar +
                          "efi";
            var bcd = File.ReadAllText(path);

            var regex = new Regex("DISK_GUID", RegexOptions.IgnoreCase);
            bcd = regex.Replace(bcd, reorderedDisk);

            regex = new Regex("WINDOWS_GUID", RegexOptions.IgnoreCase);
            bcd = regex.Replace(bcd, reorderedWindows);

            regex = new Regex("RECOVERY_GUID", RegexOptions.IgnoreCase);
            bcd = regex.Replace(bcd, reorderedRecovery);


            return StringManipulationServices.Encode(bcd);

        }

        public string UpdateEfi(string bcd, string diskGuid, string recoveryGuid, string windowsGuid)
        {
            var reorderedDisk = GuidReorder(diskGuid);
            var reorderedRecovery = GuidReorder(recoveryGuid);
            var reorderedWindows = GuidReorder(windowsGuid);

            var regfile = new RegFileObject(bcd);
            foreach (var entry in regfile.RegValues)
            {
                if (entry.Key.EndsWith("11000001") || entry.Key.EndsWith("21000001") || entry.Key.EndsWith("31000003"))
                {
                    var regValue = entry.Value["Element"].Value;
                    var regValueSplit = regValue.Split(',');
                    if (regValue.Length <= 263)
                    {
                        var originalDisk = regValueSplit[56] + "," + regValueSplit[57] + "," + regValueSplit[58] +
                                                "," + regValueSplit[59] + "," +
                                                regValueSplit[60] + "," + regValueSplit[61] + "," + regValueSplit[62] +
                                                "," + regValueSplit[63] + "," + regValueSplit[64] + "," + regValueSplit[65] +
                                                "," + regValueSplit[66] + "," +
                                                regValueSplit[67] + "," + regValueSplit[68] + "," + regValueSplit[69] +
                                                "," + regValueSplit[70] + "," + regValueSplit[71];


                        var originalWindowsOrRecovery = regValueSplit[32] + "," + regValueSplit[33] + "," + regValueSplit[34] +
                                              "," + regValueSplit[35] + "," +
                                              regValueSplit[36] + "," + regValueSplit[37] + "," + regValueSplit[38] +
                                              "," + regValueSplit[39] + "," + regValueSplit[40] + "," + regValueSplit[41] +
                                              "," + regValueSplit[42] + "," +
                                              regValueSplit[43] + "," + regValueSplit[44] + "," + regValueSplit[45] +
                                              "," + regValueSplit[46] + "," + regValueSplit[47];

                        var regex = new Regex(originalDisk, RegexOptions.IgnoreCase);
                        bcd = regex.Replace(bcd, reorderedDisk);

                        regex = new Regex(originalWindowsOrRecovery, RegexOptions.IgnoreCase);
                        bcd = regex.Replace(bcd, entry.Key.EndsWith("31000003") ? reorderedRecovery : reorderedWindows);
                    }
                    else
                    {
                        var originalDisk = regValueSplit[108] + "," + regValueSplit[109] + "," + regValueSplit[110] +
                                               "," + regValueSplit[111] + "," +
                                               regValueSplit[112] + "," + regValueSplit[113] + "," + regValueSplit[114] +
                                               "," + regValueSplit[115] + "," + regValueSplit[116] + "," + regValueSplit[117] +
                                               "," + regValueSplit[118] + "," +
                                               regValueSplit[119] + "," + regValueSplit[120] + "," + regValueSplit[121] +
                                               "," + regValueSplit[122] + "," + regValueSplit[123];

                        var originalRecovery = regValueSplit[84] + "," + regValueSplit[85] + "," + regValueSplit[86] +
                                              "," + regValueSplit[87] + "," +
                                              regValueSplit[88] + "," + regValueSplit[89] + "," + regValueSplit[90] +
                                              "," + regValueSplit[91] + "," + regValueSplit[92] + "," + regValueSplit[93] +
                                              "," + regValueSplit[94] + "," +
                                              regValueSplit[95] + "," + regValueSplit[96] + "," + regValueSplit[97] +
                                              "," + regValueSplit[98] + "," + regValueSplit[99];

                        var regex = new Regex(originalDisk, RegexOptions.IgnoreCase);
                        bcd = regex.Replace(bcd, reorderedDisk);

                        regex = new Regex(originalRecovery, RegexOptions.IgnoreCase);
                        bcd = regex.Replace(bcd, reorderedRecovery);

                    }
                }
            }
            return StringManipulationServices.Encode(bcd);
        }

        public string UpdateLegacy(string bcd, string diskSignature, long windowsPartitionStart)
        {
            if (diskSignature.Length >= 8)
            {
                diskSignature = diskSignature.Substring(diskSignature.Length - 8);
            }
            var hexDiskSignature = diskSignature.Reverse().ToList();
            var hexPartitionStart = windowsPartitionStart.ToString("X16").Reverse().ToList();

            var reorderedDiskSigHex = HexReorder(hexDiskSignature);
            var reorderedPartStartHex = HexReorder(hexPartitionStart);

            var regfile = new RegFileObject(bcd);
            foreach (var entry in regfile.RegValues)
            {
                if (entry.Key.EndsWith("11000001") || entry.Key.EndsWith("21000001") || entry.Key.EndsWith("31000003"))
                {
                    var regValue = entry.Value["Element"].Value;
                    var regValueSplit = regValue.Split(',');
                    if (regValue.Length <= 263)
                    {
                        var originalPartStart = regValueSplit[32] + "," + regValueSplit[33] + "," + regValueSplit[34] +
                                                "," + regValueSplit[35] + "," +
                                                regValueSplit[36] + "," + regValueSplit[37] + "," + regValueSplit[38] +
                                                "," + regValueSplit[39];

                        var originalDiskSig = regValueSplit[56] + "," + regValueSplit[57] + "," + regValueSplit[58] +
                                              "," + regValueSplit[59];

                        var regex = new Regex(originalPartStart, RegexOptions.IgnoreCase);
                        bcd = regex.Replace(bcd, reorderedPartStartHex);

                        regex = new Regex(originalDiskSig, RegexOptions.IgnoreCase);
                        bcd = regex.Replace(bcd, reorderedDiskSigHex);
                    }
                    else
                    {
                        var originalDiskSig = regValueSplit[108] + "," + regValueSplit[109] + "," + regValueSplit[110] +
                                             "," + regValueSplit[111];
                        var regex = new Regex(originalDiskSig, RegexOptions.IgnoreCase);
                        bcd = regex.Replace(bcd, reorderedDiskSigHex);
                    }
                }
            }
            return StringManipulationServices.Encode(bcd);
        }

        public static string GuidReorder(string guid)
        {
            var uuidGuid = new Guid(guid);
            var uuidBytes = uuidGuid.ToByteArray();
            var strReverseUuid = "";
            foreach (var b in uuidBytes)
            {
                strReverseUuid += b.ToString("X2");
                strReverseUuid += ",";
            }
            return strReverseUuid.Trim(',');
        }

        public static string HexReorder(List<Char> hex)
        {
            var output = new StringBuilder();

            for (var i = 0; i < hex.Count; i++)
            {
                if (i % 2 != 0) continue;
                if (i + 1 < hex.Count)
                {
                    output.Append(hex[i + 1]);
                }
                output.Append(hex[i]);

                if (i + 2 != hex.Count)
                    output.Append(",");
            }
            return output.ToString();
        }
    }
}