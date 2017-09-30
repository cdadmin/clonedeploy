using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using CloneDeploy_Common;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services.Helpers;
using log4net;

namespace CloneDeploy_Services.Workflows
{
    public class MulticastArguments
    {
        private readonly ILog log = LogManager.GetLogger(typeof(MulticastArguments));

        public int GenerateProcessArguments(MulticastArgsDTO mArgs)
        {
            var schemaCounter = -1;
            var multicastHdCounter = 0;
            string processArguments = null;
            foreach (var hd in mArgs.schema.HardDrives)
            {
                schemaCounter++;
                if (!hd.Active) continue;
                multicastHdCounter++;

                var x = 0;
                foreach (var part in mArgs.schema.HardDrives[schemaCounter].Partitions)
                {
                    if (!part.Active) continue;
                    string imageFile = null;
                    foreach (var ext in new[] {"ntfs", "fat", "extfs", "hfsp", "imager", "winpe", "xfs"})
                    {
                        imageFile = new FilesystemServices().GetFileNameWithFullPath(mArgs.ImageName,
                            schemaCounter.ToString(), part.Number, ext);

                        if (!string.IsNullOrEmpty(imageFile)) break;

                        //Look for lvm
                        if (part.VolumeGroup == null) continue;
                        if (part.VolumeGroup.LogicalVolumes == null) continue;
                        foreach (var lv in part.VolumeGroup.LogicalVolumes.Where(lv => lv.Active))
                        {
                            imageFile = new FilesystemServices().GetLVMFileNameWithFullPath(mArgs.ImageName,
                                schemaCounter.ToString(), lv.VolumeGroup, lv.Name, ext);
                        }
                    }

                    if (string.IsNullOrEmpty(imageFile))
                        continue;
                    if (mArgs.Environment == "winpe" &&
                        mArgs.schema.HardDrives[schemaCounter].Table.ToLower() == "gpt")
                    {
                        if (part.Type.ToLower() == "system" || part.Type.ToLower() == "recovery" ||
                            part.Type.ToLower() == "reserved")
                            continue;
                    }
                    if (mArgs.Environment == "winpe" &&
                        mArgs.schema.HardDrives[schemaCounter].Table.ToLower() == "mbr")
                    {
                        if (part.Number == mArgs.schema.HardDrives[schemaCounter].Boot &&
                            mArgs.schema.HardDrives[schemaCounter].Partitions.Length > 1)
                            continue;
                    }
                    x++;

                    var minReceivers = "";

                    if (!string.IsNullOrEmpty(mArgs.clientCount))
                        minReceivers = " --min-receivers " + mArgs.clientCount;

                    var isUnix = Environment.OSVersion.ToString().Contains("Unix");

                    string compAlg;
                    var stdout = "";
                    switch (Path.GetExtension(imageFile))
                    {
                        case ".lz4":
                            compAlg = isUnix ? "lz4 -d " : "lz4.exe\" -d ";
                            stdout = " - ";
                            break;
                        case ".gz":
                            compAlg = isUnix ? "gzip -c -d " : "gzip.exe\" -c -d ";
                            stdout = "";
                            break;
                        case ".uncp":
                            compAlg = "none";
                            break;
                        case ".wim":
                            compAlg = "none";
                            break;
                        default:
                            return 0;
                    }

                    if (isUnix)
                    {
                        string prefix = null;
                        if (multicastHdCounter == 1)
                            prefix = x == 1 ? " -c \"" : " ; ";
                        else
                            prefix = " ; ";

                        if (compAlg == "none" ||
                            SettingServices.GetSettingValue(SettingStrings.MulticastDecompression) == "client")
                        {
                            processArguments += prefix + "cat " + "\"" + imageFile + "\"" + " | udp-sender" +
                                                " --portbase " + mArgs.Port + minReceivers + " " +
                                                " --ttl 32 " +
                                                mArgs.ExtraArgs;
                        }

                        else
                        {
                            processArguments += prefix + compAlg + "\"" + imageFile + "\"" + stdout + " | udp-sender" +
                                                " --portbase " + mArgs.Port + minReceivers + " " +
                                                " --ttl 32 " +
                                                mArgs.ExtraArgs;
                        }
                    }
                    else
                    {
                        var appPath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "private" +
                                      Path.DirectorySeparatorChar + "apps" + Path.DirectorySeparatorChar;

                        string prefix = null;
                        if (multicastHdCounter == 1)
                        {
                            //Relative to the multicast server being called
                            var primaryDp = new DistributionPointServices().GetPrimaryDistributionPoint();
                            if (primaryDp.Location == "Local")
                            {
                                prefix = x == 1 ? " /c \"" : " & ";
                            }
                            else //Remote
                            {
                                if (x == 1)
                                {
                                    prefix = " /c \"net use \\\\" + primaryDp.Server + "\\" + primaryDp.ShareName +
                                             " /user:" + primaryDp.RoUsername + " " +
                                             new EncryptionServices().DecryptText(primaryDp.RoPassword) + " & ";
                                }
                                else
                                {
                                    prefix = " & ";
                                }
                            }
                        }
                        else
                            prefix = " & ";

                        if (compAlg == "none" ||
                            SettingServices.GetSettingValue(SettingStrings.MulticastDecompression) == "client")
                        {
                            processArguments += prefix + "\"" + appPath +
                                                "udp-sender.exe" + "\"" + " --file " + "\"" + imageFile + "\"" +
                                                " --portbase " + mArgs.Port + minReceivers + " " +
                                                " --ttl 32 " +
                                                mArgs.ExtraArgs;
                        }
                        else
                        {
                            processArguments += prefix + "\"" + appPath + compAlg + "\"" + imageFile + "\"" + stdout +
                                                " | " + "\"" + appPath +
                                                "udp-sender.exe" + "\"" +
                                                " --portbase " + mArgs.Port + minReceivers + " " +
                                                " --ttl 32 " +
                                                mArgs.ExtraArgs;
                        }
                    }
                }
            }

            var pDp = new DistributionPointServices().GetPrimaryDistributionPoint();
            if (pDp.Location == "Remote")
                processArguments += " & net use /delete \\\\" + pDp.Server + "\\" + pDp.ShareName;
            processArguments += "\"";

            return StartMulticastSender(processArguments, mArgs.groupName);
        }

        private int StartMulticastSender(string processArguments, string groupName)
        {
            var isUnix = Environment.OSVersion.ToString().Contains("Unix");

            var shell = isUnix ? "/bin/bash" : "cmd.exe";

            var senderInfo = new ProcessStartInfo {FileName = shell, Arguments = processArguments};

            //Fix
            var logPath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "private" +
                          Path.DirectorySeparatorChar + "logs" + Path.DirectorySeparatorChar + "multicast.log";

            var logText = Environment.NewLine + DateTime.Now.ToString("MM-dd-yy hh:mm") +
                          " Starting Multicast Session " +
                          groupName +
                          " With The Following Command:" + Environment.NewLine + senderInfo.FileName +
                          senderInfo.Arguments
                          + Environment.NewLine;
            File.AppendAllText(logPath, logText);

            Process sender;
            try
            {
                sender = Process.Start(senderInfo);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                File.AppendAllText(logPath,
                    "Could Not Start Session " + groupName + " Try Pasting The Command Into A Command Prompt");
                return 0;
            }

            Thread.Sleep(2000);

            if (sender == null) return 0;

            if (sender.HasExited)
            {
                File.AppendAllText(logPath,
                    "Session " + groupName + " Started And Was Forced To Quit, Try Running The Command Manually");
                return 0;
            }

            return sender.Id;
        }
    }
}