using System;
using Helpers;
using Models;

public partial class views_admin_scripts_create : BasePages.Global
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.CreateGlobal);
        var script = new Script
        {
            Name = txtScriptName.Text,
            Description = txtScriptDesc.Text
        };
        var fixedLineEnding = scriptEditor.Value.Replace("\r\n", "\n");
        script.Contents = fixedLineEnding;
        var result = BLL.Script.AddScript(script);
        if (result.Success)
        {
            EndUserMessage = "Successfully Created Script";
            Response.Redirect("~/views/global/scripts/edit.aspx?cat=sub1&scriptid=" + script.Id);
        }
        else
        {
            EndUserMessage = result.Message;
        }

    }
}