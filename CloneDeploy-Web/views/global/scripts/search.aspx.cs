using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.global.scripts
{
    public partial class views_admin_scripts_search : Global
    {
        protected void ButtonConfirmDelete_Click(object sender, EventArgs e)
        {
            RequiresAuthorization(AuthorizationStrings.DeleteGlobal);
            foreach (GridViewRow row in gvScripts.Rows)
            {
                var cb = (CheckBox) row.FindControl("chkSelector");
                if (cb == null || !cb.Checked) continue;
                var dataKey = gvScripts.DataKeys[row.RowIndex];
                if (dataKey == null) continue;
                Call.ScriptApi.Delete(Convert.ToInt32(dataKey.Value));
            }

            PopulateGrid();
        }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            ChkAll(gvScripts);
        }


        protected void gridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            PopulateGrid();
            var listScripts = (List<ScriptEntity>) gvScripts.DataSource;
            switch (e.SortExpression)
            {
                case "Name":
                    listScripts = GetSortDirection(e.SortExpression) == "Asc"
                        ? listScripts.OrderBy(s => s.Name).ToList()
                        : listScripts.OrderByDescending(s => s.Name).ToList();
                    break;
            }


            gvScripts.DataSource = listScripts;
            gvScripts.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            PopulateGrid();
        }

        protected void PopulateGrid()
        {
            gvScripts.DataSource = Call.ScriptApi.Get(int.MaxValue, txtSearch.Text);
            gvScripts.DataBind();

            lblTotal.Text = gvScripts.Rows.Count + " Result(s) / " + Call.ScriptApi.GetCount() + " Total Script(s)";
        }

        protected void search_Changed(object sender, EventArgs e)
        {
            PopulateGrid();
        }
    }
}