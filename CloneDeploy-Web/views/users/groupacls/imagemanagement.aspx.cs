using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.users.groupacls
{
    public partial class views_users_groupacls_imagemanagement : Users
    {
        protected void buttonUpdate_OnClick(object sender, EventArgs e)
        {
            var list = new List<UserGroupImageManagementEntity>();
            foreach (GridViewRow row in gvImages.Rows)
            {
                var cb = (CheckBox) row.FindControl("chkSelector");
                if (cb == null || !cb.Checked) continue;
                var dataKey = gvImages.DataKeys[row.RowIndex];
                if (dataKey == null) continue;
                var userImageManagement = new UserGroupImageManagementEntity
                {
                    UserGroupId = CloneDeployUserGroup.Id,
                    ImageId = Convert.ToInt32(dataKey.Value)
                };
                list.Add(userImageManagement);
            }

            Call.UserGroupApi.DeleteImageManagements(CloneDeployUserGroup.Id);
            Call.UserGroupImageManagementApi.Post(list);
            Call.UserGroupApi.UpdateMemberImages(CloneDeployUserGroup.Id);
            EndUserMessage = "Updated Image Management";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RequiresAuthorization(AuthorizationStrings.Administrator);
            if (!IsPostBack) PopulateForm();
        }

        protected void PopulateForm()
        {
            var listOfImages = Call.UserGroupApi.GetImageManagements(CloneDeployUserGroup.Id);

            gvImages.DataSource = Call.ImageApi.GetAll(int.MaxValue, "");
            gvImages.DataBind();

            foreach (GridViewRow row in gvImages.Rows)
            {
                var chkBox = (CheckBox) row.FindControl("chkSelector");
                var dataKey = gvImages.DataKeys[row.RowIndex];
                if (dataKey == null) continue;
                foreach (var groupManagement in listOfImages)
                {
                    if (groupManagement.ImageId == Convert.ToInt32(dataKey.Value))
                    {
                        chkBox.Checked = true;
                    }
                }
            }
        }

        protected void SelectAll_CheckedChanged(object sender, EventArgs e)
        {
            ChkAll(gvImages);
        }
    }
}