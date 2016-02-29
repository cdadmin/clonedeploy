using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BasePages;
using Helpers;

namespace views.admin
{
    public partial class Logview : Admin
    {
        protected void btnExportLog_Click(object sender, EventArgs e)
        {
            try
            {
                var computerLogPath = ddlLog.Text;
                var logPath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "private" +
                              Path.DirectorySeparatorChar + "logs" + Path.DirectorySeparatorChar;
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + ddlLog.Text);
                HttpContext.Current.Response.TransmitFile(logPath + computerLogPath);
                HttpContext.Current.Response.End();
            }
            catch
            {
                // ignored
            }
        }

        private void PopulateLogs()
        {
            if (ddlLog.Text != "Select A Log")
            {
                if (ddlLog.Text == "On Demand")
                {
                    
                    var limit = ddlDbLimit.Text == "All" ? int.MaxValue : Convert.ToInt32(ddlDbLimit.Text);
                    dbView.Visible = true;
                    fileView.Visible = false;
                    gvLogs.DataSource = BLL.ComputerLog.SearchOnDemand(limit);
                    gvLogs.DataBind();
                }
                else
                {
                    fileView.Visible = true;
                    dbView.Visible = false;
                    GridView1.DataSource = Logger.ViewLog(ddlLog.Text, ddlLimit.Text);
                    GridView1.DataBind();
                }

            }
        }
        protected void ddlLimit_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateLogs();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlLog.DataSource = Utility.GetLogs();
                ddlLog.DataBind();
                ddlLog.Items.Insert(0, "Select A Log");
                ddlLog.Items.Insert(1, "On Demand");
                ddlLimit.SelectedValue = "10";
               
            }
            PopulateLogs();
           
            
        }

        protected void ddlDbLimit_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateLogs();
        }

        protected void btnExport_OnClick(object sender, EventArgs e)
        {
            var control = sender as Control;
            if (control == null) return;
            var gvRow = (GridViewRow)control.Parent.Parent;
            var dataKey = gvLogs.DataKeys[gvRow.RowIndex];
            if (dataKey == null) return;
            var log = BLL.ComputerLog.GetComputerLog(Convert.ToInt32(dataKey.Value));
            Export(gvRow.Cells[2].Text + "-" + log.SubType + ".txt", log.Contents);
        }

        protected void btnView_OnClick(object sender, EventArgs e)
        {
            var control = sender as Control;
            if (control == null) return;
            var gvRow = (GridViewRow)control.Parent.Parent;
            var dataKey = gvLogs.DataKeys[gvRow.RowIndex];
            if (dataKey == null) return;
            var log = BLL.ComputerLog.GetComputerLog(Convert.ToInt32(dataKey.Value));

            dbView.Visible = false;
            ViewLog.Visible = true;

            // I didn't want a textbox for this, that's why it seems strange.
            var text = new List<string>();
            using (var reader = new StringReader(log.Contents))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                    text.Add(line);
            }
            gvLogView.DataSource = text;
            gvLogView.DataBind();


        }
    }
}