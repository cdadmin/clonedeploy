using System;
using BasePages;
using Helpers;
using Models;
using Security;

namespace views.groups
{
    public partial class GroupCreate : Groups
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RequiresAuthorization(Authorizations.CreateGroup);           
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            var group = new Models.Group
            {
                Name = txtGroupName.Text,
                Description = txtGroupDesc.Text,
                Type = ddlGroupType.Text,
            };

            var result = BLL.Group.AddGroup(group);
            if (!result.IsValid)
                EndUserMessage = result.Message;
            else
            {
                EndUserMessage = "Successfully Created Group";
                Response.Redirect("~/views/groups/edit.aspx?groupid=" + group.Id);
            }            
        }


      
    }
}