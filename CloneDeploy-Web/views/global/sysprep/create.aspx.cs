using System;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.global.sysprep
{
    public partial class views_global_sysprep_create : Global
    {
        protected void btnSubmit_OnClick(object sender, EventArgs e)
        {
            RequiresAuthorization(AuthorizationStrings.CreateGlobal);
            var sysPrepTag = new SysprepTagEntity
            {
                Name = txtName.Text,
                OpeningTag = txtOpenTag.Text,
                ClosingTag = txtCloseTag.Text,
                Description = txtSysprepDesc.Text,
                Contents = txtContent.Text
            };

            var result = Call.SysprepTagApi.Post(sysPrepTag);
            if (result.Success)
            {
                EndUserMessage = "Successfully Created Sysprep Tag";
                Response.Redirect("~/views/global/sysprep/edit.aspx?cat=sub1&syspreptagid=" + result.Id);
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