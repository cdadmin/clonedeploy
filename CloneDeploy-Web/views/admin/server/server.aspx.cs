using System;
using System.Collections.Generic;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.admin.server
{
    public partial class serversettings : Admin
    {
        protected void btnUpdateSettings_OnClick(object sender, EventArgs e)
        {
            RequiresAuthorization(AuthorizationStrings.UpdateAdmin);
            if (!ValidateSettings()) return;
            var listSettings = new List<SettingEntity>
            {
                new SettingEntity
                {
                    Name = "Server IP",
                    Value = txtIP.Text,
                    Id = Call.SettingApi.GetSetting("Server IP").Id
                },
                new SettingEntity
                {
                    Name = "Server Identifier",
                    Value = txtId.Text,
                    Id = Call.SettingApi.GetSetting("Server Identifier").Id
                },
                new SettingEntity
                {
                    Name = "Web Server Port",
                    Value = txtPort.Text,
                    Id = Call.SettingApi.GetSetting("Web Server Port").Id
                },
                new SettingEntity
                {
                    Name = "Tftp Path",
                    Value = txtTFTPPath.Text,
                    Id = Call.SettingApi.GetSetting("Tftp Path").Id
                },
                new SettingEntity
                {
                    Name = "Default Computer View",
                    Value = ddlComputerView.Text,
                    Id = Call.SettingApi.GetSetting("Default Computer View").Id
                },
                new SettingEntity
                {
                    Name = "Web Path",
                    Value = txtWebService.Text,
                    Id = Call.SettingApi.GetSetting("Web Path").Id
                },
                new SettingEntity
                {
                    Name = "Tftp Server IP",
                    Value = txtTftpServerIp.Text,
                    Id = Call.SettingApi.GetSetting("Tftp Server IP").Id
                }
            };

            var newBootMenu = false;
            var newClientIso = false;
            if (Call.SettingApi.UpdateSettings(listSettings))
            {
                EndUserMessage = "Successfully Updated Settings";
                if ((string) ViewState["serverIP"] != txtIP.Text)
                {
                    newBootMenu = true;
                    newClientIso = true;
                }
                if ((string) ViewState["serverPort"] != txtPort.Text)
                {
                    newBootMenu = true;
                    newClientIso = true;
                }
                if ((string) ViewState["servicePath"] != PlaceHolderReplace(txtWebService.Text))
                {
                    newBootMenu = true;
                    newClientIso = true;
                }
            }
            else
            {
                EndUserMessage = "Could Not Update Settings";
            }

            if (!newBootMenu) return;

            lblTitle.Text =
                "Your Settings Changes Require A New PXE Boot File Be Created.  <br>Go There Now?";
            if (newClientIso)
            {
                lblClientISO.Text = "The Client ISO Must Also Be Updated.";
            }
            ClientScript.RegisterStartupScript(GetType(), "modalscript",
                "$(function() {  var menuTop = document.getElementById('confirmbox'),body = document.body;classie.toggle(menuTop, 'confirm-box-outer-open'); });",
                true);
            Session.Remove("Message");
        }

        protected void OkButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/views/admin/bootmenu/defaultmenu.aspx?level=2");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (GetSetting(SettingStrings.OperationMode) == "Cluster Secondary")
            {
                divComputerView.Visible = false;
            }
            if (IsPostBack) return;
            txtIP.Text = GetSetting(SettingStrings.ServerIp);
            txtPort.Text = GetSetting(SettingStrings.WebServerPort);
            txtTFTPPath.Text = GetSetting(SettingStrings.TftpPath);
            ddlComputerView.SelectedValue = GetSetting(SettingStrings.DefaultComputerView);
            txtWebService.Text = GetSetting(SettingStrings.WebPath);
            txtId.Text = GetSetting(SettingStrings.ServerIdentifier);
            txtTftpServerIp.Text = GetSetting(SettingStrings.TftpServerIp);

            //These require pxe boot menu or client iso to be recreated
            ViewState["serverIP"] = txtIP.Text;
            ViewState["serverPort"] = txtPort.Text;
            ViewState["servicePath"] = txtWebService.Text;
        }

        protected bool ValidateSettings()
        {
            if (!chkOverride.Checked)
            {
                if (txtPort.Text != "80" && txtPort.Text != "443" && !string.IsNullOrEmpty(txtPort.Text))
                {
                    txtWebService.Text = "http://[server-ip]:" + txtPort.Text + "/clonedeploy/api/";
                }
                if (txtPort.Text == "80" || string.IsNullOrEmpty(txtPort.Text))
                {
                    txtWebService.Text = "http://[server-ip]/clonedeploy/api/";
                }
                if (txtPort.Text == "443")
                    txtWebService.Text = "https://[server-ip]/clonedeploy/api/";
            }
            else
            {
                if (!txtWebService.Text.Trim().EndsWith("/"))
                    txtWebService.Text += "/";
            }
            var seperator = Call.FilesystemApi.GetServerPaths("seperator", "");
            if (!txtTFTPPath.Text.Trim().EndsWith(seperator))
                txtTFTPPath.Text += seperator;

            return true;
        }
    }
}