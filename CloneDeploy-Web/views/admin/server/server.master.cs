using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CloneDeploy_Common;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.admin.server
{
    public partial class server : MasterBaseMaster
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (GetSetting(SettingStrings.OperationMode) == "Cluster Secondary")
            {
                alternateips.Visible = false;
                
            }
        }
    }
}