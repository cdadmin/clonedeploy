using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Configuration;
using BasePages;
using BLL;
using CsvHelper;
using CsvHelper.Configuration;
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

        protected void btnExportSql_Click(object sender, EventArgs e)
        {
            string constring = ConfigurationManager.ConnectionStrings["clonedeploy"].ConnectionString;
            string exportPath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "data" +
                         Path.DirectorySeparatorChar + "exports" + Path.DirectorySeparatorChar;

            using (MySqlConnection conn = new MySqlConnection(constring))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        cmd.Connection = conn;
                        conn.Open();
                        mb.ExportToFile(exportPath + "dbDump.sql");
                        conn.Close();
                    }
                }
            }

            EndUserMessage = "Export Complete";


        }

        
        protected void btnExportCsv_Click(object sender, EventArgs e)
        {
            string exportPath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "data" +
                         Path.DirectorySeparatorChar + "exports" + Path.DirectorySeparatorChar;

           BLL.Computer.ExportCsv(exportPath + "computers.csv");
           BLL.Group.ExportCsv(exportPath + "groups.csv");
           BLL.Image.ExportCsv(exportPath + "images.csv");
           EndUserMessage = "Export Complete";
        }
    }
}