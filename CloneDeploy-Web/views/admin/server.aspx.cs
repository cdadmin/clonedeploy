using System;
using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Web;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

public partial class views_admin_server : Admin
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        txtIP.Text = Settings.ServerIp;
        txtPort.Text = Settings.WebServerPort;
        txtTFTPPath.Text = Settings.TftpPath;
        ddlComputerView.SelectedValue = Settings.DefaultComputerView;
        txtWebService.Text = Settings.WebPath;
        txtId.Text = Settings.ServerIdentifier;

        //These require pxe boot menu or client iso to be recreated
        ViewState["serverIP"] = txtIP.Text;
        ViewState["serverPort"] = txtPort.Text;
        ViewState["servicePath"] = txtWebService.Text;
    }

    protected void btnUpdateSettings_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.UpdateAdmin);
        if (!ValidateSettings()) return;
        var listSettings = new List<SettingEntity>
        {
            new SettingEntity {Name = "Server IP", Value = txtIP.Text, Id = Call.SettingApi.GetSetting("Server IP").Id},
            new SettingEntity {Name = "Server Identifier", Value = txtId.Text, Id = Call.SettingApi.GetSetting("Server Identifier").Id},
            new SettingEntity {Name = "Web Server Port", Value = txtPort.Text, Id = Call.SettingApi.GetSetting("Web Server Port").Id},
            new SettingEntity {Name = "Tftp Path", Value = txtTFTPPath.Text, Id = Call.SettingApi.GetSetting("Tftp Path").Id},
            new SettingEntity {Name = "Default Computer View", Value = ddlComputerView.Text, Id = Call.SettingApi.GetSetting("Default Computer View").Id},
            new SettingEntity {Name = "Web Path", Value = txtWebService.Text, Id = Call.SettingApi.GetSetting("Web Path").Id}
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
            if ((string)ViewState["servicePath"] != Utility.Between(txtWebService.Text))
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

    protected bool ValidateSettings()
    {
        

        if (!chkOverride.Checked)
        {
            if (txtPort.Text != "80" && txtPort.Text != "443" && !string.IsNullOrEmpty(txtPort.Text))
            {
                txtWebService.Text = "http://[server-ip]:" + txtPort.Text + "/clonedeploy/service/client.asmx/";
            }
            if (txtPort.Text == "80" || string.IsNullOrEmpty(txtPort.Text))
            {
                txtWebService.Text = "http://[server-ip]/clonedeploy/service/client.asmx/";
            }
            if(txtPort.Text == "443")
                txtWebService.Text = "https://[server-ip]/clonedeploy/service/client.asmx/";
        }
        var seperator = Call.FilesystemApi.GetServerPaths("seperator", "");
        if (!txtTFTPPath.Text.Trim().EndsWith(seperator))
            txtTFTPPath.Text += seperator;

        return true;
    }
}