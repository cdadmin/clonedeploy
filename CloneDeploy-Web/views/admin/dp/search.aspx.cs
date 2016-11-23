using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using BasePages;
using CloneDeploy_Web.Models;
using Helpers;

public partial class views_admin_dp_search : Admin
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        PopulateGrid();
    }

    protected void ButtonConfirmDelete_Click(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.UpdateAdmin);
        var deletedCount = 0;
        foreach (GridViewRow row in gvDps.Rows)
        {
            var cb = (CheckBox)row.FindControl("chkSelector");
            if (cb == null || !cb.Checked) continue;
            var dataKey = gvDps.DataKeys[row.RowIndex];
            if (dataKey == null) continue;
            if (BLL.DistributionPoint.DeleteDistributionPoint(Convert.ToInt32(dataKey.Value)))
                deletedCount++;
        }
        EndUserMessage = "Successfully Deleted " + deletedCount + " Distribution Points";
        PopulateGrid();
    }


    protected void gridView_Sorting(object sender, GridViewSortEventArgs e)
    {
        PopulateGrid();

        List<DistributionPoint> listDistributionPoints = (List<DistributionPoint>)gvDps.DataSource;
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

    protected void PopulateGrid()
    {
        gvDps.DataSource = BLL.DistributionPoint.SearchDistributionPoints(txtSearch.Text);
        gvDps.DataBind();

        lblTotal.Text = gvDps.Rows.Count + " Result(s) / " + BLL.DistributionPoint.TotalCount() + " Distribution Points(s)";
    }

    protected void search_Changed(object sender, EventArgs e)
    {
        PopulateGrid();
    }

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        ChkAll(gvDps);
    }
}