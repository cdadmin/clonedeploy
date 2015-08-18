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

namespace views.hosts
{
    public partial class Addhosts : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Master.FindControl("SubNav").Visible = false;

            if (IsPostBack) return;

            if (new Authorize().IsInMembership("User"))
                Response.Redirect("~/views/dashboard/dash.aspx?access=denied");
            else
                PopulateForm();
        }

        protected void ButtonAddHost_Click(object sender, EventArgs e)
        {
            string scripts = null;
            foreach (ListItem item in lbScripts.Items)
            {
                if (item.Selected)
                    scripts += item.Value + ",";
            }

            var host = new Host
            {
                Name = txtHostName.Text,
                Mac = Utility.FixMac(txtHostMac.Text),
                Image = ddlHostImage.Text,
                Group = ddlHostGroup.Text,
                Description = txtHostDesc.Text,
                Kernel = ddlHostKernel.Text,
                BootImage = ddlHostBootImage.Text,
                Args = txtHostArguments.Text,
                Scripts = scripts
            };

            if (host.ValidateHostData())
            {
                if (host.Create() && !createAnother.Checked)
                    Response.Redirect("~/views/hosts/edit.aspx?hostid=" + host.Id);
            }

            Master.Msgbox(Utility.Message);
        }

        protected void PopulateForm()
        {
            ddlHostImage.DataSource = new Image().Search("").Select(i => i.Name);
            ddlHostImage.DataBind();
            ddlHostImage.Items.Insert(0, "Select Image");

            ddlHostGroup.DataSource = new Group().Search("").Select(g => g.Name);
            ddlHostGroup.DataBind();
            ddlHostGroup.Items.Insert(0, "");

            ddlHostKernel.DataSource = Utility.GetKernels();
            ddlHostKernel.DataBind();
            var itemHostKernel = ddlHostKernel.Items.FindByText(Settings.DefaultKernel32);
            if (itemHostKernel != null)
                ddlHostKernel.SelectedValue = Settings.DefaultKernel32;
            else
                ddlHostKernel.Items.Insert(0, "Select Kernel");

            ddlHostBootImage.DataSource = Utility.GetBootImages();
            ddlHostBootImage.DataBind();
            var itemBootImage = ddlHostBootImage.Items.FindByText("initrd.gz");
            if (itemBootImage != null)
                ddlHostBootImage.SelectedValue = "initrd.gz";
            else
                ddlHostBootImage.Items.Insert(0, "Select Boot Image");

            lbScripts.DataSource = Utility.GetScripts("custom");
            lbScripts.DataBind();
        }
    }
}