using System;
using Helpers;
using Models;

namespace BasePages
{
    public class Global : PageBaseMaster
    {
        public SysprepTag SysprepTag { get; set; }
        public BootTemplate BootTemplate { get; set; }
        public FileFolder FileFolder { get; set; }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            SysprepTag = !string.IsNullOrEmpty(Request["syspreptagid"]) ? BLL.SysprepTag.GetSysprepTag(Convert.ToInt32(Request.QueryString["syspreptagid"])) : null;
            BootTemplate = !string.IsNullOrEmpty(Request["templateid"]) ? BLL.BootTemplate.GetBootTemplate(Convert.ToInt32(Request.QueryString["templateid"])) : null;
            FileFolder = !string.IsNullOrEmpty(Request["fileid"]) ? BLL.FileFolder.GetFileFolder(Convert.ToInt32(Request.QueryString["fileid"])) : null;
            RequiresAuthorization(Authorizations.ReadGlobal);
        }
    }
}