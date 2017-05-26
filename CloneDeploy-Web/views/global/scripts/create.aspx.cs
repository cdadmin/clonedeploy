using System;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.global.scripts
{
    public partial class views_admin_scripts_create : Global
    {
        protected void btnSubmit_OnClick(object sender, EventArgs e)
        {
            RequiresAuthorization(AuthorizationStrings.CreateGlobal);
            var script = new ScriptEntity
            {
                Name = txtScriptName.Text,
                Description = txtScriptDesc.Text
            };
            var fixedLineEnding = scriptEditor.Value.Replace("\r\n", "\n");
            script.Contents = fixedLineEnding;
            var result = Call.ScriptApi.Post(script);
            if (result.Success)
            {
                EndUserMessage = "Successfully Created Script";
                Response.Redirect("~/views/global/scripts/edit.aspx?cat=sub1&scriptid=" + result.Id);
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