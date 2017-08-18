using System;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.admin.dp
{
    public partial class views_admin_dp_create : Admin
    {
        protected void buttonAddDp_OnClick(object sender, EventArgs e)
        {
            RequiresAuthorization(AuthorizationStrings.UpdateAdmin);
            var distributionPoint = new DistributionPointEntity
            {
                DisplayName = txtDisplayName.Text,
                Server = txtServer.Text,
                Protocol = ddlProtocol.Text,
                ShareName = txtShareName.Text,
                Domain = txtDomain.Text,
                RwUsername = txtRwUsername.Text,
                RwPassword = txtRwPassword.Text,
                RoUsername = txtRoUsername.Text,
                RoPassword = txtRoPassword.Text,
                IsPrimary = Convert.ToInt16(chkPrimary.Checked),
                PhysicalPath = chkPrimary.Checked ? txtPhysicalPath.Text : "",
                QueueSize = Convert.ToInt32(qSize.Text),
                Location = ddlPrimaryType.Text
            };

            var result = Call.DistributionPointApi.Post(distributionPoint);
            if (result.Success)
            {
                EndUserMessage = "Successfully Created Distribution Point";
                Response.Redirect("~/views/admin/dp/edit.aspx?level=2&dpid=" + result.Id);
            }
            else
            {
                EndUserMessage = result.ErrorMessage;
            }
        }

        protected void chkPrimary_OnCheckedChanged(object sender, EventArgs e)
        {
            PrimaryParams.Visible = chkPrimary.Checked;
        }

        protected void ddlPrimaryType_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            PhysicalPath.Visible = ddlPrimaryType.Text == "Local";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            PhysicalPath.Visible = ddlPrimaryType.Text == "Local";
        }
    }
}