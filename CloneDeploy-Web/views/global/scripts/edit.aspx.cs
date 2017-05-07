using System;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

public partial class views_admin_scripts_edit : Global
{
    public ScriptEntity Script
    {
        get { return ReadProfile(); }
    }

    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.UpdateGlobal);
        var script = new ScriptEntity
        {
            Id = Script.Id,
            Name = txtScriptName.Text,
            Description = txtScriptDesc.Text
        };
        var fixedLineEnding = scriptEditor.Value.Replace("\r\n", "\n");
        script.Contents = fixedLineEnding;
        var result = Call.ScriptApi.Put(script.Id, script);
        if (result.Success)
            EndUserMessage = "Successfully Updated Script";
        else

            EndUserMessage = result.ErrorMessage;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) PopulateForm();
    }

    protected void PopulateForm()
    {
        txtScriptName.Text = Script.Name;
        txtScriptDesc.Text = Script.Description;
        scriptEditor.Value = Script.Contents;
    }

    private ScriptEntity ReadProfile()
    {
        return Call.ScriptApi.Get(Convert.ToInt32(Request.QueryString["scriptid"]));
    }
}