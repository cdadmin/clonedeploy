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
        txtImagePath.Text = Settings.ImageStorePath;
        txtImageHoldPath.Text = Settings.ImageHoldPath;
        txtTFTPPath.Text = Settings.TftpPath;
        txtWebService.Text = Setting.GetValueForAdminView(Settings.WebPath);
        ddlHostView.SelectedValue = Settings.DefaultHostView;

        //These require pxe boot menu or client iso to be recreated
        ViewState["serverIP"] = txtIP.Text;
        ViewState["serverPort"] = txtPort.Text;
        ViewState["webService"] = txtWebService.Text;
    }

    protected void btnUpdateSettings_OnClick(object sender, EventArgs e)
    {
        if (ValidateSettings())
        {
            List<Models.Setting> listSettings = new List<Models.Setting>
            {
                new Models.Setting {Name = "Server IP", Value = txtIP.Text, Id = Setting.GetSetting("Server IP").Id},
                new Models.Setting {Name = "Web Server Port", Value = txtPort.Text, Id = Setting.GetSetting("Web Server Port").Id},
                new Models.Setting {Name = "Image Store Path", Value = txtImagePath.Text, Id = Setting.GetSetting("Image Store Path").Id},
                new Models.Setting {Name = "Tftp Path", Value = txtTFTPPath.Text, Id = Setting.GetSetting("Tftp Path").Id},
                new Models.Setting {Name = "Web Path", Value = txtWebService.Text, Id = Setting.GetSetting("Web Path").Id},
                new Models.Setting {Name = "Default Host View", Value = ddlHostView.Text, Id = Setting.GetSetting("Default Host View").Id},
                new Models.Setting {Name = "Image Hold Path", Value = txtImageHoldPath.Text, Id = Setting.GetSetting("Image Hold path").Id}
            };

            var newBootMenu = false;
            var newClientIso = false;
            if (new Setting().UpdateSetting(listSettings))
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


                if ((string) ViewState["webService"] != txtWebService.Text)
                {
                    newBootMenu = true;
                    newClientIso = true;
                }



            }

            if (newBootMenu)
            {
               
                lblTitle.Text = Message.Text;
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

            

        }
    }


    protected void OkButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/views/admin/bootmenu.aspx?defaultmenu=true");
    }

    protected bool ValidateSettings()
    {
        if (new ActiveImagingTask().ReadAll().Count > 0)
        {
            Message.Text = "Settings Cannot Be Changed While Tasks Are Active";
            return false;
        }
        if (txtPort.Text != "80" && txtPort.Text != "443" && !string.IsNullOrEmpty(txtPort.Text))
        {
            txtWebService.Text = "http://[server-ip]:" + txtPort.Text + "/cruciblewds/service/client.asmx/";
        }
        if (txtPort.Text == "80" || txtPort.Text == "443" || string.IsNullOrEmpty(txtPort.Text))
        {
            txtWebService.Text = "http://[server-ip]/cruciblewds/service/client.asmx/";
        }
       
        if (!txtImagePath.Text.Trim().EndsWith(Path.DirectorySeparatorChar.ToString()))
            txtImagePath.Text += Path.DirectorySeparatorChar;

        if (!txtImageHoldPath.Text.Trim().EndsWith(Path.DirectorySeparatorChar.ToString()))
            txtImageHoldPath.Text += Path.DirectorySeparatorChar;

        if (!txtTFTPPath.Text.Trim().EndsWith(Path.DirectorySeparatorChar.ToString()))
            txtTFTPPath.Text += Path.DirectorySeparatorChar;

        if (!txtWebService.Text.Trim().EndsWith("/"))
            txtWebService.Text += ("/");

        return true;
    }
}