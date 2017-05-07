using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

public partial class views_users_groupacls_groupmanagement : Users
{
    protected void buttonUpdate_OnClick(object sender, EventArgs e)
    {
        var list = new List<UserGroupGroupManagementEntity>();
        foreach (GridViewRow row in gvGroups.Rows)
        {
            var cb = (CheckBox) row.FindControl("chkSelector");
            if (cb == null || !cb.Checked) continue;
            var dataKey = gvGroups.DataKeys[row.RowIndex];
            if (dataKey == null) continue;
            var userGroupManagement = new UserGroupGroupManagementEntity
            {
                UserGroupId = CloneDeployUserGroup.Id,
                GroupId = Convert.ToInt32(dataKey.Value)
            };
            list.Add(userGroupManagement);
        }

        Call.UserGroupApi.DeleteGroupManagements(CloneDeployUserGroup.Id);
        Call.UserGroupGroupManagementApi.Post(list);
        Call.UserGroupApi.UpdateMemberGroups(CloneDeployUserGroup.Id);
        EndUserMessage = "Updated Group Management";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.Administrator);
        if (!IsPostBack) PopulateForm();
    }

    protected void PopulateForm()
    {
        var listOfGroups = Call.UserGroupApi.GetGroupManagements(CloneDeployUserGroup.Id);

        gvGroups.DataSource = Call.GroupApi.GetAll(int.MaxValue, "");
        gvGroups.DataBind();

        foreach (GridViewRow row in gvGroups.Rows)
        {
            var chkBox = (CheckBox) row.FindControl("chkSelector");
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

    protected void SelectAll_CheckedChanged(object sender, EventArgs e)
    {
        ChkAll(gvGroups);
    }
}