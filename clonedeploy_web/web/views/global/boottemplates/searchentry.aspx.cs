using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Helpers;

public partial class views_global_boottemplates_searchentry : BasePages.Global
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;

        PopulateGrid();
    }

    protected void PopulateGrid()
    {
        gvEntries.DataSource = BLL.BootEntry.SearchBootEntrys(txtSearch.Text);
        gvEntries.DataBind();

        lblTotal.Text = gvEntries.Rows.Count + " Result(s) / " + BLL.BootEntry.TotalCount() + " Total Boot Entry(s)";
    }

    protected void txtSearch_OnTextChanged(object sender, EventArgs e)
    {
        PopulateGrid();
    }

   

    protected void chkSelectAll_OnCheckedChanged(object sender, EventArgs e)
    {
        ChkAll(gvEntries);
    }

    protected void ButtonConfirmDelete_Click(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.DeleteGlobal);
        foreach (GridViewRow row in gvEntries.Rows)
        {
            var cb = (CheckBox)row.FindControl("chkSelector");
            if (cb == null || !cb.Checked) continue;
            var dataKey = gvEntries.DataKeys[row.RowIndex];
            if (dataKey == null) continue;
            BLL.BootEntry.DeleteBootEntry(Convert.ToInt32(dataKey.Value));
        }

        PopulateGrid();
    }
}