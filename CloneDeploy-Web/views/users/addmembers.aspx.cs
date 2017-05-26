using System;
using System.Web.UI.WebControls;
using CloneDeploy_Common;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.users
{
    public partial class views_users_addmembers : Users
    {
        protected void btnAddSelected_OnClick(object sender, EventArgs e)
        {
            RequiresAuthorization(AuthorizationStrings.Administrator);
            if (CloneDeployUserGroup.IsLdapGroup == 1)
            {
                EndUserMessage = "Users Cannot Be Added To LDAP Groups";
                return;
            }


            //Don't remove all administrators
            if (CloneDeployUserGroup.Membership == "User")
            {
                var existingAdminCount = Call.CloneDeployUserApi.GetAdminCount();
                var selectedAdminCount = 0;
                foreach (GridViewRow row in gvUsers.Rows)
                {
                    var cb = (CheckBox) row.FindControl("chkSelector");
                    if (cb == null || !cb.Checked) continue;
                    var dataKey = gvUsers.DataKeys[row.RowIndex];
                    if (dataKey != null)
                    {
                        var user = Call.CloneDeployUserApi.Get(Convert.ToInt32(dataKey.Value));
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
                var cb = (CheckBox) row.FindControl("chkSelector");
                if (cb == null || !cb.Checked) continue;
                var dataKey = gvUsers.DataKeys[row.RowIndex];
                if (dataKey != null)
                {
                    var user = Call.CloneDeployUserApi.Get(Convert.ToInt32(dataKey.Value));

                    Call.UserGroupApi.AddNewMember(CloneDeployUserGroup.Id, user.Id);
                    successCount++;
                }
            }
            EndUserMessage += "Successfully Added " + successCount + " Users To The Group";
            PopulateGrid();
        }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            ChkAll(gvUsers);
        }

        protected void ddlLimit_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            PopulateGrid();
        }

        protected void PopulateGrid()
        {
            gvUsers.DataSource = Call.CloneDeployUserApi.GetAll(int.MaxValue, txtSearch.Text);
            gvUsers.DataBind();
            lblTotal.Text = gvUsers.Rows.Count + " Result(s) / " + Call.CloneDeployUserApi.GetCount() + " Total User(s)";
        }

        protected void search_Changed(object sender, EventArgs e)
        {
            PopulateGrid();
        }
    }
}