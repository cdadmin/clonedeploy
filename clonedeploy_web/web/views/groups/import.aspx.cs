using System;
using System.IO;
using BasePages;
using BLL;
using Helpers;
using Security;

namespace views.groups
{
    public partial class GroupImport : Groups
    {
        protected void btnImport_Click(object sender, EventArgs e)
        {
            var csvFilePath = Server.MapPath("~") + Path.DirectorySeparatorChar + "data" + Path.DirectorySeparatorChar +
                              "csvupload" + Path.DirectorySeparatorChar + "groups.csv";
            FileUpload.SaveAs(csvFilePath);
            new FileOps().SetUnixPermissions(csvFilePath);
            new Group().ImportGroups();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (IsPostBack) return;
            if (new Authorize().IsInMembership("User"))
                Response.Redirect("~/views/dashboard/dash.aspx?access=denied");
        }
    }
}