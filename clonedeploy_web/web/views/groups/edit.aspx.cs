using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Global;
using Models;
using Security;


namespace views.groups
{
    public partial class GroupEdit : BasePages.Groups
    {
        private readonly BLL.LinuxProfile _bllLinuxProfile = new BLL.LinuxProfile();

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var group = new Models.Group
            {
                Id = Group.Id,
                Name = txtGroupName.Text,
                Type = Group.Type,
                Image = Convert.ToInt32(ddlGroupImage.SelectedValue),
                ImageProfile = Convert.ToInt32(ddlGroupImage.SelectedValue) == 0 ? 0 : Convert.ToInt32(ddlImageProfile.SelectedValue),
                Description = txtGroupDesc.Text,
                SenderArguments = txtGroupSenderArgs.Text,
                ReceiverArguments = txtGroupReceiveArgs.Text

            };

           new BLL.Group().UpdateGroup(group);
           

        }

        protected void ddlGroupImage_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlGroupImage.Text == "Select Image") return;
            ddlImageProfile.DataSource = _bllLinuxProfile.SearchProfiles(Convert.ToInt32(ddlGroupImage.SelectedValue)).Select(i => new { i.Id, i.Name });
            ddlImageProfile.DataValueField = "Id";
            ddlImageProfile.DataTextField = "Name";
            ddlImageProfile.DataBind();

        }

       

        protected void Page_Load(object sender, EventArgs e)
        {
            ddlGroupType.Enabled = false;
            if (!IsPostBack) PopulateForm();
        }

        protected void PopulateForm()
        {

            ddlGroupImage.DataSource = new BLL.Image().SearchImages("").Select(i => new { i.Id, i.Name });
            ddlGroupImage.DataValueField = "Id";
            ddlGroupImage.DataTextField = "Name";
            ddlGroupImage.DataBind();
            ddlGroupImage.Items.Insert(0, new ListItem("Select Image", "0"));

            txtGroupName.Text = Group.Name;
            txtGroupDesc.Text = Group.Description;
            ddlGroupImage.SelectedValue = Group.Image.ToString();
            txtGroupSenderArgs.Text = Group.SenderArguments;
            ddlGroupType.Text = Group.Type;

            ddlImageProfile.DataSource = _bllLinuxProfile.SearchProfiles(Convert.ToInt32(ddlGroupImage.SelectedValue)).Select(i => new { i.Id, i.Name });
            ddlImageProfile.DataValueField = "Id";
            ddlImageProfile.DataTextField = "Name";
            ddlImageProfile.DataBind();
            ddlImageProfile.SelectedValue = Group.ImageProfile.ToString();

        }

      

        
    }
}