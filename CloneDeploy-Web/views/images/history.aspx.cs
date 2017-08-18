using System;
using System.Linq;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.images
{
    public partial class history : Images
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
            limit = ddlLimit.Text == "All" ? int.MaxValue : Convert.ToInt32(ddlLimit.Text);
            var listOfAuditLogs = Call.ImageApi.GetImageAuditLogs(Image.Id, limit);
            if (ddlType.SelectedValue != "All")
                listOfAuditLogs =
                    listOfAuditLogs.Where(c => c.AuditType.ToString() == ddlType.Text).Take(limit).ToList();
            gvHistory.DataSource = listOfAuditLogs;
            gvHistory.DataBind();
        }
    }
}