using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BasePages;
using BLL;
using Global;
using Helpers;
using Models;
using Image = Models.Image;

namespace views.hosts
{
    public partial class HostEdit : BasePages.Computers
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) PopulateForm();
        }

        protected void buttonUpdateHost_Click(object sender, EventArgs e)
        {
            var host = new Models.Computer
            {
                Id = Computer.Id,
                Name = txtHostName.Text,
                Mac = Utility.FixMac(txtHostMac.Text),
                Image = Convert.ToInt32(ddlHostImage.SelectedValue),
                ImageProfile = Convert.ToInt32(ddlImageProfile.SelectedValue),
                Description = txtHostDesc.Text,
            };
          
            if (BllComputer.ValidateHostData(host)) BllComputer.UpdateComputer(host);
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