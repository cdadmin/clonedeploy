using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CloneDeploy_Web.views.images
{
    public partial class history : BasePages.Images
    {
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

        protected void ddl_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        protected void ddlType_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateGrid();
        }
    }
}