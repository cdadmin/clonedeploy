using System;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.groups
{
    public partial class views_groups_properties : Groups
    {
        private GroupPropertyEntity _groupProperty;

        protected void btnSubmit_OnClick(object sender, EventArgs e)
        {
            RequiresAuthorizationOrManagedGroup(AuthorizationStrings.UpdateGroup, Group.Id);

            var groupProperty = new GroupPropertyEntity
            {
                GroupId = Group.Id,
                ImageId = Convert.ToInt32(ddlComputerImage.SelectedValue),
                ImageProfileId =
                    Convert.ToInt32(ddlComputerImage.SelectedValue) == -1
                        ? -1
                        : Convert.ToInt32(ddlImageProfile.SelectedValue),
                Description = txtComputerDesc.Text,
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
                ProxyEnabledEnabled = Convert.ToInt16(chkProxyReservation.Checked),
                BootFileEnabled = Convert.ToInt16(chkBootFile.Checked),
                TftpServerEnabled = Convert.ToInt16(chkTftp.Checked),
                ProxyEnabled = Convert.ToInt16(chkProxyEnabled.Checked),
                TftpServer = txtTftp.Text,
                BootFile = ddlBootFile.Text,
                ClusterGroupId = Convert.ToInt32(ddlClusterGroup.SelectedValue),
                ClusterGroupEnabled = Convert.ToInt16(chkClusterGroup.Checked),
                AlternateServerIpEnabled = Convert.ToInt16(chkAltServer.Checked),
                AlternateServerIpId = Convert.ToInt32(ddlAltServer.SelectedValue),
                ImageClassificationsEnabled = Convert.ToInt16(chkImageClass.Checked)
            };

            if (_groupProperty == null)
            {
                Call.GroupPropertyApi.Post(groupProperty);
                EndUserMessage = "Successfully Updated Group Properties";
            }
            else
            {
                groupProperty.Id = _groupProperty.Id;
                Call.GroupPropertyApi.Put(groupProperty.Id, groupProperty);
                EndUserMessage = "Successfully Updated Group Properties";
            }
        }

        protected void chkDefault_OnCheckedChanged(object sender, EventArgs e)
        {
            var group = Group;
            group.SetDefaultProperties = chkDefault.Checked ? 1 : 0;
            Call.GroupApi.Put(group.Id, group);
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
            _groupProperty = Call.GroupApi.GetGroupProperties(Group.Id);
            if (!IsPostBack) PopulateForm();
        }

        protected void PopulateForm()
        {
            PopulateImagesDdlForGroupProperties(Group.Id, ddlComputerImage);
            PopulateSitesDdl(ddlSite);
            PopulateBuildingsDdl(ddlBuilding);
            PopulateRoomsDdl(ddlRoom);
            PopulateClusterGroupsDdl(ddlClusterGroup);
            PopulateAltServerIps(ddlAltServer);

            chkDefault.Checked = Convert.ToBoolean(Group.SetDefaultProperties);
            if (_groupProperty != null)
            {
                ddlComputerImage.SelectedValue = _groupProperty.ImageId.ToString();
                txtComputerDesc.Text = _groupProperty.Description;
                PopulateImageProfilesDdl(ddlImageProfile, Convert.ToInt32(ddlComputerImage.SelectedValue));
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
                chkBootFile.Checked = Convert.ToBoolean(_groupProperty.BootFileEnabled);
                chkTftp.Checked = Convert.ToBoolean(_groupProperty.TftpServerEnabled);
                chkProxyReservation.Checked = Convert.ToBoolean(_groupProperty.ProxyEnabledEnabled);
                chkProxyEnabled.Checked = Convert.ToBoolean(_groupProperty.ProxyEnabled);
                txtTftp.Text = _groupProperty.TftpServer;
                ddlBootFile.Text = _groupProperty.BootFile;
                ddlClusterGroup.SelectedValue = _groupProperty.ClusterGroupId.ToString();
                chkClusterGroup.Checked = Convert.ToBoolean(_groupProperty.ClusterGroupEnabled);
                chkAltServer.Checked = Convert.ToBoolean(_groupProperty.AlternateServerIpEnabled);
                ddlAltServer.SelectedValue = _groupProperty.AlternateServerIpId.ToString();
                chkImageClass.Checked = Convert.ToBoolean(_groupProperty.ImageClassificationsEnabled);
            }
        }
    }
}