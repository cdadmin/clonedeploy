using System;
using BasePages;
using Helpers;
using Models;
using Security;

namespace views.hosts
{
    public partial class Addhosts : Computers
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
      
            var host = new Computer
            {
                Name = txtHostName.Text,
                Mac = Utility.FixMac(txtHostMac.Text),
                Image = Convert.ToInt32(ddlHostImage.SelectedValue),
                ImageProfile = Convert.ToInt32(ddlHostImage.SelectedValue) == 0 ? 0 : Convert.ToInt32(ddlImageProfile.SelectedValue),
                Description = txtHostDesc.Text,
            };


            var result = BllComputer.AddComputer(host);
            if (!result.IsValid)
                EndUserMessage = result.Message;
            else
            {
                EndUserMessage = "Successfully Created Computer";
                if(!createAnother.Checked)
                     Response.Redirect("~/views/computers/edit.aspx?hostid=" + host.Id);
            }
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