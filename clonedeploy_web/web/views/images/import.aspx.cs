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
        protected void Page_Load(object sender, EventArgs e)
        {
            RequiresAuthorization(Authorizations.CreateImage);
            if (IsPostBack) return;

        }

        protected void ButtonImport_Click(object sender, EventArgs e)
        {
            var csvFilePath = Server.MapPath("~") + Path.DirectorySeparatorChar + "private" + Path.DirectorySeparatorChar +
                              "imports" + Path.DirectorySeparatorChar + "images.csv";
            FileUpload.SaveAs(csvFilePath);
            new FileOps().SetUnixPermissions(csvFilePath);
            var successCount = BLL.Image.ImportCsv(csvFilePath);
            EndUserMessage = "Successfully Imported " + successCount + " Images";

        }       
    }
}