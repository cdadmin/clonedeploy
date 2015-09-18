using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Global;
using Models;
using Image = Models.Image;

namespace views.hosts
{
    public partial class HostEdit : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) PopulateForm();
        }

        protected void buttonUpdateHost_Click(object sender, EventArgs e)
        {
            var host = new Computer
            {
                Id = Master.Computer.Id,
                Name = txtHostName.Text,
                Mac = Utility.FixMac(txtHostMac.Text),
                Image = Convert.ToInt32(ddlHostImage.SelectedValue),
                ImageProfile = Convert.ToInt32(ddlImageProfile.SelectedValue),
                Description = txtHostDesc.Text,
            };

           

            if (host.ValidateHostData()) host.Update();
            Master.Msgbox(Utility.Message);
        }

        protected void PopulateForm()
        {
            Master.Msgbox(Utility.Message);
            ddlHostImage.DataSource = new Image().Search("").Select(i => new { i.Id, i.Name });
            ddlHostImage.DataValueField = "Id";
            ddlHostImage.DataTextField = "Name";
            ddlHostImage.DataBind();
            ddlHostImage.Items.Insert(0, new ListItem("Select Image", "0"));

            txtHostName.Text = Master.Computer.Name;
            txtHostMac.Text = Master.Computer.Mac;
            ddlHostImage.SelectedValue = Master.Computer.Image.ToString();        
            txtHostDesc.Text = Master.Computer.Description;

            ddlImageProfile.DataSource = new LinuxEnvironmentProfile().Search(Convert.ToInt32(ddlHostImage.SelectedValue)).Select(i => new { i.Id, i.Name });
            ddlImageProfile.DataValueField = "Id";
            ddlImageProfile.DataTextField = "Name";
            ddlImageProfile.DataBind();

            ddlImageProfile.SelectedValue = Master.Computer.ImageProfile.ToString();
         
        }

        protected void ddlHostImage_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlHostImage.Text == "Select Image") return;
            ddlImageProfile.DataSource = new LinuxEnvironmentProfile().Search(Convert.ToInt32(ddlHostImage.SelectedValue)).Select(i => new { i.Id, i.Name });
            ddlImageProfile.DataValueField = "Id";
            ddlImageProfile.DataTextField = "Name";
            ddlImageProfile.DataBind();

        }
    }
}