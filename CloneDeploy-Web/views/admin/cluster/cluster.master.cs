using System;
using System.Web.UI;
using CloneDeploy_Common;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.admin.cluster
{
    public partial class cluster : MasterBaseMaster
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (GetSetting(SettingStrings.OperationMode) == "Cluster Secondary")
            {
                secondary.Visible = false;
                newsecondary.Visible = false;
                newcluster.Visible = false;
                clustergroup.Visible = false;
               
            }
        }
    }
}