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
            RequiresAuthorization(Authorizations.CreateComputer);
            if (!IsPostBack) PopulateForm();
        }

        protected void ButtonAddHost_Click(object sender, EventArgs e)
        {
      
            var host = new Computer
            {
                Name = txtHostName.Text,
                Mac = txtHostMac.Text,
                ImageId = Convert.ToInt32(ddlHostImage.SelectedValue),
                ImageProfile = Convert.ToInt32(ddlHostImage.SelectedValue) == -1 ? -1 : Convert.ToInt32(ddlImageProfile.SelectedValue),
                Description = txtHostDesc.Text,
                SiteId = Convert.ToInt32(ddlSite.SelectedValue),
                BuildingId = Convert.ToInt32(ddlBuilding.SelectedValue),
                RoomId = Convert.ToInt32(ddlRoom.SelectedValue)
            };


            var result = BLL.Computer.AddComputer(host);
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
            PopulateSitesDdl(ddlSite);
            PopulateBuildingsDdl(ddlBuilding);
            PopulateRoomsDdl(ddlRoom);
        }

        protected void ddlHostImage_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateImageProfilesDdl(ddlImageProfile, Convert.ToInt32(ddlHostImage.SelectedValue));
        }
    }
}