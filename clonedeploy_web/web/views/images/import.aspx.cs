using System;
using System.IO;
using BasePages;
using BLL;
using Helpers;
using Security;

namespace views.images
{
    public partial class ImageImport : Images
    {
        protected void btnImport_Click(object sender, EventArgs e)
        {
            var csvFilePath = Server.MapPath("~") + Path.DirectorySeparatorChar + "data" + Path.DirectorySeparatorChar +
                              "csvupload" + Path.DirectorySeparatorChar + "images.csv";
            FileUpload.SaveAs(csvFilePath);
            new FileOps().SetUnixPermissions(csvFilePath);
            BLL.Image.Import();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
          
        }
    }
}