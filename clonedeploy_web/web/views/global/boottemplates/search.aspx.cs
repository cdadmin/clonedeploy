using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class views_global_boottemplates_search : BasePages.Global
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;

        PopulateGrid();
    }

    protected void PopulateGrid()
    {
        gvTemplates.DataSource = BLL.BootTemplate.SearchBootTemplates(txtSearch.Text);
        gvTemplates.DataBind();

        lblTotal.Text = gvTemplates.Rows.Count + " Result(s) / " + BLL.BootTemplate.TotalCount() + " Total Boot Template(s)";
    }

    protected void txtSearch_OnTextChanged(object sender, EventArgs e)
    {
        PopulateGrid();
    }

    protected void gvTemplates_OnSorting(object sender, GridViewSortEventArgs e)
    {
        PopulateGrid();
        List<Models.BootTemplate> listSysprepTags = (List<Models.BootTemplate>)gvTemplates.DataSource;
        switch (e.SortExpression)
        {
            case "Name":
                listSysprepTags = GetSortDirection(e.SortExpression) == "Asc" ? listSysprepTags.OrderBy(s => s.Name).ToList() : listSysprepTags.OrderByDescending(s => s.Name).ToList();
                break;
         
        }


        gvTemplates.DataSource = listSysprepTags;
        gvTemplates.DataBind();
    }

    protected void chkSelectAll_OnCheckedChanged(object sender, EventArgs e)
    {
        ChkAll(gvTemplates);
    }

    protected void ButtonConfirmDelete_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gvTemplates.Rows)
        {
            var cb = (CheckBox)row.FindControl("chkSelector");
            if (cb == null || !cb.Checked) continue;
            var dataKey = gvTemplates.DataKeys[row.RowIndex];
            if (dataKey == null) continue;
            BLL.BootTemplate.DeleteBootTemplate(Convert.ToInt32(dataKey.Value));
        }

        PopulateGrid();
    }
}