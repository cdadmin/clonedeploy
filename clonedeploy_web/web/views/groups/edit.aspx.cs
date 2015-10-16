using System;
using BasePages;
using BLL;
using Group = Models.Group;

namespace views.groups
{
    public partial class GroupEdit : Groups
    {
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var group = new Group
            {
                Id = Group.Id,
                Name = txtGroupName.Text,
                Type = Group.Type,
                Description = txtGroupDesc.Text,
            };

           BLL.Group.UpdateGroup(group);         
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