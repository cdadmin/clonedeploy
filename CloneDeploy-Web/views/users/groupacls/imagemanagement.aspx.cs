using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using CloneDeploy_Entities;
using CloneDeploy_Web;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

public partial class views_users_groupacls_imagemanagement : Users
{
    protected void Page_Load(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.Administrator);
        if (!IsPostBack) PopulateForm();
    }

    protected void SelectAll_CheckedChanged(object sender, EventArgs e)
    {
        ChkAll(gvImages);
    }

    protected void PopulateForm()
    {
        var listOfImages = Call.UserGroupApi.GetImageManagements(CloneDeployUserGroup.Id);

        gvImages.DataSource = Call.ImageApi.GetAll(Int32.MaxValue,"");
        gvImages.DataBind();

        foreach (GridViewRow row in gvImages.Rows)
        {
            var chkBox = (CheckBox)row.FindControl("chkSelector");
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

    protected void buttonUpdate_OnClick(object sender, EventArgs e)
    {
        var list = new List<UserGroupImageManagementEntity>();
        foreach (GridViewRow row in gvImages.Rows)
        {
            var cb = (CheckBox)row.FindControl("chkSelector");
            if (cb == null || !cb.Checked) continue;
            var dataKey = gvImages.DataKeys[row.RowIndex];
            if (dataKey == null) continue;
            var userImageManagement = new UserGroupImageManagementEntity()
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
}