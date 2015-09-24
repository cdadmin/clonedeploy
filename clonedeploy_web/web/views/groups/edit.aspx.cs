/*  
    CrucibleWDS A Windows Deployment Solution
    Copyright (C) 2011  Jon Dolny

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/.
 */

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
    public partial class GroupEdit : Page
    {
        private readonly BLL.LinuxProfile _bllLinuxProfile = new BLL.LinuxProfile();

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var group = new Models.Group
            {
                Id = Master.Group.Id,
                Name = txtGroupName.Text,
                Type = Master.Group.Type,
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

            txtGroupName.Text = Master.Group.Name;
            txtGroupDesc.Text = Master.Group.Description;
            ddlGroupImage.SelectedValue = Master.Group.Image.ToString();
            txtGroupSenderArgs.Text = Master.Group.SenderArguments;
            ddlGroupType.Text = Master.Group.Type;

            ddlImageProfile.DataSource = _bllLinuxProfile.SearchProfiles(Convert.ToInt32(ddlGroupImage.SelectedValue)).Select(i => new { i.Id, i.Name });
            ddlImageProfile.DataValueField = "Id";
            ddlImageProfile.DataTextField = "Name";
            ddlImageProfile.DataBind();
            ddlImageProfile.SelectedValue = Master.Group.ImageProfile.ToString();

        }

      

        
    }
}