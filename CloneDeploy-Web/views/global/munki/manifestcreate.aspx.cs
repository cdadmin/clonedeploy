using System;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.global.munki
{
    public partial class views_global_munki_manifestcreate : Global
    {
        protected void btnSubmit_OnClick(object sender, EventArgs e)
        {
            RequiresAuthorization(AuthorizationStrings.CreateGlobal);
            var manifestTemplate = new MunkiManifestTemplateEntity
            {
                Name = txtManifestName.Text,
                Description = txtManifestDesc.Text
            };

            var result = Call.MunkiManifestTemplateApi.Post(manifestTemplate);
            if (result.Success)
            {
                EndUserMessage = "Successfully Created Manifest Template";
                Response.Redirect("~/views/global/munki/general.aspx?cat=sub2&manifestid=" + result.Id);
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