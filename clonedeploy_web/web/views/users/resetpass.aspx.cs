﻿/*  
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
using System.Web;
using System.Web.UI;
using Global;
using Models;
using Security;

namespace views.users
{
    public partial class ResetPass : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Master.FindControl("SubNav").Visible = false;
            if (IsPostBack) return;
            if (new Authorize().IsInMembership("Administrator")) return;
            var wdsuser = new WdsUser { Name = HttpContext.Current.User.Identity.Name };
            wdsuser.Read();
            if (wdsuser.Id != Request.QueryString["userid"])
                Response.Redirect("~/views/dashboard/dash.aspx?access=denied");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var user = new WdsUser {Id = Request.QueryString["userid"]};
            user.Read();

            if (txtUserPwd.Text == txtUserPwdConfirm.Text)
            {
                user.Password = txtUserPwd.Text;
                user.Salt = new WdsUser().CreateSalt(16);
                if (user.ValidateUserData()) user.Update(true);
            }
            else
                Utility.Message = "Passwords Did Not Match";

            Master.Master.Msgbox(Utility.Message);
        }
    }
}