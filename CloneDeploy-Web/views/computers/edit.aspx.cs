using System;
using BasePages;
using CloneDeploy_ApiCalls;
using CloneDeploy_Entities;
using CloneDeploy_Web;

namespace views.computers
{
    public partial class ComputerEdit : Computers
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) PopulateForm();
        }

        protected void buttonUpdateComputer_Click(object sender, EventArgs e)
        {
            RequiresAuthorizationOrManagedComputer(Authorizations.UpdateComputer, Computer.Id);
            var computer = new ComputerEntity
            {
                Id = Computer.Id,
                Name = txtComputerName.Text,
                Mac = txtComputerMac.Text,
                ImageId = Convert.ToInt32(ddlComputerImage.SelectedValue),
                ImageProfileId = Convert.ToInt32(ddlComputerImage.SelectedValue) == -1 ? -1 : Convert.ToInt32(ddlImageProfile.SelectedValue),
                Description = txtComputerDesc.Text,
                SiteId = Convert.ToInt32(ddlSite.SelectedValue),
                BuildingId = Convert.ToInt32(ddlBuilding.SelectedValue),
                RoomId = Convert.ToInt32(ddlRoom.SelectedValue),
                CustomAttribute1 = txtCustom1.Text,
                CustomAttribute2 = txtCustom2.Text,
                CustomAttribute3 = txtCustom3.Text,
                CustomAttribute4 = txtCustom4.Text,
                CustomAttribute5 = txtCustom5.Text,
                ProxyReservation = Computer.ProxyReservation
            };

            var result = new APICall().ComputerApi.Put(computer.Id, computer); 
            EndUserMessage = !result.Success ? result.ErrorMessage : "Successfully Updated Computer";
        }

        protected void PopulateForm()
        {
            PopulateImagesDdl(ddlComputerImage);
            PopulateSitesDdl(ddlSite);
            PopulateBuildingsDdl(ddlBuilding);
            PopulateRoomsDdl(ddlRoom);
            txtComputerName.Text = Computer.Name;
            txtComputerMac.Text = Computer.Mac;
            ddlComputerImage.SelectedValue = Computer.ImageId.ToString();        
            txtComputerDesc.Text = Computer.Description;
            PopulateImageProfilesDdl(ddlImageProfile, Convert.ToInt32(ddlComputerImage.SelectedValue));
            ddlImageProfile.SelectedValue = Computer.ImageProfileId.ToString();
            ddlSite.SelectedValue = Computer.SiteId.ToString();
            ddlBuilding.SelectedValue = Computer.BuildingId.ToString();
            ddlRoom.SelectedValue = Computer.RoomId.ToString();
            txtCustom1.Text = Computer.CustomAttribute1;
            txtCustom2.Text = Computer.CustomAttribute2;
            txtCustom3.Text = Computer.CustomAttribute3;
            txtCustom4.Text = Computer.CustomAttribute4;
            txtCustom5.Text = Computer.CustomAttribute5;
        }

        protected void ddlComputerImage_OnSelectedIndexChanged(object sender, EventArgs e)
        {           
            if (ddlComputerImage.Text == "Select Image") return;
            PopulateImageProfilesDdl(ddlImageProfile, Convert.ToInt32(ddlComputerImage.SelectedValue));
            try
            {
                ddlImageProfile.SelectedIndex = 1;
            }
            catch 
            {
                //ignore
            }
           
        }
    }
}