using System;
using System.Collections.Generic;
using System.Drawing;
using BasePages;
using Helpers;
using Models;
using Pxe;
using ActiveImagingTask = BLL.ActiveImagingTask;

public partial class views_admin_pxe : Admin
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;

        ddlPXEMode.SelectedValue = Settings.PxeMode;
        ddlProxyDHCP.SelectedValue = Settings.ProxyDhcp;
        ddlProxyBios.SelectedValue = Settings.ProxyBiosFile;
        ddlProxyEfi32.SelectedValue = Settings.ProxyEfi32File;
        ddlProxyEfi64.SelectedValue = Settings.ProxyEfi64File;

        //These require pxe boot menu or client iso to be recreated
        ViewState["pxeMode"] = ddlPXEMode.Text;
        ViewState["proxyDHCP"] = ddlProxyDHCP.SelectedValue;
        ViewState["proxyBios"] = ddlProxyBios.SelectedValue;
        ViewState["proxyEfi32"] = ddlProxyEfi32.SelectedValue;
        ViewState["proxyEfi64"] = ddlProxyEfi64.SelectedValue;

        ShowProxyMode();
    }

    protected void btnUpdateSettings_OnClick(object sender, EventArgs e)
    {
        if (ValidateSettings())
        {
            var listSettings = new List<Setting>
            {
                new Setting {Name = "PXE Mode", Value = ddlPXEMode.Text},
                new Setting {Name = "Proxy Dhcp", Value = ddlProxyDHCP.Text},
                new Setting {Name = "Proxy Bios File", Value = ddlProxyBios.Text},
                new Setting {Name = "Proxy Efi32 File", Value = ddlProxyEfi32.Text},
                new Setting {Name = "Proxy Efi64 File", Value = ddlProxyEfi64.Text}
            };


            var newBootMenu = false;
            if (new BLL.Setting().UpdateSetting(listSettings))
            {
                new PxeFileOps().CopyPxeFiles(ddlPXEMode.Text);

                if ((string) ViewState["proxyDHCP"] != ddlProxyDHCP.Text)
                    newBootMenu = true;
                if ((string) ViewState["proxyBios"] != ddlProxyBios.Text)
                    newBootMenu = true;
                if ((string) ViewState["proxyEfi32"] != ddlProxyEfi32.Text)
                    newBootMenu = true;
                if ((string) ViewState["proxyEfi64"] != ddlProxyEfi64.Text)
                    newBootMenu = true;
                if ((string) ViewState["pxeMode"] != ddlPXEMode.Text)
                {
                    newBootMenu = true;
                }
            }

            if (newBootMenu)
            {

                lblTitle.Text = Message.Text;
                lblTitle.Text +=
                    "<br> Your Settings Changes Require A New PXE Boot File Be Created.  <br>Create It Now?";

                ClientScript.RegisterStartupScript(GetType(), "modalscript",
                    "$(function() {  var menuTop = document.getElementById('confirmbox'),body = document.body;classie.toggle(menuTop, 'confirm-box-outer-open'); });",
                    true);
                Session.Remove("Message");
            }
        }

    }

    protected void ProxyDhcp_Changed(object sender, EventArgs e)
    {
        ShowProxyMode();
    }

    protected void ShowProxyMode()
    {
        ddlProxyBios.BackColor = Color.White;
        ddlProxyEfi32.BackColor = Color.White;
        ddlProxyEfi64.BackColor = Color.White;
        ddlProxyBios.Font.Strikeout = false;
        ddlProxyEfi32.Font.Strikeout = false;
        ddlProxyEfi64.Font.Strikeout = false;
        ddlPXEMode.BackColor = Color.White;
        ddlPXEMode.Font.Strikeout = false;
        if (ddlProxyDHCP.Text == "No")
        {
            ddlProxyBios.BackColor = Color.LightGray;
            ddlProxyEfi32.BackColor = Color.LightGray;
            ddlProxyEfi64.BackColor = Color.LightGray;
            ddlProxyBios.Font.Strikeout = true;
            ddlProxyEfi32.Font.Strikeout = true;
            ddlProxyEfi64.Font.Strikeout = true;
        }
        else
        {
            ddlPXEMode.BackColor = Color.LightGray;
            ddlPXEMode.Font.Strikeout = true;
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

        return true;
    }
}