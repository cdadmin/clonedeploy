using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BasePages;
using BLL;
using Global;
using Models;
using Image = Models.Image;

namespace views.hosts
{
    public partial class HostEdit : BasePages.Computers
    {
        private readonly BLL.LinuxProfile _bllLinuxProfile = new BLL.LinuxProfile();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) PopulateForm();
        }

        protected void buttonUpdateHost_Click(object sender, EventArgs e)
        {
            var host = new Models.Computer
            {
                Id = Master.Computer.Id,
                Name = txtHostName.Text,
                Mac = Utility.FixMac(txtHostMac.Text),
                Image = Convert.ToInt32(ddlHostImage.SelectedValue),
                ImageProfile = Convert.ToInt32(ddlImageProfile.SelectedValue),
                Description = txtHostDesc.Text,
            };

           
            if (BllComputer.ValidateHostData(host)) BllComputer.UpdateComputer(host);
            Master.Msgbox(Utility.Message);
        }

        protected void PopulateForm()
        {
            Master.Msgbox(Utility.Message);
            ddlHostImage.DataSource = new BLL.Image().SearchImages("").Select(i => new { i.Id, i.Name });
            ddlHostImage.DataValueField = "Id";
            ddlHostImage.DataTextField = "Name";
            ddlHostImage.DataBind();
            ddlHostImage.Items.Insert(0, new ListItem("Select Image", "0"));

            txtHostName.Text = Computer.Name;
            txtHostMac.Text = Computer.Mac;
            ddlHostImage.SelectedValue = Computer.Image.ToString();        
            txtHostDesc.Text = Computer.Description;

            ddlImageProfile.DataSource = _bllLinuxProfile.SearchProfiles(Convert.ToInt32(ddlHostImage.SelectedValue)).Select(i => new { i.Id, i.Name });
            ddlImageProfile.DataValueField = "Id";
            ddlImageProfile.DataTextField = "Name";
            ddlImageProfile.DataBind();

            ddlImageProfile.SelectedValue = Computer.ImageProfile.ToString();
         
        }

        protected void ddlHostImage_OnSelectedIndexChanged(object sender, EventArgs e)
        {
           
            if (ddlHostImage.Text == "Select Image") return;
            Test(ddlImageProfile, Convert.ToInt32(ddlHostImage.SelectedValue));

        }
    }
}