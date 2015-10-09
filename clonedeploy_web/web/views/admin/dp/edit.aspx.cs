using System;
using BasePages;
using BLL;

public partial class views_admin_dp_edit : Admin
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) PopulateForm();
    }

    protected void buttonUpdateDp_OnClick(object sender, EventArgs e)
    {
        var distributionPoint = BLL.DistributionPoint.GetDistributionPoint(Convert.ToInt32(Request.QueryString["dpid"]));

        distributionPoint.DisplayName = txtDisplayName.Text;
        distributionPoint.Server = txtServer.Text;
        distributionPoint.Protocol = ddlProtocol.Text;
        distributionPoint.ShareName = txtShareName.Text;
        distributionPoint.Domain = txtDomain.Text;
        distributionPoint.Username = txtUsername.Text;
        distributionPoint.Password = !string.IsNullOrEmpty(txtPassword.Text) ? txtPassword.Text : distributionPoint.Password;
        distributionPoint.IsPrimary = Convert.ToInt16(chkPrimary.Checked);
        distributionPoint.PhysicalPath = chkPrimary.Checked ? txtPhysicalPath.Text : "";
        distributionPoint.IsBackend = Convert.ToInt16(chkBackend.Checked);
        distributionPoint.BackendServer = chkBackend.Checked ? txtBackendServer.Text : "";

        BLL.DistributionPoint.UpdateDistributionPoint(distributionPoint);
    }

    protected void chkPrimary_OnCheckedChanged(object sender, EventArgs e)
    {
        PhysicalPath.Visible = chkPrimary.Checked;
    }

    protected void chkBackend_OnCheckedChanged(object sender, EventArgs e)
    {
        BackendServer.Visible = chkBackend.Checked;
    }

    protected void PopulateForm()
    {
        var distributionPoint = BLL.DistributionPoint.GetDistributionPoint(Convert.ToInt32(Request.QueryString["dpid"]));

        txtDisplayName.Text = distributionPoint.DisplayName;
        txtServer.Text = distributionPoint.Server;
        ddlProtocol.Text = distributionPoint.Protocol;
        txtShareName.Text = distributionPoint.ShareName;
        txtDomain.Text = distributionPoint.Domain;
        txtUsername.Text = distributionPoint.Username;
        txtPassword.Text = distributionPoint.Password;
        chkPrimary.Checked = Convert.ToBoolean(distributionPoint.IsPrimary);
        if (chkPrimary.Checked) PhysicalPath.Visible = true;
        txtPhysicalPath.Text = distributionPoint.PhysicalPath;
        chkBackend.Checked = Convert.ToBoolean(distributionPoint.IsBackend);
        if (chkBackend.Checked) BackendServer.Visible = true;
        txtBackendServer.Text = distributionPoint.BackendServer;


    }
}