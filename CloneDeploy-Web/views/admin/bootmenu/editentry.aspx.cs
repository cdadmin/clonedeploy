using System;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.global.boottemplates
{
    public partial class views_global_boottemplates_editentry : Admin
    {
        protected void btnSubmit_OnClick(object sender, EventArgs e)
        {
            RequiresAuthorization(AuthorizationStrings.UpdateGlobal);
            var bootEntry = new BootEntryEntity
            {
                Id = BootEntry.Id,
                Name = txtName.Text,
                Description = txtDescription.Text,
                Content = txtContents.Text,
                Type = ddlType.Text,
                Order = txtOrder.Text,
                Active = chkActive.Checked ? 1 : 0,
                Default = chkDefault.Checked ? 1 : 0
            };

            var result = Call.BootEntryApi.Put(bootEntry.Id, bootEntry);
            EndUserMessage = !result.Success ? result.ErrorMessage : "Successfully Updated Boot Menu Entry";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) PopulateForm();
        }

        protected void PopulateForm()
        {
            txtName.Text = BootEntry.Name;
            txtDescription.Text = BootEntry.Description;
            txtContents.Text = BootEntry.Content;
            ddlType.Text = BootEntry.Type;
            txtOrder.Text = BootEntry.Order;
            if (BootEntry.Active == 1) chkActive.Checked = true;
            if (BootEntry.Default == 1) chkDefault.Checked = true;
        }
    }
}