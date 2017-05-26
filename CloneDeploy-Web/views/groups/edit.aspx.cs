using System;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.groups
{
    public partial class GroupEdit : Groups
    {
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            RequiresAuthorizationOrManagedGroup(AuthorizationStrings.UpdateGroup, Group.Id);
            var group = new GroupEntity
            {
                Id = Group.Id,
                Name = txtGroupName.Text,
                Type = Group.Type,
                Description = txtGroupDesc.Text
            };

            var result = Call.GroupApi.Put(group.Id, group);
            EndUserMessage = !result.Success ? result.ErrorMessage : "Successfully Updated Group";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ddlGroupType.Enabled = false;
            if (!IsPostBack) PopulateForm();
        }

        protected void PopulateForm()
        {
            txtGroupName.Text = Group.Name;
            txtGroupDesc.Text = Group.Description;
            ddlGroupType.Text = Group.Type;
        }
    }
}