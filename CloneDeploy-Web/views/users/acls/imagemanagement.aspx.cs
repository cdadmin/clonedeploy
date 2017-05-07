using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

public partial class views_users_acls_imagemanagement : Users
{
    protected void buttonUpdate_OnClick(object sender, EventArgs e)
    {
        if (CloneDeployUser.UserGroupId != -1)
        {
            EndUserMessage = "Cannot Update. This User's Image Management Is Controlled By A Group";
            return;
        }

        var list = new List<UserImageManagementEntity>();
        foreach (GridViewRow row in gvImages.Rows)
        {
            var cb = (CheckBox) row.FindControl("chkSelector");
            if (cb == null || !cb.Checked) continue;
            var dataKey = gvImages.DataKeys[row.RowIndex];
            if (dataKey == null) continue;
            var userImageManagement = new UserImageManagementEntity
            {
                UserId = CloneDeployUser.Id,
                ImageId = Convert.ToInt32(dataKey.Value)
            };
            list.Add(userImageManagement);
        }

        Call.CloneDeployUserApi.DeleteImageManagements(CloneDeployUser.Id);
        EndUserMessage = Call.UserImageManagementApi.Post(list).Success
            ? "Successfully Updated Image Management"
            : "Could Not Update Image Management";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.Administrator);
        if (!IsPostBack) PopulateForm();
    }

    protected void PopulateForm()
    {
        var listOfImages = Call.CloneDeployUserApi.GetImageManagements(CloneDeployUser.Id);

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