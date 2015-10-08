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
using System.Web;
using BLL;
using Helpers;
using Tasks;

namespace views.tasks
{
    public partial class TaskCustom : BasePages.Tasks
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var user = new User().GetUser(HttpContext.Current.User.Identity.Name);

            if (Settings.OnDemand == "Disabled")
            {
                secure.Visible = false;
                secureMsg.Text = "On Demand Mode Has Been Globally Disabled";
                secureMsg.Visible = true;
            }
            else if (user.OndAccess == "0")
            {
                secure.Visible = false;
                secureMsg.Text = "On Demand Mode Has Been Disabled For This Account";
                secureMsg.Visible = true;
            }
            else
            {
                secure.Visible = true;
                secureMsg.Visible = false;
            }
            if (IsPostBack) return;
            ddlImage.DataSource = new Image().SearchImages("").Select(i => i.Name);
            ddlImage.DataBind();
            ddlImage.Items.Insert(0, "Select Image");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (ddlImage.Text != "Select Image")
            {
                var multicast = new Multicast
                {
                    ActiveMcTask = {Image = ddlImage.Text},
                    IsCustom = true
                };
                multicast.StartMulticastSender();

            }
            else
                EndUserMessage = "Select An Image";
        }
    }
}