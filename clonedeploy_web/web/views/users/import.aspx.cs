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
using System.IO;
using BasePages;
using Helpers;
using Security;

namespace views.users
{
    public partial class ImportUser : Users
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            if (!new Authorize().IsInMembership("Administrator"))
                Response.Redirect("~/views/dashboard/dash.aspx?access=denied");
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            var csvFilePath = Server.MapPath("~") + Path.DirectorySeparatorChar + "data" + Path.DirectorySeparatorChar +
                              "csvupload" + Path.DirectorySeparatorChar + "users.csv";
            FileUpload.SaveAs(csvFilePath);
            new FileOps().SetUnixPermissions(csvFilePath);
            BLL.User.ImportUsers();
        }        
    }
}