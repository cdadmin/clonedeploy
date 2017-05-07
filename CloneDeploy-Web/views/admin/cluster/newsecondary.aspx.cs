using System;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

namespace CloneDeploy_Web.views.admin.cluster
{
    public partial class newsecondary : Admin
    {
        protected void btnUpdateSettings_OnClick(object sender, EventArgs e)
        {
            RequiresAuthorization(Authorizations.UpdateAdmin);
            var secondaryServer = new SecondaryServerEntity
            {
                ApiURL = txtApi.Text,
                ServiceAccountName = txtAccountName.Text,
                ServiceAccountPassword = txtAccountPassword.Text
            };

            var result = Call.SecondaryServerApi.Post(secondaryServer);
            if (result.Success)
            {
                EndUserMessage = "Successfully Created Secondary Server";
                Response.Redirect("~/views/admin/cluster/editsecondary.aspx?cat=sub1&ssid=" + result.Id);
            }
            else
            {
                EndUserMessage = result.ErrorMessage;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}