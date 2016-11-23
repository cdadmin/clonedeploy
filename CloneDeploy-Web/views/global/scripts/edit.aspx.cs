using System;
using CloneDeploy_Web.Models;
using Helpers;

public partial class views_admin_scripts_edit : BasePages.Global
{
    public Script Script { get { return ReadProfile(); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) PopulateForm();
    }

    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.UpdateGlobal);
        var script = new Script
        {
            Id = Script.Id,
            Name = txtScriptName.Text,
            Description = txtScriptDesc.Text
        };
        var fixedLineEnding = scriptEditor.Value.Replace("\r\n", "\n");
        script.Contents = fixedLineEnding;
        var result = BLL.Script.UpdateScript(script);
        if (result.Success)
            EndUserMessage = "Successfully Updated Script";
        else
        
            EndUserMessage = result.Message;

    }

    protected void PopulateForm()
    {
        txtScriptName.Text = Script.Name;
        txtScriptDesc.Text = Script.Description;
        scriptEditor.Value = Script.Contents;
    }

    private Script ReadProfile()
    {
        return BLL.Script.GetScript(Convert.ToInt32(Request.QueryString["scriptid"]));

    }
}