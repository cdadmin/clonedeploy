using System;
using System.IO;
using Claunia.PropertyList;
using CloneDeploy_Entities;
using CloneDeploy_Web;

namespace BasePages
{
    public class Global : PageBaseMaster
    {
        public SysprepTagEntity SysprepTag { get; set; }
        public BootTemplateEntity BootTemplate { get; set; }
        public BootEntryEntity BootEntry { get; set; }
        public FileFolderEntity FileFolder { get; set; }
        public MunkiManifestTemplateEntity ManifestTemplate { get; set; }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            SysprepTag = !string.IsNullOrEmpty(Request["syspreptagid"]) ? Call.SysprepTagApi.Get(Convert.ToInt32(Request.QueryString["syspreptagid"])) : null;
            BootTemplate = !string.IsNullOrEmpty(Request["templateid"]) ? Call.BootTemplateApi.Get(Convert.ToInt32(Request.QueryString["templateid"])) : null;
            BootEntry = !string.IsNullOrEmpty(Request["entryid"]) ? Call.BootEntryApi.Get(Convert.ToInt32(Request.QueryString["entryid"])) : null;
            FileFolder = !string.IsNullOrEmpty(Request["fileid"]) ? Call.FileFolderApi.Get(Convert.ToInt32(Request.QueryString["fileid"])) : null;
            ManifestTemplate = !string.IsNullOrEmpty(Request["manifestid"]) ? Call.MunkiManifestTemplateApi.Get(Convert.ToInt32(Request.QueryString["manifestid"])) : null;

            RequiresAuthorization(Authorizations.ReadGlobal);
        }

        public MunkiPackageInfoEntity ReadPlist(string fileName)
        {
            try
            {
                NSDictionary rootDict = (NSDictionary)PropertyListParser.Parse(fileName);
                var plist = new MunkiPackageInfoEntity();
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
                    var smbPassword = new Encryption().DecryptText(Settings.MunkiSMBPassword);
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