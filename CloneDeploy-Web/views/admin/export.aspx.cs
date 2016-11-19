using System;
using System.Configuration;
using System.IO;
using System.Web;
using BasePages;
using Helpers;
using MySql.Data.MySqlClient;

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
            string exportPath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "private" +
                         Path.DirectorySeparatorChar + "exports" + Path.DirectorySeparatorChar;

           BLL.Computer.ExportCsv(exportPath + "computers.csv");
           BLL.Group.ExportCsv(exportPath + "groups.csv");
           BLL.Image.ExportCsv(exportPath + "images.csv");
           EndUserMessage = "Export Complete";
        }
    }
}