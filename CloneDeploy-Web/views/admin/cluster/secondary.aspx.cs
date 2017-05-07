using System;
using System.Web.UI.WebControls;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

namespace CloneDeploy_Web.views.admin.cluster
{
    public partial class secondary : Admin
    {
        protected void ButtonConfirmDelete_Click(object sender, EventArgs e)
        {
            RequiresAuthorization(Authorizations.UpdateAdmin);
            foreach (GridViewRow row in gvServers.Rows)
            {
                var cb = (CheckBox) row.FindControl("chkSelector");
                if (cb == null || !cb.Checked) continue;
                var dataKey = gvServers.DataKeys[row.RowIndex];
                if (dataKey == null) continue;
                Call.SecondaryServerApi.Delete(Convert.ToInt32(dataKey.Value));
            }

            PopulateGrid();
        }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            ChkAll(gvServers);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            PopulateGrid();
        }

        protected void PopulateGrid()
        {
            gvServers.DataSource = Call.SecondaryServerApi.GetAll(int.MaxValue, txtSearch.Text);
            gvServers.DataBind();
        }

        protected void search_Changed(object sender, EventArgs e)
        {
            PopulateGrid();
        }
    }
}