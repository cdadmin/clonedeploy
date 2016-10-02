using System;
using BasePages;
using Helpers;
using Group = Models.Group;

namespace views.groups
{
    public partial class GroupEdit : Groups
    {
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            RequiresAuthorizationOrManagedGroup(Authorizations.UpdateGroup, Group.Id); 
            var group = new Group
            {
                Id = Group.Id,
                Name = txtGroupName.Text,
                Type = Group.Type,
                Description = txtGroupDesc.Text,
            };

            var result = BLL.Group.UpdateGroup(group);
            EndUserMessage = !result.IsValid ? result.Message : "Successfully Updated Group";              
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