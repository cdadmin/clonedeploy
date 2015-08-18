using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Global;
using Models;

namespace views.hosts
{
    public partial class HostLog : Page
    {
        public Host Host { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Host = new Host { Id = Convert.ToInt16(Request["hostid"]) };
            Host.Read();
            var subTitle = Master.Master.FindControl("SubNav").FindControl("labelSubTitle") as Label;
            if (subTitle != null) subTitle.Text = Host.Name + " | Logs";
            if (!IsPostBack) PopulateLog();
        }

        protected void btnExportLog_Click(object sender, EventArgs e)
        {
            string logType;
            switch (ddlLogType.Text)
            {
                case "Upload":
                    logType = ".upload";
                    break;
                case "Deploy":
                    logType = ".download";
                    break;
                default:
                    return;
            }
            try
            {
                var hostLogPath = "hosts" + Path.DirectorySeparatorChar + Host.Id + logType;
                var logPath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "data" +
                              Path.DirectorySeparatorChar + "logs" + Path.DirectorySeparatorChar;
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AppendHeader("Content-Disposition",
                    "attachment; filename=" + Host.Id + logType);
                HttpContext.Current.Response.TransmitFile(logPath + hostLogPath);
                HttpContext.Current.Response.End();
            }
            catch
            {
                // ignored
            }
        }

        protected void ddlLogLimit_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateLog();
        }

        protected void ddlLogType_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateLog();
        }

        protected void PopulateLog()
        {
            if (!IsPostBack)
                ddlLogLimit.SelectedValue = "All";

            if (ddlLogType.Text == "Select A Log") return;
            var logType = ddlLogType.Text == "Upload" ? ".upload" : ".download";

            gvHostLog.DataSource = Logger.ViewLog("hosts" + Path.DirectorySeparatorChar + Host.Id + logType,
                ddlLogLimit.Text);
            gvHostLog.DataBind();
            Master.Msgbox(Utility.Message);
        }
    }
}