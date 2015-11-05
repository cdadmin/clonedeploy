using System;
using Models;

public partial class views_admin_scripts_create : BasePages.Global
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {
        var script = new Script
        {
            Name = txtScriptName.Text,
            Description = txtScriptDesc.Text
        };
        var fixedLineEnding = scriptEditor.Value.Replace("\r\n", "\n");
        script.Contents = fixedLineEnding;
        BLL.Script.AddScript(script);
            //if (script.Create())
                //Response.Redirect("~/views/computers/edit.aspx?hostid=" + host.Id);

   }
}