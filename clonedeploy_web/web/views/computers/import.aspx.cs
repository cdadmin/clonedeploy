using System;
using System.IO;
using BasePages;
using BLL;
using Helpers;
using Security;

namespace views.hosts
{
    public partial class HostImport : Computers
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           RequiresAuthorization(Authorizations.CreateComputer);
            if (IsPostBack) return;
          
        }

        protected void ButtonImport_Click(object sender, EventArgs e)
        {
            var csvFilePath = Server.MapPath("~") + Path.DirectorySeparatorChar + "data" + Path.DirectorySeparatorChar +
                              "csvupload" + Path.DirectorySeparatorChar + "hosts.csv";
            FileUpload.SaveAs(csvFilePath);
            new FileOps().SetUnixPermissions(csvFilePath);
            BLL.Computer.ImportComputers();
           
        }       
    }
}