using System;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.global.boottemplates
{
    public partial class views_global_boottemplates_createentry : Admin
    {
        protected void btnSubmit_OnClick(object sender, EventArgs e)
        {
            RequiresAuthorization(AuthorizationStrings.CreateGlobal);
            var bootEntry = new BootEntryEntity
            {
                Name = txtName.Text,
                Description = txtDescription.Text,
                Content = txtContents.Text,
                Type = ddlType.Text,
                Order = txtOrder.Text,
                Active = chkActive.Checked ? 1 : 0,
                Default = chkDefault.Checked ? 1 : 0
            };

            var result = Call.BootEntryApi.Post(bootEntry);
            if (!result.Success)
                EndUserMessage = result.ErrorMessage;
            else
            {
                EndUserMessage = "Successfully Added Boot Menu Entry";
                Response.Redirect("~/views/admin/bootmenu/editentry.aspx?&entryid=" + result.Id);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}