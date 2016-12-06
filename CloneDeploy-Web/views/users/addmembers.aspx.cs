using System;
using System.Web.UI.WebControls;

public partial class views_users_addmembers : BasePages.Users
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        PopulateGrid();
    }

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        ChkAll(gvUsers);
    }

    protected void PopulateGrid()
    {
        gvUsers.DataSource = BLL.User.SearchUsers(txtSearch.Text);
        gvUsers.DataBind();
        lblTotal.Text = gvUsers.Rows.Count + " Result(s) / " + BLL.User.TotalCount() + " Total User(s)";
    }

    protected void search_Changed(object sender, EventArgs e)
    {
        PopulateGrid();
    }

    protected void btnAddSelected_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.Administrator);
        if (CloneDeployUserGroup.IsLdapGroup == 1)
        {
            EndUserMessage = "Users Cannot Be Added To LDAP Groups";
            return;
        }


        //Don't remove all administrators
        if (CloneDeployUserGroup.Membership == "User")
        {
            var existingAdminCount = BLL.User.GetAdminCount();
            var selectedAdminCount = 0;
            foreach (GridViewRow row in gvUsers.Rows)
            {
                var cb = (CheckBox) row.FindControl("chkSelector");
                if (cb == null || !cb.Checked) continue;
                var dataKey = gvUsers.DataKeys[row.RowIndex];
                if (dataKey != null)
                {
                    var user = BLL.User.GetUser(Convert.ToInt32(dataKey.Value));
                    if (user.Membership == "Administrator")
                        selectedAdminCount++;
                }
            }
            if (existingAdminCount == selectedAdminCount)
            {
                EndUserMessage = "Cannot Move Users To Group.  It Would Remove All Administrators From The System";
                return;
            }
        }

        var successCount = 0;
        foreach (GridViewRow row in gvUsers.Rows)
        {
            var cb = (CheckBox)row.FindControl("chkSelector");
            if (cb == null || !cb.Checked) continue;
            var dataKey = gvUsers.DataKeys[row.RowIndex];
            if (dataKey != null)
            {
                var user = BLL.User.GetUser(Convert.ToInt32(dataKey.Value));

                BLL.UserGroup.AddNewGroupMember(CloneDeployUserGroup, user);
                successCount++;

            }
        }
        EndUserMessage += "Successfully Added " + successCount +" Users To The Group";
        PopulateGrid();
    }

    protected void ddlLimit_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateGrid();
    }
}