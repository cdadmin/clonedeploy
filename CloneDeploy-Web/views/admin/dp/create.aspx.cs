using System;
using BasePages;
using CloneDeploy_Entities;
using CloneDeploy_Web;

public partial class views_admin_dp_create : Admin
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void buttonAddDp_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.UpdateAdmin);
        var distributionPoint = new DistributionPointEntity
        {
            DisplayName = txtDisplayName.Text,
            Server = txtServer.Text,
            Protocol = ddlProtocol.Text,
            ShareName = txtShareName.Text,
            Domain = txtDomain.Text,
            RwUsername = txtRwUsername.Text,
            RwPassword = new Encryption().EncryptText(txtRwPassword.Text),
            RoUsername = txtRoUsername.Text,
            RoPassword = new Encryption().EncryptText(txtRoPassword.Text),
            IsPrimary = Convert.ToInt16(chkPrimary.Checked),
            PhysicalPath = chkPrimary.Checked ? txtPhysicalPath.Text : "",           
           
        };

        var result = Call.DistributionPointApi.Post(distributionPoint);
        if (result.Success)
        {
            EndUserMessage = "Successfully Created Distribution Point";
            Response.Redirect("~/views/admin/dp/edit.aspx?level=2&dpid=" + distributionPoint.Id);
            
        }
        else
        {
            EndUserMessage = result.ErrorMessage;
        }

    }

    protected void chkPrimary_OnCheckedChanged(object sender, EventArgs e)
    {
        PhysicalPath.Visible = chkPrimary.Checked;
    }

}