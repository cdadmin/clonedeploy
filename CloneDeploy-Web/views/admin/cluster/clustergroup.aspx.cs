using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CloneDeploy_Web.Helpers;

namespace CloneDeploy_Web.views.admin.cluster
{
    public partial class clustergroup : BasePages.Admin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            PopulateGrid();
        }

        protected void PopulateGrid()
        {
            gvClusters.DataSource = Call.ClusterGroupApi.GetAll(Int32.MaxValue, txtSearch.Text);
            gvClusters.DataBind();
        }

        protected void search_Changed(object sender, EventArgs e)
        {
            PopulateGrid();
        }


        protected void ButtonConfirmDelete_Click(object sender, EventArgs e)
        {
            RequiresAuthorization(Authorizations.UpdateAdmin);
            foreach (GridViewRow row in gvClusters.Rows)
            {
                var cb = (CheckBox)row.FindControl("chkSelector");
                if (cb == null || !cb.Checked) continue;
                var dataKey = gvClusters.DataKeys[row.RowIndex];
                if (dataKey == null) continue;
                Call.ClusterGroupApi.Delete(Convert.ToInt32(dataKey.Value));
            }

            PopulateGrid();
        }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            ChkAll(gvClusters);
        }
    }
}