using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Helpers;

public partial class views_admin_dp_search : BasePages.Admin
{
    private readonly BLL.DistributionPoint _bllDistributionPoint = new BLL.DistributionPoint();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;

      
        PopulateGrid();
    }

    protected void ButtonConfirmDelete_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gvDps.Rows)
        {
            var cb = (CheckBox)row.FindControl("chkSelector");
            if (cb == null || !cb.Checked) continue;
            var dataKey = gvDps.DataKeys[row.RowIndex];
            if (dataKey == null) continue;
            _bllDistributionPoint.DeleteDistributionPoint(Convert.ToInt32(dataKey.Value));
        }

        PopulateGrid();
    }


    protected void gridView_Sorting(object sender, GridViewSortEventArgs e)
    {
        PopulateGrid();

        List<Models.DistributionPoint> listDistributionPoints = (List<Models.DistributionPoint>)gvDps.DataSource;
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
        gvDps.DataSource = _bllDistributionPoint.SearchDistributionPoints(txtSearch.Text);
        gvDps.DataBind();

        lblTotal.Text = gvDps.Rows.Count + " Result(s) / " + _bllDistributionPoint.TotalCount() + " Distribution Points(s)";
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