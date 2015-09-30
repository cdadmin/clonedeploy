using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BasePages;
using BLL;
using Global;
using Helpers;
using Models;
using Security;
using Image = Models.Image;

namespace views.hosts
{
    public partial class Addhosts : BasePages.Computers
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            if (new Authorize().IsInMembership("User"))
                Response.Redirect("~/views/dashboard/dash.aspx?access=denied");
            else
                PopulateForm();
        }

        protected void ButtonAddHost_Click(object sender, EventArgs e)
        {
      
            var host = new Models.Computer
            {
                Name = txtHostName.Text,
                Mac = Utility.FixMac(txtHostMac.Text),
                Image = Convert.ToInt32(ddlHostImage.SelectedValue),
                Description = txtHostDesc.Text,
            };

            host.ImageProfile = host.Image == 0 ? 0 : Convert.ToInt32(ddlImageProfile.SelectedValue);

            if (!BllComputer.ValidateHostData(host)) return;

            if (BllComputer.AddComputer(host) && !createAnother.Checked)
                Response.Redirect("~/views/computers/edit.aspx?hostid=" + host.Id);
        }

        protected void PopulateForm()
        {
            PopulateImagesDdl(ddlHostImage);
        }

        protected void ddlHostImage_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateImageProfilesDdl(ddlImageProfile, Convert.ToInt32(ddlHostImage.SelectedValue));
        }
    }
}