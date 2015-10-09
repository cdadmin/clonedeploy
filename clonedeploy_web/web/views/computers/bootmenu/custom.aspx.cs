using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class views_computers_bootmenu_custom : BasePages.Computers
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack) PopulateForm();
    }

    protected void PopulateForm()
    {
        if (Helpers.Settings.ProxyDhcp == "Yes")
            divProxy.Visible = true;

        PopulateBootTemplatesDdl(ddlTemplates);
        var fixedLineEnding = scriptEditor.Value.Replace("\r\n", "\n");
    }

    protected void ddlTemplates_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        scriptEditor.Value = "";
        if (ddlTemplates.SelectedValue == "-1") return;
        scriptEditor.Value = BLL.BootTemplate.GetBootTemplate(Convert.ToInt32(ddlTemplates.SelectedValue)).Contents;
    }
}