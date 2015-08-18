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
using Security;

namespace views.admin
{
    public partial class Logview : Page
    {
        protected void btnExportLog_Click(object sender, EventArgs e)
        {
            try
            {
                var hostLogPath = ddlLog.Text;
                var logPath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "data" +
                              Path.DirectorySeparatorChar + "logs" + Path.DirectorySeparatorChar;
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + ddlLog.Text);
                HttpContext.Current.Response.TransmitFile(logPath + hostLogPath);
                HttpContext.Current.Response.End();
            }
            catch
            {
                // ignored
            }
        }

        protected void ddlLimit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlLog.Text != "Select A Log")
            {
                GridView1.DataSource = Logger.ViewLog(ddlLog.Text, ddlLimit.Text);
                GridView1.DataBind();
                Master.Master.Msgbox(Utility.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!new Authorize().IsInMembership("Administrator"))
                Response.Redirect("~/views/dashboard/dash.aspx?access=denied");

            if (!IsPostBack)
            {
                ddlLog.DataSource = Utility.GetLogs();
                ddlLog.DataBind();
                ddlLog.Items.Insert(0, "Select A Log");
                ddlLimit.SelectedValue = "10";
            }

            if (ddlLog.Text != "Select A Log")
            {
                GridView1.DataSource = Logger.ViewLog(ddlLog.Text, ddlLimit.Text);
                GridView1.DataBind();
                Master.Master.Msgbox(Utility.Message);
            }
        }
    }
}