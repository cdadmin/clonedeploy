using System;
using System.Web.UI.WebControls;

public partial class views_users_searchgroup : BasePages.Users
{
    protected void Page_Load(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.Administrator);

        if (IsPostBack) return;
        PopulateGrid();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        var deletedCount = 0;
        var adminMessage = string.Empty;
        foreach (GridViewRow row in gvGroups.Rows)
        {
            var cb = (CheckBox)row.FindControl("chkSelector");
            if (cb == null || !cb.Checked) continue;
            var dataKey = gvGroups.DataKeys[row.RowIndex];
            if (dataKey != null)
            {
                var userGroup = BLL.UserGroup.GetUserGroup(Convert.ToInt32(dataKey.Value));

                if (BLL.UserGroup.DeleteUserGroup(userGroup.Id))
                    deletedCount++;
            }
        }
        EndUserMessage = "Successfully Deleted " + deletedCount + " User Groups" + adminMessage;
        PopulateGrid();
    }

    protected void search_Changed(object sender, EventArgs e)
    {
        PopulateGrid();
    }

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        ChkAll(gvGroups);
    }

    protected void PopulateGrid()
    {
        gvGroups.DataSource = BLL.UserGroup.SearchUserGroups(txtSearch.Text).OrderBy(x => x.Name);
        gvGroups.DataBind();

        foreach (GridViewRow row in gvGroups.Rows)
        {
            var lbl = row.FindControl("lblCount") as Label;
            var dataKey = gvGroups.DataKeys[row.RowIndex];
            if (dataKey != null && lbl != null)
                lbl.Text = BLL.UserGroup.MemberCount(Convert.ToInt32(dataKey.Value));
        }

        lblTotal.Text = gvGroups.Rows.Count + " Result(s) / " + BLL.UserGroup.TotalCount() + " Total Group(s)";
    }
}