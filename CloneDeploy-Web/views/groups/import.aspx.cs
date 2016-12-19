using System;
using CloneDeploy_Web;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

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
            var csvFilePath = Call.FilesystemApi.GetServerPaths("csv", "groups.csv");
            
            FileUpload.SaveAs(csvFilePath);
            Call.FilesystemApi.SetUnixPermissions(csvFilePath);
            //var successCount = BLL.Group.ImportCsv(csvFilePath,CloneDeployCurrentUser.Id);
            //EndUserMessage = "Successfully Imported " + successCount + " Groups";

        }       
    }
}