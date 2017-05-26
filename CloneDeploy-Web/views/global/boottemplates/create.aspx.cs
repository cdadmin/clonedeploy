using System;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.global.boottemplates
{
    public partial class views_global_boottemplates_create : Global
    {
        protected void btnSubmit_OnClick(object sender, EventArgs e)
        {
            RequiresAuthorization(AuthorizationStrings.CreateGlobal);
            var bootTemplate = new BootTemplateEntity
            {
                Name = txtName.Text,
                Description = txtDescription.Text,
                Contents = txtContents.Text
            };

            var result = Call.BootTemplateApi.Post(bootTemplate);
            if (!result.Success)
                EndUserMessage = result.ErrorMessage;
            else
            {
                EndUserMessage = "Successfully Added Boot Menu Template";
                Response.Redirect("~/views/global/boottemplates/edit.aspx?cat=sub1&templateid=" + result.Id);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}