using System;
using CloneDeploy_Entities;
using CloneDeploy_Web;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

namespace views.groups
{
    public partial class GroupCreate : Groups
    {
        protected void Page_Load(object sender, EventArgs e)
        {
                      
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            RequiresAuthorization(Authorizations.CreateGroup); 
            if(ddlGroupType.Text == "smart")
                RequiresAuthorization(Authorizations.CreateSmart);
            var group = new GroupEntity
            {
                Name = txtGroupName.Text,
                Description = txtGroupDesc.Text,
                Type = ddlGroupType.Text,
            };

            var result = Call.GroupApi.Post(group);
            if (!result.Success)
                EndUserMessage = result.ErrorMessage;
            else
            {
                EndUserMessage = "Successfully Created Group";
                Response.Redirect("~/views/groups/edit.aspx?groupid=" + group.Id);
            }            
        }


      
    }
}