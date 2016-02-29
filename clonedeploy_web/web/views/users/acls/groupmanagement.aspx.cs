using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Helpers;

public partial class views_users_acls_groupmanagement : BasePages.Users
{
    protected void Page_Load(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.Administrator);
        if (!IsPostBack) PopulateForm();
    }

    protected void SelectAll_CheckedChanged(object sender, EventArgs e)
    {
        ChkAll(gvGroups);
    }

    protected void PopulateForm()
    {
        var listOfGroups = BLL.UserGroupManagement.Get(CloneDeployUser.Id);
       
        gvGroups.DataSource = BLL.Group.SearchGroups();
        gvGroups.DataBind();

        foreach (GridViewRow row in gvGroups.Rows)
        {
            var chkBox = (CheckBox)row.FindControl("chkSelector");
            var dataKey = gvGroups.DataKeys[row.RowIndex];
            if (dataKey == null) continue;
            foreach (var groupManagement in listOfGroups)
            {
                if (groupManagement.GroupId == Convert.ToInt32(dataKey.Value))
                {
                    chkBox.Checked = true;
                }
            }
        }
    }

    protected void buttonUpdate_OnClick(object sender, EventArgs e)
    {
        var list = new List<Models.UserGroupManagement>();
        foreach (GridViewRow row in gvGroups.Rows)
        {
            var cb = (CheckBox)row.FindControl("chkSelector");
            if (cb == null || !cb.Checked) continue;
            var dataKey = gvGroups.DataKeys[row.RowIndex];
            if (dataKey == null) continue;
            var userGroupManagement = new Models.UserGroupManagement
            {
                UserId = CloneDeployUser.Id,
                GroupId = Convert.ToInt32(dataKey.Value)
            };
            list.Add(userGroupManagement);

        }

        BLL.UserGroupManagement.DeleteUserGroupManagements(CloneDeployUser.Id);
        EndUserMessage = BLL.UserGroupManagement.AddUserGroupManagements(list)
            ? "Successfully Updated Group Management"
            : "Could Not Update Group Management";

    }
}