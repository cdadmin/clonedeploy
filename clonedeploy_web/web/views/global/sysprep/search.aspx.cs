using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

public partial class views_global_sysprep_search : BasePages.Global
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;

        PopulateGrid();
    }

    protected void PopulateGrid()
    {
        gvSysprepTags.DataSource = BLL.SysprepTag.SearchSysprepTags(txtSearch.Text);
        gvSysprepTags.DataBind();

        lblTotal.Text = gvSysprepTags.Rows.Count + " Result(s) / " + BLL.SysprepTag.TotalCount() + " Total Sysprep Tag(s)";
    }

    protected void search_Changed(object sender, EventArgs e)
    {
        PopulateGrid();
    }

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        ChkAll(gvSysprepTags);
    }



    protected void gridView_Sorting(object sender, GridViewSortEventArgs e)
    {
        PopulateGrid();
        List<Models.SysprepTag> listSysprepTags = (List<Models.SysprepTag>)gvSysprepTags.DataSource;
        switch (e.SortExpression)
        {
            case "Name":
                listSysprepTags = GetSortDirection(e.SortExpression) == "Asc" ? listSysprepTags.OrderBy(s => s.Name).ToList() : listSysprepTags.OrderByDescending(s => s.Name).ToList();
                break;
            case "OpeningTag":
                listSysprepTags = GetSortDirection(e.SortExpression) == "Asc" ? listSysprepTags.OrderBy(s => s.OpeningTag).ToList() : listSysprepTags.OrderByDescending(s => s.OpeningTag).ToList();
                break;

        }


        gvSysprepTags.DataSource = listSysprepTags;
        gvSysprepTags.DataBind();
    }

    protected void ButtonConfirmDelete_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gvSysprepTags.Rows)
        {
            var cb = (CheckBox)row.FindControl("chkSelector");
            if (cb == null || !cb.Checked) continue;
            var dataKey = gvSysprepTags.DataKeys[row.RowIndex];
            if (dataKey == null) continue;
            BLL.SysprepTag.DeleteSysprepTag(Convert.ToInt32(dataKey.Value));
        }

        PopulateGrid();
    }
}