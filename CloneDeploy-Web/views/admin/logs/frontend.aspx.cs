using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CloneDeploy_Web.Helpers;

namespace CloneDeploy_Web.views.admin.logs
{
    public partial class frontend : BasePages.Admin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlLog.DataSource = Utility.GetFeLogs();
                ddlLog.DataBind();
                ddlLog.Items.Insert(0, "Select A Log");
                ddlLimit.SelectedValue = "10";

            }
            PopulateLogs();
        }

        protected void btnExportLog_Click(object sender, EventArgs e)
        {

            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.AppendHeader("Content-Disposition",
                "attachment; filename=" + ddlLog.Text);
            var log = Utility.GetLogContents(ddlLog.Text, Int32.MaxValue);
            var sb = new StringBuilder();
            foreach (var line in log)
            {
                sb.Append(line);
                sb.Append(Environment.NewLine);
            }
            HttpContext.Current.Response.Write(sb.ToString());
            HttpContext.Current.Response.End();

        }

        private void PopulateLogs()
        {
            if (ddlLog.Text != "Select A Log")
            {
                var limit = ddlLimit.Text == "All" ? int.MaxValue : Convert.ToInt32(ddlLimit.Text);
                gvLog.DataSource = Utility.GetLogContents(ddlLog.Text, limit);
                gvLog.DataBind();


            }
        }

        protected void ddlLimit_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateLogs();
        }
    }
}