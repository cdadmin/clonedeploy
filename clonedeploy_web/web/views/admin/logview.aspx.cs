using System;
using System.IO;
using System.Web;
using BasePages;
using Helpers;
using Security;

namespace views.admin
{
    public partial class Logview : Admin
    {
        protected void btnExportLog_Click(object sender, EventArgs e)
        {
            try
            {
                var computerLogPath = ddlLog.Text;
                var logPath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "data" +
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

        protected void ddlLimit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlLog.Text != "Select A Log")
            {
                GridView1.DataSource = Logger.ViewLog(ddlLog.Text, ddlLimit.Text);
                GridView1.DataBind();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
           

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
            }
        }
    }
}