using System;
using System.IO;
using CloneDeploy_Entities.DTOs;
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
            if (FileUpload.HasFile)
            {
                string csvContent;
                using (StreamReader inputStreamReader = new StreamReader(FileUpload.PostedFile.InputStream))
                {
                    csvContent = inputStreamReader.ReadToEnd();
                }

                var count = Call.GroupApi.Import(new ApiStringResponseDTO() { Value = csvContent });
                EndUserMessage = "Successfully Imported " + count + " Groups";
            }         
        }       
    }
}