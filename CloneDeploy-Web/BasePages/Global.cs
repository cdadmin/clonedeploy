using System;
using System.IO;
using Claunia.PropertyList;
using CloneDeploy_Web.Models;
using ConnectUNCWithCredentials;
using Helpers;

namespace BasePages
{
    public class Global : PageBaseMaster
    {
        public SysprepTag SysprepTag { get; set; }
        public BootTemplate BootTemplate { get; set; }
        public BootEntry BootEntry { get; set; }
        public FileFolder FileFolder { get; set; }
        public MunkiManifestTemplate ManifestTemplate { get; set; }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            SysprepTag = !string.IsNullOrEmpty(Request["syspreptagid"]) ? BLL.SysprepTag.GetSysprepTag(Convert.ToInt32(Request.QueryString["syspreptagid"])) : null;
            BootTemplate = !string.IsNullOrEmpty(Request["templateid"]) ? BLL.BootTemplate.GetBootTemplate(Convert.ToInt32(Request.QueryString["templateid"])) : null;
            BootEntry = !string.IsNullOrEmpty(Request["entryid"]) ? BLL.BootEntry.GetBootEntry(Convert.ToInt32(Request.QueryString["entryid"])) : null;
            FileFolder = !string.IsNullOrEmpty(Request["fileid"]) ? BLL.FileFolder.GetFileFolder(Convert.ToInt32(Request.QueryString["fileid"])) : null;
            ManifestTemplate = !string.IsNullOrEmpty(Request["manifestid"]) ? BLL.MunkiManifestTemplate.GetManifest(Convert.ToInt32(Request.QueryString["manifestid"])) : null;

            RequiresAuthorization(Authorizations.ReadGlobal);
        }

        public MunkiPackageInfo ReadPlist(string fileName)
        {
            try
            {
                NSDictionary rootDict = (NSDictionary)PropertyListParser.Parse(fileName);
                var plist = new MunkiPackageInfo();
                plist.Name = rootDict.ObjectForKey("name").ToString();
                plist.Version = rootDict.ObjectForKey("version").ToString();
                return plist;
              
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return null;
            }
        }

        public FileInfo[] GetMunkiResources(string type)
        {
            FileInfo[] directoryFiles = null;
            string pkgInfoFiles = Settings.MunkiBasePath + Path.DirectorySeparatorChar + type + Path.DirectorySeparatorChar;
            if (Settings.MunkiPathType == "Local")
            {
                DirectoryInfo di = new DirectoryInfo(pkgInfoFiles);
                try
                {
                    directoryFiles = di.GetFiles("*.*");
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message);
                  
                }
            }
           
            else
            {
                using (UNCAccessWithCredentials unc = new UNCAccessWithCredentials())
                {
                    var smbPassword = new Helpers.Encryption().DecryptText(Settings.MunkiSMBPassword);
                    var smbDomain = string.IsNullOrEmpty(Settings.MunkiSMBDomain) ? "" : Settings.MunkiSMBDomain;
                    if (unc.NetUseWithCredentials(Settings.MunkiBasePath, Settings.MunkiSMBUsername, smbDomain, smbPassword) || unc.LastError == 1219)
                    {
                        
                        DirectoryInfo di = new DirectoryInfo(pkgInfoFiles);
                        try
                        {
                            directoryFiles = di.GetFiles("*.*");
                        }
                        catch (Exception ex)
                        {
                            Logger.Log(ex.Message);
                           
                        }
                    }
                    else
                    {
                        Logger.Log("Failed to connect to " + Settings.MunkiBasePath + "\r\nLastError = " + unc.LastError);
                    }
                }
            }

            return directoryFiles;

        }
    }
}