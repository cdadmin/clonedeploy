/*  
    CrucibleWDS A Windows Deployment Solution
    Copyright (C) 2011  Jon Dolny

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/.
 */

using System;
using System.IO;
using System.Web;
using System.Web.UI;
using Global;
using Helpers;
using Models;

namespace views.admin
{
    public partial class AdminSysprep : BasePages.Global
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void buttonShowCustom_OnClick(object sender, EventArgs e)
        {
            aceEditor.Visible = true;
            Custom.Visible = true;
            Core.Visible = false;
            scriptEditor.Value = "";
            ddlCustomScripts.DataSource = Utility.GetScripts("custom");
            ddlCustomScripts.DataBind();
            ddlCustomScripts.Items.Insert(0, "Select A Script");
        }

        protected void buttonShowCore_OnClick(object sender, EventArgs e)
        {
            aceEditor.Visible = true;
            Custom.Visible = false;
            Core.Visible = true;
            scriptEditor.Value = "";
            ddlCoreScripts.DataSource = Utility.GetScripts("core");
            ddlCoreScripts.DataBind();
            ddlCoreScripts.Items.Insert(0, "Select A Script");
        }

        protected void ddlCustomScripts_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            scriptEditor.Value = "";
            if (ddlCustomScripts.Text == "Select A Script") return;
            var path = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "data" +
                        Path.DirectorySeparatorChar + "clientscripts" + Path.DirectorySeparatorChar + "custom" + Path.DirectorySeparatorChar + ddlCustomScripts.Text;


            scriptEditor.Value = File.ReadAllText(path);
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
            File.WriteAllText(path,fixedLineEnding);
        }

        protected void buttonCreateScript_OnClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNewScript.Text))
            {
                return;
            }
            var path = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "data" +
                     Path.DirectorySeparatorChar + "clientscripts" + Path.DirectorySeparatorChar + "custom" + Path.DirectorySeparatorChar + txtNewScript.Text;

            var fixedLineEnding = scriptEditor.Value.Replace("\r\n", "\n");
            File.WriteAllText(path, fixedLineEnding);

            ddlCustomScripts.DataSource = Utility.GetScripts("custom");
            ddlCustomScripts.DataBind();
            ddlCustomScripts.Items.Insert(0, "Select A Script");
            ddlCustomScripts.SelectedValue = txtNewScript.Text;
        }

        protected void buttonDeleteScript_OnClick(object sender, EventArgs e)
        {
            var path = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "data" +
                  Path.DirectorySeparatorChar + "clientscripts" + Path.DirectorySeparatorChar + "custom" + Path.DirectorySeparatorChar + ddlCustomScripts.Text;
            File.Delete(path);
            ddlCustomScripts.DataSource = Utility.GetScripts("custom");
            ddlCustomScripts.DataBind();
            ddlCustomScripts.Items.Insert(0, "Select A Script");
            ddlCustomScripts.SelectedValue = "Select A Script";
            scriptEditor.Value = "";

        }

        protected void btnUpdateCustom_OnClick(object sender, EventArgs e)
        {
            var path = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "data" +
                      Path.DirectorySeparatorChar + "clientscripts" + Path.DirectorySeparatorChar + "custom" + Path.DirectorySeparatorChar + ddlCustomScripts.Text;

            var fixedLineEnding = scriptEditor.Value.Replace("\r\n", "\n");
            File.WriteAllText(path, fixedLineEnding);
        }
    }
}