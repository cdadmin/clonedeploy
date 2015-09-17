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
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Global;
using Models;
using Security;
using Image = Models.Image;

namespace views.groups
{
    public partial class GroupCreate : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          
            if (IsPostBack) return;
            if (new Authorize().IsInMembership("User"))
                Response.Redirect("~/views/dashboard/dash.aspx?access=denied");
            PopulateForm();
        }

        protected void PopulateForm()
        {
            ddlGroupImage.DataSource = new Image().Search("").Select(i => new { i.Id, i.Name });
            ddlGroupImage.DataValueField = "Id";
            ddlGroupImage.DataTextField = "Name";
            ddlGroupImage.DataBind();
            ddlGroupImage.Items.Insert(0, new ListItem("Select Image", "0"));

        
         
        }

     

    

        protected void Submit_Click(object sender, EventArgs e)
        {
            var group = new Group();

        
            group.Name = txtGroupName.Text;
            group.Description = txtGroupDesc.Text;
            group.Image = Convert.ToInt32(ddlGroupImage.SelectedValue);
            group.SenderArguments = txtGroupSenderArgs.Text;
            group.Type = ddlGroupType.Text;
            group.ImageProfile = group.Image == 0 ? 0 : Convert.ToInt32(ddlImageProfile.SelectedValue);
          

            if (group.ValidateGroupData()) group.Create();

            if (Utility.Message.Contains("Successfully"))
                Response.Redirect("~/views/groups/edit.aspx?groupid=" + group.Id);

            Master.Master.Msgbox(Utility.Message);
        }


        protected void ddlGroupImage_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlGroupImage.Text == "Select Image") return;
            ddlImageProfile.DataSource = new LinuxEnvironmentProfile().Search(Convert.ToInt32(ddlGroupImage.SelectedValue)).Select(i => new { i.Id, i.Name });
            ddlImageProfile.DataValueField = "Id";
            ddlImageProfile.DataTextField = "Name";
            ddlImageProfile.DataBind();

        }
    }
}