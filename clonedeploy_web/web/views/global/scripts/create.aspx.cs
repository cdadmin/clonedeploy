using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Global;
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
            Priority = Convert.ToInt32(txtPriority.Text),
            Description = txtScriptDesc.Text
        };
        var fixedLineEnding = scriptEditor.Value.Replace("\r\n", "\n");
        script.Contents = fixedLineEnding;
        new BLL.Script().AddScript(script);
            //if (script.Create())
                //Response.Redirect("~/views/computers/edit.aspx?hostid=" + host.Id);

   }
}