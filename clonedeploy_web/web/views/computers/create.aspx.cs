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
using BasePages;
using BLL;
using Global;
using Models;
using Security;
using Image = Models.Image;

namespace views.hosts
{
    public partial class Addhosts : BasePages.Computers
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            if (new Authorize().IsInMembership("User"))
                Response.Redirect("~/views/dashboard/dash.aspx?access=denied");
            else
                PopulateForm();
        }

        protected void ButtonAddHost_Click(object sender, EventArgs e)
        {
      
            var host = new Models.Computer
            {
                Name = txtHostName.Text,
                Mac = Utility.FixMac(txtHostMac.Text),
                Image = Convert.ToInt32(ddlHostImage.SelectedValue),
                Description = txtHostDesc.Text,
            };

            host.ImageProfile = host.Image == 0 ? 0 : Convert.ToInt32(ddlImageProfile.SelectedValue);
            var bllComputer = new BLL.Computer();
            if (bllComputer.ValidateHostData(host))
            {
                if (new  BLL.Computer().AddComputer(host) && !createAnother.Checked)
                    Response.Redirect("~/views/computers/edit.aspx?hostid=" + host.Id);
            }

           
        }

        protected void PopulateForm()
        {
            

     
        }

        protected void ddlHostImage_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateImageProfilesDdl(ddlImageProfile, Convert.ToInt32(ddlHostImage.SelectedValue));

        }
    }
}