using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.admin.dp
{
    public partial class views_admin_dp_search : Admin
    {
        protected void ButtonConfirmDelete_Click(object sender, EventArgs e)
        {
            RequiresAuthorization(AuthorizationStrings.UpdateAdmin);
            var deletedCount = 0;
            foreach (GridViewRow row in gvDps.Rows)
            {
                var cb = (CheckBox) row.FindControl("chkSelector");
                if (cb == null || !cb.Checked) continue;
                var dataKey = gvDps.DataKeys[row.RowIndex];
                if (dataKey == null) continue;
                if (Call.DistributionPointApi.Delete(Convert.ToInt32(dataKey.Value)).Success)
                    deletedCount++;
            }
            EndUserMessage = "Successfully Deleted " + deletedCount + " Distribution Points";
            PopulateGrid();
        }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            ChkAll(gvDps);
        }

        protected void gridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            PopulateGrid();

            var listDistributionPoints = (List<DistributionPointEntity>) gvDps.DataSource;
            switch (e.SortExpression)
            {
                case "DisplayName":
                    listDistributionPoints = GetSortDirection(e.SortExpression) == "Asc"
                        ? listDistributionPoints.OrderBy(h => h.DisplayName).ToList()
                        : listDistributionPoints.OrderByDescending(h => h.DisplayName).ToList();
                    break;
            }

            gvDps.DataSource = listDistributionPoints;
            gvDps.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            PopulateGrid();
        }

        protected void PopulateGrid()
        {
            gvDps.DataSource = Call.DistributionPointApi.Get(int.MaxValue, txtSearch.Text);
            gvDps.DataBind();

            lblTotal.Text = gvDps.Rows.Count + " Result(s) / " + Call.DistributionPointApi.GetCount() +
                            " Distribution Point(s)";
        }

        protected void search_Changed(object sender, EventArgs e)
        {
            PopulateGrid();
        }
    }
}