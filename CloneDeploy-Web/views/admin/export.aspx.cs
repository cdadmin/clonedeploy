using System;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

namespace views.admin
{
    public partial class AdminExport : Admin
    {
        protected void btnExportCsv_Click(object sender, EventArgs e)
        {
            var exportPath = Call.FilesystemApi.GetServerPaths("exports", "");

            Call.ComputerApi.Export(exportPath + "computers.csv");
            Call.GroupApi.Export(exportPath + "groups.csv");
            Call.ImageApi.Export(exportPath + "images.csv");
            EndUserMessage = "Export Complete";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RequiresAuthorization(Authorizations.Administrator);
        }
    }
}