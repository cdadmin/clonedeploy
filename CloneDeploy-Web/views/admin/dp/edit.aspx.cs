using System;
using BasePages;

public partial class views_admin_dp_edit : Admin
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) PopulateForm();
    }

    protected void buttonUpdateDp_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.UpdateAdmin);
        var distributionPoint = BLL.DistributionPoint.GetDistributionPoint(Convert.ToInt32(Request.QueryString["dpid"]));

        distributionPoint.DisplayName = txtDisplayName.Text;
        distributionPoint.Server = txtServer.Text;
        distributionPoint.Protocol = ddlProtocol.Text;
        distributionPoint.ShareName = txtShareName.Text;
        distributionPoint.Domain = txtDomain.Text;
        distributionPoint.RwUsername = txtRwUsername.Text;
        distributionPoint.RwPassword = !string.IsNullOrEmpty(txtRwPassword.Text)
            ? new Helpers.Encryption().EncryptText(txtRwPassword.Text)
            : distributionPoint.RwPassword;
        distributionPoint.RoUsername = txtRoUsername.Text;
        distributionPoint.RoPassword = !string.IsNullOrEmpty(txtRoPassword.Text)
            ? new Helpers.Encryption().EncryptText(txtRoPassword.Text)
            : distributionPoint.RoPassword;
        distributionPoint.IsPrimary = Convert.ToInt16(chkPrimary.Checked);
        distributionPoint.PhysicalPath = chkPrimary.Checked ? txtPhysicalPath.Text : "";
       

        var result = BLL.DistributionPoint.UpdateDistributionPoint(distributionPoint);
        EndUserMessage = result.Success ? "Successfully Updated Distribution Point" : result.Message;
    }

    protected void chkPrimary_OnCheckedChanged(object sender, EventArgs e)
    {
        PhysicalPath.Visible = chkPrimary.Checked;
    }



    protected void PopulateForm()
    {
        var distributionPoint = BLL.DistributionPoint.GetDistributionPoint(Convert.ToInt32(Request.QueryString["dpid"]));

        txtDisplayName.Text = distributionPoint.DisplayName;
        txtServer.Text = distributionPoint.Server;
        ddlProtocol.Text = distributionPoint.Protocol;
        txtShareName.Text = distributionPoint.ShareName;
        txtDomain.Text = distributionPoint.Domain;
        txtRwUsername.Text = distributionPoint.RwUsername;
        txtRwPassword.Text = distributionPoint.RwPassword;
        txtRoUsername.Text = distributionPoint.RoUsername;
        txtRoPassword.Text = distributionPoint.RoPassword;
        chkPrimary.Checked = Convert.ToBoolean(distributionPoint.IsPrimary);
        if (chkPrimary.Checked) PhysicalPath.Visible = true;
        txtPhysicalPath.Text = distributionPoint.PhysicalPath;

        txtBackendServer.Text = distributionPoint.BackendServer;


    }
}