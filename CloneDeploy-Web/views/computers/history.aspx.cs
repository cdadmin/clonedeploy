using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CloneDeploy_Entities;

namespace CloneDeploy_Web.views.computers
{
    public partial class history : BasePages.Computers
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack) PopulateGrid();
        }

        protected void PopulateGrid()
        {
            var limit = 0;
            limit = ddlLimit.Text == "All" ? int.MaxValue : Convert.ToInt32(ddlLimit.Text);
            var listOfAuditLogs = Call.ComputerApi.GetComputerAuditLogs(Computer.Id,limit);
            gvHistory.DataSource = listOfAuditLogs;
            gvHistory.DataBind();       
        }

        protected void ddl_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateGrid();
        }
    }
}