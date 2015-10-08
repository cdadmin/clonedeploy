using System;
using BasePages;
using Helpers;
using Models;

namespace views.hosts
{
    public partial class HostEdit : Computers
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) PopulateForm();
        }

        protected void buttonUpdateHost_Click(object sender, EventArgs e)
        {
            var host = new Computer
            {
                Id = Computer.Id,
                Name = txtHostName.Text,
                Mac = Utility.FixMac(txtHostMac.Text),
                Image = Convert.ToInt32(ddlHostImage.SelectedValue),
                ImageProfile = ddlImageProfile.SelectedValue == "" ? -1 : Convert.ToInt32(ddlImageProfile.SelectedValue),
                Description = txtHostDesc.Text,
            };

            var result = BllComputer.UpdateComputer(host);
            EndUserMessage = !result.IsValid ? result.Message : "Successfully Updated Computer";
        }

        protected void PopulateForm()
        {
            PopulateImagesDdl(ddlHostImage);
            txtHostName.Text = Computer.Name;
            txtHostMac.Text = Computer.Mac;
            ddlHostImage.SelectedValue = Computer.Image.ToString();        
            txtHostDesc.Text = Computer.Description;
            PopulateImageProfilesDdl(ddlImageProfile, Convert.ToInt32(ddlHostImage.SelectedValue));
            ddlImageProfile.SelectedValue = Computer.ImageProfile.ToString();       
        }

        protected void ddlHostImage_OnSelectedIndexChanged(object sender, EventArgs e)
        {           
            if (ddlHostImage.Text == "Select Image") return;
            PopulateImageProfilesDdl(ddlImageProfile, Convert.ToInt32(ddlHostImage.SelectedValue));
        }
    }
}