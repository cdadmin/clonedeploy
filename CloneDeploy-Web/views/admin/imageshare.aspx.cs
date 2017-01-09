using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CloneDeploy_Entities;
using CloneDeploy_Web.Helpers;

namespace CloneDeploy_Web.views.admin
{
    public partial class imageshare : BasePages.Admin
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Settings.OperationMode != "Single" && !Settings.ImageServerRole)
            {
                divShareDisabled.Visible = true;
            }
            else
            {
                divShareEnabled.Visible = true;
                if (!IsPostBack)
                {
                    var imageShare = Call.SettingApi.GetImageShareSettings();
                    ddlType.Text = imageShare.Type;
                    txtServer.Text = imageShare.Server;
                    txtShareName.Text = imageShare.Name;
                    txtDomain.Text = imageShare.Domain;
                    txtRwUsername.Text = imageShare.ReadWriteUser;
                    txtRwPassword.Text = imageShare.ReadWritePassword;
                    txtRoUsername.Text = imageShare.ReadOnlyUser;
                    txtRoPassword.Text = imageShare.ReadOnlyPassword;
                    txtPhysicalPath.Text = imageShare.PhysicalPath;
                    txtQueueSize.Text = imageShare.QueueSize;
                }

            }
        }

        protected void btnUpdateSettings_OnClick(object sender, EventArgs e)
        {
            RequiresAuthorization(Authorizations.UpdateAdmin);    
            var listSettings = new List<SettingEntity>
            {
                new SettingEntity
                {
                    Name = "Image Share Type",
                    Value = ddlType.Text,
                    Id = Call.SettingApi.GetSetting("Image Share Type").Id
                },
                new SettingEntity
                {
                    Name = "Image Server Ip",
                    Value = txtServer.Text,
                    Id = Call.SettingApi.GetSetting("Image Server Ip").Id
                },
                new SettingEntity
                {
                    Name = "Image Share Name",
                    Value = txtShareName.Text,
                    Id = Call.SettingApi.GetSetting("Image Share Name").Id
                },
                new SettingEntity
                {
                    Name = "Image Domain",
                    Value = txtDomain.Text,
                    Id = Call.SettingApi.GetSetting("Image Domain").Id
                },
                new SettingEntity
                {
                    Name = "Image ReadWrite Username",
                    Value = txtRwUsername.Text,
                    Id = Call.SettingApi.GetSetting("Image ReadWrite Username").Id
                },
                new SettingEntity
                {
                    Name = "Image ReadWrite Password Encrypted",
                    Value = txtRwPassword.Text,
                    Id = Call.SettingApi.GetSetting("Image ReadWrite Password Encrypted").Id
                },
                new SettingEntity
                {
                    Name = "Image ReadOnly Username",
                    Value = txtRoUsername.Text,
                    Id = Call.SettingApi.GetSetting("Image ReadOnly Username").Id
                },
                new SettingEntity
                {
                    Name = "Image ReadOnly Password Encrypted",
                    Value = txtRoPassword.Text,
                    Id = Call.SettingApi.GetSetting("Image ReadOnly Password Encrypted").Id
                },
                 new SettingEntity
                {
                    Name = "Image Physical Path",
                    Value = txtPhysicalPath.Text,
                    Id = Call.SettingApi.GetSetting("Image Physical Path").Id
                },
                 new SettingEntity
                {
                    Name = "Image Queue Size",
                    Value = txtQueueSize.Text,
                    Id = Call.SettingApi.GetSetting("Image Queue Size").Id
                },
            };

         
            if (Call.SettingApi.UpdateSettings(listSettings))
            {
                EndUserMessage = "Successfully Updated Settings";
            }
        }
    }
}