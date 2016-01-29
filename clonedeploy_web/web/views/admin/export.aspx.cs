using System;
using System.Configuration;
using System.IO;
using System.Web;
using BasePages;
using BLL;
using CsvHelper;
using MySql.Data.MySqlClient;

namespace views.admin
{
    public partial class AdminExport : Admin
    {
        protected void btnExport_Click(object sender, EventArgs e)
        {
            string constring = ConfigurationManager.ConnectionStrings["clonedeploy"].ConnectionString;
            string file = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "data" +
                          Path.DirectorySeparatorChar + "logs" + Path.DirectorySeparatorChar + "export.csv";

            using (var csv = new CsvWriter(new StreamWriter(file)))
            {
                
                csv.WriteRecords(BLL.Computer.GetAll());
            }
            /*MemoryStream ms = new MemoryStream();
            using (MySqlConnection conn = new MySqlConnection(constring))
            {
                MySqlCommand cmd = new MySqlCommand();
                MySqlBackup mb = new MySqlBackup(cmd);
                cmd.Connection = conn;
                conn.Open();
                mb.ExportToMemoryStream(ms);
            }
            Response.ContentType = "text/plain";
            Response.AppendHeader("Content-Disposition", "attachment; filename=backup.sql");
            Response.BinaryWrite(ms.ToArray());
            Response.End();*/
            
          
        }
    }
}