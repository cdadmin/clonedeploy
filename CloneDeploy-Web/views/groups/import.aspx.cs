using System;
using System.IO;
using BasePages;
using CloneDeploy_Web;

namespace views.groups
{
    public partial class GroupImport : Groups
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RequiresAuthorization(Authorizations.CreateGroup);
            if (IsPostBack) return;

        }

        protected void ButtonImport_Click(object sender, EventArgs e)
        {
            var csvFilePath = Server.MapPath("~") + Path.DirectorySeparatorChar + "private" + Path.DirectorySeparatorChar +
                              "imports" + Path.DirectorySeparatorChar + "groups.csv";
            FileUpload.SaveAs(csvFilePath);
            Call.FilesystemApi.SetUnixPermissions(csvFilePath);
            //var successCount = BLL.Group.ImportCsv(csvFilePath,CloneDeployCurrentUser.Id);
            //EndUserMessage = "Successfully Imported " + successCount + " Groups";

        }       
    }
}