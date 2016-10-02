using System;
using BasePages;
using Helpers;
using Models;

public partial class views_admin_dp_create : Admin
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void buttonAddDp_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.UpdateAdmin);
        var distributionPoint = new DistributionPoint
        {
            DisplayName = txtDisplayName.Text,
            Server = txtServer.Text,
            Protocol = ddlProtocol.Text,
            ShareName = txtShareName.Text,
            Domain = txtDomain.Text,
            RwUsername = txtRwUsername.Text,
            RwPassword = new Helpers.Encryption().EncryptText(txtRwPassword.Text),
            RoUsername = txtRoUsername.Text,
            RoPassword = new Helpers.Encryption().EncryptText(txtRoPassword.Text),
            IsPrimary = Convert.ToInt16(chkPrimary.Checked),
            PhysicalPath = chkPrimary.Checked ? txtPhysicalPath.Text : "",           
           
        };

        var result = BLL.DistributionPoint.AddDistributionPoint(distributionPoint);
        if (result.IsValid)
        {
            EndUserMessage = "Successfully Created Distribution Point";
            Response.Redirect("~/views/admin/dp/edit.aspx?level=2&dpid=" + distributionPoint.Id);
            
        }
        else
        {
            EndUserMessage = result.Message;
        }

    }

    protected void chkPrimary_OnCheckedChanged(object sender, EventArgs e)
    {
        PhysicalPath.Visible = chkPrimary.Checked;
    }

}