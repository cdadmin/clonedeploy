using System;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.global.boottemplates
{
    public partial class views_global_boottemplates_edit : Global
    {
        protected void btnSubmit_OnClick(object sender, EventArgs e)
        {
            RequiresAuthorization(AuthorizationStrings.UpdateGlobal);
            var bootTemplate = new BootTemplateEntity
            {
                Id = BootTemplate.Id,
                Name = txtName.Text,
                Description = txtDescription.Text,
                Contents = txtContents.Text
            };

            var result = Call.BootTemplateApi.Put(bootTemplate.Id, bootTemplate);
            EndUserMessage = !result.Success ? result.ErrorMessage : "Successfully Updated Boot Menu Template";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) PopulateForm();
        }

        protected void PopulateForm()
        {
            txtName.Text = BootTemplate.Name;
            txtDescription.Text = BootTemplate.Description;
            txtContents.Text = BootTemplate.Contents;
        }
    }
}