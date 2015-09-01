using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Global;

public partial class views_admin_scripts_editcore : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        scriptEditor.Value = "";
        ddlCoreScripts.DataSource = Utility.GetScripts("core");
        ddlCoreScripts.DataBind();
        ddlCoreScripts.Items.Insert(0, "Select A Script");
    }

    protected void ddlCoreScripts_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        scriptEditor.Value = "";
        if (ddlCoreScripts.Text == "Select A Script") return;
        var path = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "data" +
                    Path.DirectorySeparatorChar + "clientscripts" + Path.DirectorySeparatorChar + "core" + Path.DirectorySeparatorChar + ddlCoreScripts.Text;


        scriptEditor.Value = File.ReadAllText(path);
    }

    protected void buttonSaveCore_OnClick(object sender, EventArgs e)
    {
        var path = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "data" +
                   Path.DirectorySeparatorChar + "clientscripts" + Path.DirectorySeparatorChar + "core" + Path.DirectorySeparatorChar + ddlCoreScripts.Text;

        var fixedLineEnding = scriptEditor.Value.Replace("\r\n", "\n");
        File.WriteAllText(path, fixedLineEnding);
        new Utility().Msgbox("Successfully Updated " + ddlCoreScripts.Text);
    }
}