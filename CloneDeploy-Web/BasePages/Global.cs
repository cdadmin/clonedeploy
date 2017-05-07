using System;
using CloneDeploy_Entities;
using CloneDeploy_Web.Helpers;

namespace CloneDeploy_Web.BasePages
{
    public class Global : PageBaseMaster
    {
        public BootEntryEntity BootEntry { get; set; }
        public BootTemplateEntity BootTemplate { get; set; }
        public FileFolderEntity FileFolder { get; set; }
        public MunkiManifestTemplateEntity ManifestTemplate { get; set; }
        public SysprepTagEntity SysprepTag { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            SysprepTag = !string.IsNullOrEmpty(Request["syspreptagid"])
                ? Call.SysprepTagApi.Get(Convert.ToInt32(Request.QueryString["syspreptagid"]))
                : null;
            BootTemplate = !string.IsNullOrEmpty(Request["templateid"])
                ? Call.BootTemplateApi.Get(Convert.ToInt32(Request.QueryString["templateid"]))
                : null;
            BootEntry = !string.IsNullOrEmpty(Request["entryid"])
                ? Call.BootEntryApi.Get(Convert.ToInt32(Request.QueryString["entryid"]))
                : null;
            FileFolder = !string.IsNullOrEmpty(Request["fileid"])
                ? Call.FileFolderApi.Get(Convert.ToInt32(Request.QueryString["fileid"]))
                : null;
            ManifestTemplate = !string.IsNullOrEmpty(Request["manifestid"])
                ? Call.MunkiManifestTemplateApi.Get(Convert.ToInt32(Request.QueryString["manifestid"]))
                : null;

            RequiresAuthorization(Authorizations.ReadGlobal);
        }
    }
}