using System;
using System.IO;
using CloneDeploy_Common;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.groups
{
    public partial class GroupImport : Groups
    {
        protected void ButtonImport_Click(object sender, EventArgs e)
        {
            if (FileUpload.HasFile)
            {
                string csvContent;
                using (var inputStreamReader = new StreamReader(FileUpload.PostedFile.InputStream))
                {
                    csvContent = inputStreamReader.ReadToEnd();
                }

                var count = Call.GroupApi.Import(new ApiStringResponseDTO {Value = csvContent});
                EndUserMessage = "Successfully Imported " + count + " Groups";
                Call.GroupApi.ReCalcSmart();
                
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RequiresAuthorization(AuthorizationStrings.CreateGroup);
            if (IsPostBack) return;
        }
    }
}