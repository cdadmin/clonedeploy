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
            RequiresAuthorizationOrManagedGroup(Authorizations.ReadComputer,Computer.Id);
            if (!IsPostBack) PopulateForm();
        }

        protected void buttonUpdateHost_Click(object sender, EventArgs e)
        {
            var host = new Computer
            {
                Id = Computer.Id,
                Name = txtHostName.Text,
                Mac = txtHostMac.Text,
                ImageId = Convert.ToInt32(ddlHostImage.SelectedValue),
                ImageProfile = Convert.ToInt32(ddlHostImage.SelectedValue) == -1 ? -1 : Convert.ToInt32(ddlImageProfile.SelectedValue),
                Description = txtHostDesc.Text,
                SiteId = Convert.ToInt32(ddlSite.SelectedValue),
                BuildingId = Convert.ToInt32(ddlBuilding.SelectedValue),
                RoomId = Convert.ToInt32(ddlRoom.SelectedValue)
            };

            var result = BLL.Computer.UpdateComputer(host);
            EndUserMessage = !result.IsValid ? result.Message : "Successfully Updated Computer";
        }

        protected void PopulateForm()
        {
            PopulateImagesDdl(ddlHostImage);
            PopulateSitesDdl(ddlSite);
            PopulateBuildingsDdl(ddlBuilding);
            PopulateRoomsDdl(ddlRoom);
            txtHostName.Text = Computer.Name;
            txtHostMac.Text = Computer.Mac;
            ddlHostImage.SelectedValue = Computer.ImageId.ToString();        
            txtHostDesc.Text = Computer.Description;
            PopulateImageProfilesDdl(ddlImageProfile, Convert.ToInt32(ddlHostImage.SelectedValue));
            ddlImageProfile.SelectedValue = Computer.ImageProfile.ToString();
            ddlSite.SelectedValue = Computer.SiteId.ToString();
            ddlBuilding.SelectedValue = Computer.BuildingId.ToString();
            ddlRoom.SelectedValue = Computer.RoomId.ToString();
        }

        protected void ddlHostImage_OnSelectedIndexChanged(object sender, EventArgs e)
        {           
            if (ddlHostImage.Text == "Select Image") return;
            PopulateImageProfilesDdl(ddlImageProfile, Convert.ToInt32(ddlHostImage.SelectedValue));
        }
    }
}