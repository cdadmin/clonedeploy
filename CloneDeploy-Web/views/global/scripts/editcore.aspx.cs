using System;
using System.Web;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Web;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

public partial class views_admin_scripts_editcore : Global
{
    protected void Page_Load(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.Administrator);
        if (IsPostBack) return;
        scriptEditor.Value = "";
        ddlCoreScripts.DataSource = Call.FilesystemApi.GetScripts("core");
        ddlCoreScripts.DataBind();
        ddlCoreScripts.Items.Insert(0, "Select A Script");
    }

    protected void ddlCoreScripts_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        scriptEditor.Value = "";
        if (ddlCoreScripts.Text == "Select A Script") return;
        var path = Call.FilesystemApi.GetServerPaths("clientScript", ddlCoreScripts.Text);


        scriptEditor.Value = Call.FilesystemApi.ReadFileText(path);
    }

    protected void buttonSaveCore_OnClick(object sender, EventArgs e)
    {

        var fixedLineEnding = scriptEditor.Value.Replace("\r\n", "\n");
        var script = new CoreScriptDTO();
        script.Name = ddlCoreScripts.Text;
        script.Contents = fixedLineEnding;
        if(Call.FilesystemApi.WriteCoreScript(script))
        EndUserMessage = "Successfully Updated " + ddlCoreScripts.Text;
        else
        {
            EndUserMessage = "Could Not Update Script";
        }
    }
}