using System;
using BasePages;
using CloneDeploy_Entities;
using CloneDeploy_Web;

namespace views.masters
{
    public partial class UserMaster : BasePages.MasterBaseMaster
    {
        private Users UsersBasePage { get; set; }
        public CloneDeployUserEntity CloneDeployUser { get; set; }
        public CloneDeployUserGroupEntity CloneDeployUserGroup { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            UsersBasePage = (Page as Users);
            CloneDeployUser = UsersBasePage.CloneDeployUser;
            CloneDeployUserGroup = UsersBasePage.CloneDeployUserGroup;
            if (CloneDeployUser == null && CloneDeployUserGroup == null) //level 1
            {
                Level2.Visible = false;
                Level2Group.Visible = false;
                Level3.Visible = false;
                btnDelete.Visible = false;
            }
            else
            {
                if (CloneDeployUser == null)
                {
                    Level2.Visible = false;
                    btnDelete.Visible = false;
                    btnDeleteGroup.Visible = true;
                }
                if (CloneDeployUserGroup == null)
                {
                    Level2Group.Visible = false;
                    btnDelete.Visible = true;
                    btnDeleteGroup.Visible = false;
                }

                Level1.Visible = false;
                if (Request.QueryString["level"] == "3")
                {
                    Level2.Visible = false;
                    Level2Group.Visible = false;
                }
            }

            if (UsersBasePage.CloneDeployCurrentUser.Membership == "User")
                Level1.Visible = false;

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            UsersBasePage.RequiresAuthorization(Authorizations.Administrator);
            if (UsersBasePage.Call.CloneDeployUserApi.GetAdminCount() == 1 && CloneDeployUser.Membership == "Administrator")
            {
                PageBaseMaster.EndUserMessage = "There Must Be At Least One Administrator";
            }
            else
            {
                lblTitle.Text = "Delete This User?";
                Page.ClientScript.RegisterStartupScript(GetType(), "modalscript",
                    "$(function() {  var menuTop = document.getElementById('confirmbox'),body = document.body;classie.toggle(menuTop, 'confirm-box-outer-open'); });",
                    true);
            }
        }

        protected void btnDeleteGroup_Click(object sender, EventArgs e)
        {
            UsersBasePage.RequiresAuthorization(Authorizations.Administrator);
           
                lblTitle.Text = "Delete This User Group?";
                Page.ClientScript.RegisterStartupScript(GetType(), "modalscript",
                    "$(function() {  var menuTop = document.getElementById('confirmbox'),body = document.body;classie.toggle(menuTop, 'confirm-box-outer-open'); });",
                    true);
            
        }

        protected void OkButton_Click(object sender, EventArgs e)
        {
            if (CloneDeployUser != null)
            {

                if (UsersBasePage.Call.CloneDeployUserApi.Delete(CloneDeployUser.Id).Success)
                {
                    PageBaseMaster.EndUserMessage = "Successfully Deleted User";
                    Response.Redirect("~/views/users/search.aspx");
                }
                else
                {
                    PageBaseMaster.EndUserMessage = "Could Not Delete User";
                }
            }
            else if (CloneDeployUserGroup != null)
            {
                if (UsersBasePage.Call.UserGroupApi.Delete(CloneDeployUserGroup.Id).Success)
                {
                    PageBaseMaster.EndUserMessage = "Successfully Deleted User Group";
                    Response.Redirect("~/views/users/searchgroup.aspx");
                }
                else
                {
                    PageBaseMaster.EndUserMessage = "Could Not Delete User Group";
                }
            }

        }       
    }
}