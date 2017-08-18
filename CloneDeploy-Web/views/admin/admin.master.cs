using System;
using System.Web.UI;
using CloneDeploy_Common;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.admin
{
    public partial class AdminMaster : MasterBaseMaster
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (GetSetting(SettingStrings.OperationMode) == "Cluster Secondary")
            {
                client.Visible = false;
                bootmenu.Visible = false;
                clobber.Visible = false;
                editcore.Visible = false;
                email.Visible = false;
                export.Visible = false;
                kernel.Visible = false;
                kerneldownload.Visible = false;
                munki.Visible = false;
                multicast.Visible = false;
                pxe.Visible = false;
                security.Visible = false;
             
            }
        }
    }
}