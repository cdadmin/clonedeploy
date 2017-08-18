using System;
using System.Linq;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.computers
{
    public partial class history : Computers
    {
        protected void ddl_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        protected void ddlType_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) PopulateGrid();
        }

        protected void PopulateGrid()
        {
            var limit = 0;
            limit = ddlLimit.Text == "Select Filter" ? int.MaxValue : Convert.ToInt32(ddlLimit.Text);
            var listOfAuditLogs = Call.ComputerApi.GetComputerAuditLogs(Computer.Id, limit);

            if (ddlType.SelectedValue != "Select Filter")
                listOfAuditLogs =
                    listOfAuditLogs.Where(c => c.AuditType.ToString() == ddlType.Text).Take(limit).ToList();
            gvHistory.DataSource = listOfAuditLogs;
            gvHistory.DataBind();
        }
    }
}