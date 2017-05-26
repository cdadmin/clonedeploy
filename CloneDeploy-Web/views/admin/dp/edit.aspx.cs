using System;
using CloneDeploy_Common;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.admin.dp
{
    public partial class views_admin_dp_edit : Admin
    {
        protected void buttonUpdateDp_OnClick(object sender, EventArgs e)
        {
            RequiresAuthorization(AuthorizationStrings.UpdateAdmin);
            var distributionPoint = Call.DistributionPointApi.Get(Convert.ToInt32(Request.QueryString["dpid"]));

            distributionPoint.DisplayName = txtDisplayName.Text;
            distributionPoint.Server = txtServer.Text;
            distributionPoint.Protocol = ddlProtocol.Text;
            distributionPoint.ShareName = txtShareName.Text;
            distributionPoint.Domain = txtDomain.Text;
            distributionPoint.RwUsername = txtRwUsername.Text;
            distributionPoint.RwPassword = txtRwPassword.Text;
            distributionPoint.RoUsername = txtRoUsername.Text;
            distributionPoint.RoPassword = txtRoPassword.Text;
            distributionPoint.IsPrimary = Convert.ToInt16(chkPrimary.Checked);
            distributionPoint.PhysicalPath = chkPrimary.Checked ? txtPhysicalPath.Text : "";
            distributionPoint.QueueSize = Convert.ToInt32(qSize.Text);
            distributionPoint.Location = ddlPrimaryType.Text;
            var result = Call.DistributionPointApi.Put(distributionPoint.Id, distributionPoint);
            EndUserMessage = result.Success ? "Successfully Updated Distribution Point" : result.ErrorMessage;
        }

        protected void chkPrimary_OnCheckedChanged(object sender, EventArgs e)
        {
            PrimaryParams.Visible = chkPrimary.Checked;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) PopulateForm();
        }


        protected void PopulateForm()
        {
            var distributionPoint = Call.DistributionPointApi.Get(Convert.ToInt32(Request.QueryString["dpid"]));
            

            txtDisplayName.Text = distributionPoint.DisplayName;
            txtServer.Text = distributionPoint.Server;
            ddlProtocol.Text = distributionPoint.Protocol;
            txtShareName.Text = distributionPoint.ShareName;
            txtDomain.Text = distributionPoint.Domain;
            txtRwUsername.Text = distributionPoint.RwUsername;           
            txtRoUsername.Text = distributionPoint.RoUsername;           
            chkPrimary.Checked = Convert.ToBoolean(distributionPoint.IsPrimary);          
            txtPhysicalPath.Text = distributionPoint.PhysicalPath;
            qSize.Text = distributionPoint.QueueSize.ToString();
            ddlPrimaryType.Text = distributionPoint.Location;
            if (ddlPrimaryType.Text == "Local") PhysicalPath.Visible = true;
            PrimaryParams.Visible = Convert.ToBoolean(distributionPoint.IsPrimary);
        }

        protected void ddlPrimaryType_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            PhysicalPath.Visible = ddlPrimaryType.Text == "Local";
        }
    }
}