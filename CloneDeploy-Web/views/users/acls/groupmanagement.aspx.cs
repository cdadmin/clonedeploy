using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.users.acls
{
    public partial class views_users_acls_groupmanagement : Users
    {
        protected void buttonUpdate_OnClick(object sender, EventArgs e)
        {
            if (CloneDeployUser.UserGroupId != -1)
            {
                EndUserMessage = "Cannot Update. This User's Group Management Is Controlled By A Group";
                return;
            }

            var list = new List<UserGroupManagementEntity>();
            foreach (GridViewRow row in gvGroups.Rows)
            {
                var cb = (CheckBox) row.FindControl("chkSelector");
                if (cb == null || !cb.Checked) continue;
                var dataKey = gvGroups.DataKeys[row.RowIndex];
                if (dataKey == null) continue;
                var userGroupManagement = new UserGroupManagementEntity
                {
                    UserId = CloneDeployUser.Id,
                    GroupId = Convert.ToInt32(dataKey.Value)
                };
                list.Add(userGroupManagement);
            }

            Call.CloneDeployUserApi.DeleteGroupManagements(CloneDeployUser.Id);
            EndUserMessage = Call.UserGroupManagementApi.Post(list).Success
                ? "Successfully Updated Group Management"
                : "Could Not Update Group Management";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RequiresAuthorization(AuthorizationStrings.Administrator);
            if (!IsPostBack) PopulateForm();
        }

        protected void PopulateForm()
        {
            var listOfGroups = Call.CloneDeployUserApi.GetGroupManagements(CloneDeployUser.Id);

            gvGroups.DataSource = Call.GroupApi.Get(int.MaxValue, "");
            gvGroups.DataBind();

            chkEnabled.Checked = Convert.ToBoolean(CloneDeployUser.GroupManagementEnabled);

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

        protected void chkEnabled_OnCheckedChanged(object sender, EventArgs e)
        {
            if (CloneDeployUser.UserGroupId != -1)
            {
                EndUserMessage = "Cannot Update. This User's Group Management Is Controlled By A Group";
                return;
            }
            Call.CloneDeployUserApi.ToggleGroupManagement(CloneDeployUser.Id, chkEnabled.Checked ? 1 : 0);
        }
    }
}