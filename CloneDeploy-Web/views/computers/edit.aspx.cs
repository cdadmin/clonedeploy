using System;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.computers
{
    public partial class ComputerEdit : Computers
    {
        protected void buttonUpdateComputer_Click(object sender, EventArgs e)
        {
            RequiresAuthorizationOrManagedComputer(AuthorizationStrings.UpdateComputer, Computer.Id);
            var nameChange = txtComputerName.Text != Computer.Name;

            Computer.Name = txtComputerName.Text;
            Computer.Mac = txtComputerMac.Text;
            Computer.ImageId = Convert.ToInt32(ddlComputerImage.SelectedValue);
            Computer.ImageProfileId =
                Convert.ToInt32(ddlComputerImage.SelectedValue) == -1
                    ? -1
                    : Convert.ToInt32(ddlImageProfile.SelectedValue);
            Computer.Description = txtComputerDesc.Text;
            Computer.SiteId = Convert.ToInt32(ddlSite.SelectedValue);
            Computer.BuildingId = Convert.ToInt32(ddlBuilding.SelectedValue);
            Computer.RoomId = Convert.ToInt32(ddlRoom.SelectedValue);
            Computer.ClusterGroupId = Convert.ToInt32(ddlClusterGroup.SelectedValue);
            Computer.CustomAttribute1 = txtCustom1.Text;
            Computer.CustomAttribute2 = txtCustom2.Text;
            Computer.CustomAttribute3 = txtCustom3.Text;
            Computer.CustomAttribute4 = txtCustom4.Text;
            Computer.CustomAttribute5 = txtCustom5.Text;

            

            var result = Call.ComputerApi.Put(Computer.Id, Computer);

            if (result.Success)
            {
                //Don't move this to the ComputerServices.  It's called here for a reason.
                if (nameChange)
                {
                    Call.GroupApi.ReCalcSmart();
                    Computer = Call.ComputerApi.Get(Computer.Id);
                    PopulateForm();
                }
                EndUserMessage = "Successfully Updated Computer";
            }
            else
            {
                EndUserMessage = result.ErrorMessage;
            }
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) PopulateForm();
        }

        protected void PopulateForm()
        {
            PopulateImagesDdl(ddlComputerImage);
            PopulateSitesDdl(ddlSite);
            PopulateBuildingsDdl(ddlBuilding);
            PopulateRoomsDdl(ddlRoom);
            PopulateClusterGroupsDdl(ddlClusterGroup);
            txtComputerName.Text = Computer.Name;
            txtComputerMac.Text = Computer.Mac;
            ddlComputerImage.SelectedValue = Computer.ImageId.ToString();
            txtComputerDesc.Text = Computer.Description;
            PopulateImageProfilesDdl(ddlImageProfile, Convert.ToInt32(ddlComputerImage.SelectedValue));
            ddlImageProfile.SelectedValue = Computer.ImageProfileId.ToString();
            ddlSite.SelectedValue = Computer.SiteId.ToString();
            ddlBuilding.SelectedValue = Computer.BuildingId.ToString();
            ddlRoom.SelectedValue = Computer.RoomId.ToString();
            ddlClusterGroup.SelectedValue = Computer.ClusterGroupId.ToString();
            txtCustom1.Text = Computer.CustomAttribute1;
            txtCustom2.Text = Computer.CustomAttribute2;
            txtCustom3.Text = Computer.CustomAttribute3;
            txtCustom4.Text = Computer.CustomAttribute4;
            txtCustom5.Text = Computer.CustomAttribute5;
        }
    }
}