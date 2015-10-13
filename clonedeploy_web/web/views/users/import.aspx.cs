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
            RequiresAuthorization(Authorizations.Administrator);

            if (IsPostBack) return;
         
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