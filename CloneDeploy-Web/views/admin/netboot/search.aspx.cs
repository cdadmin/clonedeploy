using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CloneDeploy_Common;
using CloneDeploy_Entities;

namespace CloneDeploy_Web.views.admin.netboot
{
    public partial class search : BasePages.Admin
    {
        protected void ButtonConfirmDelete_Click(object sender, EventArgs e)
        {
            RequiresAuthorization(AuthorizationStrings.UpdateAdmin);
            var deletedCount = 0;
            foreach (GridViewRow row in gvProfiles.Rows)
            {
                var cb = (CheckBox)row.FindControl("chkSelector");
                if (cb == null || !cb.Checked) continue;
                var dataKey = gvProfiles.DataKeys[row.RowIndex];
                if (dataKey == null) continue;
                if (Call.NetBootProfileApi.Delete(Convert.ToInt32(dataKey.Value)).Success)
                    deletedCount++;
            }
            EndUserMessage = "Successfully Deleted " + deletedCount + " NetBoot Profile(s)";
            PopulateGrid();
        }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            ChkAll(gvProfiles);
        }


        protected void gridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            PopulateGrid();

            var listNetBootProfiles = (List<NetBootProfileEntity>)gvProfiles.DataSource;
            switch (e.SortExpression)
            {
                case "Name":
                    listNetBootProfiles = GetSortDirection(e.SortExpression) == "Asc"
                        ? listNetBootProfiles.OrderBy(h => h.Name).ToList()
                        : listNetBootProfiles.OrderByDescending(h => h.Name).ToList();
                    break;
            }


            gvProfiles.DataSource = listNetBootProfiles;
            gvProfiles.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            PopulateGrid();
        }

        protected void PopulateGrid()
        {
            gvProfiles.DataSource = Call.NetBootProfileApi.GetAll(txtSearch.Text);
            gvProfiles.DataBind();

            lblTotal.Text = gvProfiles.Rows.Count + " Result(s) / " + Call.NetBootProfileApi.GetCount() +
                            " NetBoot Profile(s)";
        }

        protected void search_Changed(object sender, EventArgs e)
        {
            PopulateGrid();
        }
    }
}