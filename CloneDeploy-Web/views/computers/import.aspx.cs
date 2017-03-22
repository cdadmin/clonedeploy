using System;
using System.IO;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Web;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;


namespace views.computers
{
    public partial class ComputerImport : Computers
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           RequiresAuthorization(Authorizations.CreateComputer);
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

                var count = Call.ComputerApi.Import(new ApiStringResponseDTO(){Value = csvContent});
                Call.GroupApi.ReCalcSmart();
                EndUserMessage = "Successfully Imported " + count + " Computers";
            }         

        }

    }
}