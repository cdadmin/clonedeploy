using System;
using System.Web.UI.WebControls;
using CloneDeploy_Common;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.global.boottemplates
{
    public partial class views_global_boottemplates_searchentry : Admin
    {
        protected void ButtonConfirmDelete_Click(object sender, EventArgs e)
        {
            RequiresAuthorization(AuthorizationStrings.DeleteGlobal);
            foreach (GridViewRow row in gvEntries.Rows)
            {
                var cb = (CheckBox) row.FindControl("chkSelector");
                if (cb == null || !cb.Checked) continue;
                var dataKey = gvEntries.DataKeys[row.RowIndex];
                if (dataKey == null) continue;
                Call.BootEntryApi.Delete(Convert.ToInt32(dataKey.Value));
            }

            PopulateGrid();
        }

        protected void chkSelectAll_OnCheckedChanged(object sender, EventArgs e)
        {
            ChkAll(gvEntries);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            PopulateGrid();
        }

        protected void PopulateGrid()
        {
            gvEntries.DataSource = Call.BootEntryApi.Get(int.MaxValue, txtSearch.Text);
            gvEntries.DataBind();

            lblTotal.Text = gvEntries.Rows.Count + " Result(s) / " + Call.BootEntryApi.GetCount() +
                            " Total Boot Entry(s)";
        }

        protected void txtSearch_OnTextChanged(object sender, EventArgs e)
        {
            PopulateGrid();
        }
    }
}