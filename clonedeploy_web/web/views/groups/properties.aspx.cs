using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Helpers;
using Models;

public partial class views_groups_properties : BasePages.Groups
{
    private GroupProperty _groupProperty;
    protected void Page_Load(object sender, EventArgs e)
    {
        _groupProperty = BLL.GroupProperty.GetGroupProperty(Group.Id);
        if (!IsPostBack) PopulateForm();
    }

   protected void PopulateForm()
    {
        PopulateImagesDdl(ddlHostImage);
        PopulateSitesDdl(ddlSite);
        PopulateBuildingsDdl(ddlBuilding);
        PopulateRoomsDdl(ddlRoom);

       chkDefault.Checked = Convert.ToBoolean(Group.SetDefaultProperties);
        if (_groupProperty != null)
        {
            ddlHostImage.SelectedValue = _groupProperty.ImageId.ToString();
            txtHostDesc.Text = _groupProperty.Description;
            PopulateImageProfilesDdl(ddlImageProfile, Convert.ToInt32(ddlHostImage.SelectedValue));
            ddlImageProfile.SelectedValue = _groupProperty.ImageProfileId.ToString();
            ddlSite.SelectedValue = _groupProperty.SiteId.ToString();
            ddlBuilding.SelectedValue = _groupProperty.BuildingId.ToString();
            ddlRoom.SelectedValue = _groupProperty.RoomId.ToString();
            txtCustom1.Text = _groupProperty.CustomAttribute1;
            txtCustom2.Text = _groupProperty.CustomAttribute2;
            txtCustom3.Text = _groupProperty.CustomAttribute3;
            txtCustom4.Text = _groupProperty.CustomAttribute4;
            txtCustom5.Text = _groupProperty.CustomAttribute5;
            chkImage.Checked = Convert.ToBoolean(_groupProperty.ImageEnabled);
            chkProfile.Checked = Convert.ToBoolean(_groupProperty.ImageProfileEnabled);
            chkDescription.Checked = Convert.ToBoolean(_groupProperty.DescriptionEnabled);
            chkSite.Checked = Convert.ToBoolean(_groupProperty.SiteEnabled);
            chkBuilding.Checked = Convert.ToBoolean(_groupProperty.BuildingEnabled);
            chkRoom.Checked = Convert.ToBoolean(_groupProperty.RoomEnabled);
            chkCustom1.Checked = Convert.ToBoolean(_groupProperty.CustomAttribute1Enabled);
            chkCustom2.Checked = Convert.ToBoolean(_groupProperty.CustomAttribute2Enabled);
            chkCustom3.Checked = Convert.ToBoolean(_groupProperty.CustomAttribute3Enabled);
            chkCustom4.Checked = Convert.ToBoolean(_groupProperty.CustomAttribute4Enabled);
            chkCustom5.Checked = Convert.ToBoolean(_groupProperty.CustomAttribute5Enabled);
        }
    }

    protected void ddlHostImage_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlHostImage.Text == "Select Image") return;
        PopulateImageProfilesDdl(ddlImageProfile, Convert.ToInt32(ddlHostImage.SelectedValue));
    }

    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorizationOrManagedGroup(Authorizations.UpdateGroup, Group.Id);

        var groupProperty = new GroupProperty
        {
            GroupId = Group.Id,
            ImageId = Convert.ToInt32(ddlHostImage.SelectedValue),
            ImageProfileId = Convert.ToInt32(ddlHostImage.SelectedValue) == -1 ? -1 : Convert.ToInt32(ddlImageProfile.SelectedValue),
            Description = txtHostDesc.Text,
            SiteId = Convert.ToInt32(ddlSite.SelectedValue),
            BuildingId = Convert.ToInt32(ddlBuilding.SelectedValue),
            RoomId = Convert.ToInt32(ddlRoom.SelectedValue),
            CustomAttribute1 = txtCustom1.Text,
            CustomAttribute2 = txtCustom2.Text,
            CustomAttribute3 = txtCustom3.Text,
            CustomAttribute4 = txtCustom4.Text,
            CustomAttribute5 = txtCustom5.Text,
            ImageEnabled = Convert.ToInt16(chkImage.Checked),
            ImageProfileEnabled = Convert.ToInt16(chkProfile.Checked),
            DescriptionEnabled = Convert.ToInt16(chkDescription.Checked),
            SiteEnabled = Convert.ToInt16(chkSite.Checked),
            BuildingEnabled = Convert.ToInt16(chkBuilding.Checked),
            RoomEnabled = Convert.ToInt16(chkRoom.Checked),
            CustomAttribute1Enabled = Convert.ToInt16(chkCustom1.Checked),
            CustomAttribute2Enabled = Convert.ToInt16(chkCustom2.Checked),
            CustomAttribute3Enabled = Convert.ToInt16(chkCustom3.Checked),
            CustomAttribute4Enabled = Convert.ToInt16(chkCustom4.Checked),
            CustomAttribute5Enabled = Convert.ToInt16(chkCustom5.Checked),
        };

        if (_groupProperty == null)
        {      
            BLL.GroupProperty.AddGroupProperty(groupProperty);
            EndUserMessage = "Successfully Updated Group Properties";
        }
        else
        {
            groupProperty.Id = _groupProperty.Id;
            BLL.GroupProperty.UpdateGroupProperty(groupProperty);
            EndUserMessage = "Successfully Updated Group Properties";
        }
    }

    protected void chkDefault_OnCheckedChanged(object sender, EventArgs e)
    {
        var group = Group;
        group.SetDefaultProperties = chkDefault.Checked ? 1 : 0;
        BLL.Group.UpdateGroup(group);
    }
}