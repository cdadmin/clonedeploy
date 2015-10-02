using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class views_admin_dp_create : BasePages.Admin
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void buttonAddDp_OnClick(object sender, EventArgs e)
    {
        var distributionPoint = new Models.DistributionPoint
        {
            DisplayName = txtDisplayName.Text,
            Server = txtServer.Text,
            Protocol = ddlProtocol.Text,
            ShareName = txtShareName.Text,
            Domain = txtDomain.Text,
            Username = txtUsername.Text,
            Password = txtPassword.Text,
            IsPrimary = Convert.ToInt16(chkPrimary.Checked),
            PhysicalPath = chkPrimary.Checked ? txtPhysicalPath.Text : "",           
            IsBackend = Convert.ToInt16(chkBackend.Checked),
            BackendServer = chkBackend.Checked ? txtBackendServer.Text : ""
        };

        new BLL.DistributionPoint().AddDistributionPoint(distributionPoint);
    }

    protected void chkPrimary_OnCheckedChanged(object sender, EventArgs e)
    {
        PhysicalPath.Visible = chkPrimary.Checked;
    }

    protected void chkBackend_OnCheckedChanged(object sender, EventArgs e)
    {
        BackendServer.Visible = chkBackend.Checked;
    }
}