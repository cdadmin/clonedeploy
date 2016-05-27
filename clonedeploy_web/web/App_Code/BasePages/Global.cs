using System;
using System.IO;
using Claunia.PropertyList;
using Helpers;
using Models;

namespace BasePages
{
    public class Global : PageBaseMaster
    {
        public SysprepTag SysprepTag { get; set; }
        public BootTemplate BootTemplate { get; set; }
        public FileFolder FileFolder { get; set; }
        public MunkiManifestTemplate ManifestTemplate { get; set; }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            SysprepTag = !string.IsNullOrEmpty(Request["syspreptagid"]) ? BLL.SysprepTag.GetSysprepTag(Convert.ToInt32(Request.QueryString["syspreptagid"])) : null;
            BootTemplate = !string.IsNullOrEmpty(Request["templateid"]) ? BLL.BootTemplate.GetBootTemplate(Convert.ToInt32(Request.QueryString["templateid"])) : null;
            FileFolder = !string.IsNullOrEmpty(Request["fileid"]) ? BLL.FileFolder.GetFileFolder(Convert.ToInt32(Request.QueryString["fileid"])) : null;
            ManifestTemplate = !string.IsNullOrEmpty(Request["manifestid"]) ? BLL.MunkiManifestTemplate.GetManifest(Convert.ToInt32(Request.QueryString["manifestid"])) : null;
            RequiresAuthorization(Authorizations.ReadGlobal);
        }

        public Models.MunkiPackageInfo ReadPlist(string fileName)
        {
            try
            {
                NSDictionary rootDict = (NSDictionary)PropertyListParser.Parse(fileName);
                var plist = new Models.MunkiPackageInfo();
                plist.Name = rootDict.ObjectForKey("name").ToString();
                plist.Version = rootDict.ObjectForKey("version").ToString();
                return plist;
                /*NSObject[] parameters = ((NSArray) rootDict.ObjectForKey("installs")).GetArray();
                foreach (NSObject param in parameters)
                {   
                }*/
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return null;
            }
        }

        public FileInfo[] GetMunkiResources(string type)
        {
            var pkgInfoFiles = Settings.MunkiRootPath + type + Path.DirectorySeparatorChar;
            DirectoryInfo di = new DirectoryInfo(pkgInfoFiles);
            return di.GetFiles("*.*");
        }
    }
}