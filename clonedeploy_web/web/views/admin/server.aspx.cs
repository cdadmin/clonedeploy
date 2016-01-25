using System;
using System.Collections.Generic;
using System.IO;
using BasePages;
using BLL;
using Helpers;

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

        //These require pxe boot menu or client iso to be recreated
        ViewState["serverIP"] = txtIP.Text;
        ViewState["serverPort"] = txtPort.Text;
    }

    protected void btnUpdateSettings_OnClick(object sender, EventArgs e)
    {
        if (!ValidateSettings()) return;
        var listSettings = new List<Models.Setting>
        {
            new Models.Setting {Name = "Server IP", Value = txtIP.Text, Id = Setting.GetSetting("Server IP").Id},
            new Models.Setting {Name = "Web Server Port", Value = txtPort.Text, Id = Setting.GetSetting("Web Server Port").Id},
            new Models.Setting {Name = "Tftp Path", Value = txtTFTPPath.Text, Id = Setting.GetSetting("Tftp Path").Id},
            new Models.Setting {Name = "Default Computer View", Value = ddlComputerView.Text, Id = Setting.GetSetting("Default Computer View").Id},
            new Models.Setting {Name = "Web Path", Value = txtWebService.Text, Id = Setting.GetSetting("Web Path").Id}
        };

        var newBootMenu = false;
        var newClientIso = false;
        if (Setting.UpdateSetting(listSettings))
        {

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
        }

        if (!newBootMenu) return;

        lblTitle.Text = EndUserMessage;
        lblTitle.Text +=
            "<br> Your Settings Changes Require A New PXE Boot File Be Created.  <br>Create It Now?";
        if (newClientIso)
        {
            lblClientISO.Text = "If You Are Using The Client ISO, It Must Also Be Manually Updated.";
        }
        ClientScript.RegisterStartupScript(GetType(), "modalscript",
            "$(function() {  var menuTop = document.getElementById('confirmbox'),body = document.body;classie.toggle(menuTop, 'confirm-box-outer-open'); });",
            true);
        Session.Remove("Message");
    }


    protected void OkButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/views/admin/bootmenu.aspx?defaultmenu=true");
    }

    protected bool ValidateSettings()
    {
        if (ActiveImagingTask.ReadAll().Count > 0)
        {
            EndUserMessage = "Settings Cannot Be Changed While Tasks Are Active";
            return false;
        }

        if (!txtTFTPPath.Text.Trim().EndsWith(Path.DirectorySeparatorChar.ToString()))
            txtTFTPPath.Text += Path.DirectorySeparatorChar;

        return true;
    }
}