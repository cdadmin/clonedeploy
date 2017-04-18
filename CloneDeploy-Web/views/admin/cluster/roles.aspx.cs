using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CloneDeploy_ApiCalls;
using CloneDeploy_Entities;
using CloneDeploy_Web.Helpers;

namespace CloneDeploy_Web.views.admin.cluster
{
    public partial class roles : BasePages.Admin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (IsPostBack) return;

            ddlOperationMode.Text = Settings.OperationMode;
            if (ddlOperationMode.Text != "single")
            {
               
                chkTftpServer.Checked = Settings.TftpServerRole;
                chkMulticastServer.Checked = Settings.MulticastServerRole;
            }
            DisplayRoles();
            
        }

        protected void btnUpdateSettings_OnClick(object sender, EventArgs e)
        {
            RequiresAuthorization(Authorizations.UpdateAdmin);
            List<SettingEntity> listSettings = new List<SettingEntity>
            {
                new SettingEntity
                {
                    Name = "Operation Mode",
                    Value = ddlOperationMode.Text,
                    Id = Call.SettingApi.GetSetting("Operation Mode").Id
                },
                new SettingEntity
                {
                    Name = "Image Server Role",
                    Value = chkImageServer.Checked ? "1" : "0",
                    Id = Call.SettingApi.GetSetting("Image Server Role").Id
                },
                 new SettingEntity
                {
                    Name = "Tftp Server Role",
                    Value = chkTftpServer.Checked ? "1" : "0",
                    Id = Call.SettingApi.GetSetting("Tftp Server Role").Id
                },
                 new SettingEntity
                {
                    Name = "Multicast Server Role",
                    Value = chkMulticastServer.Checked ? "1" : "0",
                    Id = Call.SettingApi.GetSetting("Multicast Server Role").Id
                }
            };

            EndUserMessage = Call.SettingApi.UpdateSettings(listSettings) ? "Successfully Updated Settings" : "Could Not Update Settings";


        }

        protected void ddlOperationMode_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayRoles();
        }

        private void DisplayRoles()
        {
            divRoles.Visible = ddlOperationMode.Text != "Single";
        }
    }
}