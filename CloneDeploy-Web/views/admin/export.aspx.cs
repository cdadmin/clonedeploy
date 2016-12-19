using System;
using System.Web;
using CloneDeploy_Web;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

namespace views.admin
{
    public partial class AdminExport : Admin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RequiresAuthorization(Authorizations.Administrator);
        }
  
        protected void btnExportCsv_Click(object sender, EventArgs e)
        {
            string exportPath = Call.FilesystemApi.GetServerPaths("exports", "");
         
           Call.ComputerApi.Export(exportPath + "computers.csv");
           Call.GroupApi.Export(exportPath + "groups.csv");
           Call.ImageApi.Export(exportPath + "images.csv");
           EndUserMessage = "Export Complete";
        }
    }
}