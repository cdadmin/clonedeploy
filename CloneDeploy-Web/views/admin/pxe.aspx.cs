using System;
using System.Collections.Generic;
using System.Drawing;
using BasePages;
using Helpers;
using System.IO;
using CloneDeploy_Web.Models;

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
        RequiresAuthorization(Authorizations.UpdateAdmin);
        if (ValidateSettings())
        {
            var listSettings = new List<Setting>
            {
                new Setting {Name = "PXE Mode", Value = ddlPXEMode.Text, Id = BLL.Setting.GetSetting("PXE Mode").Id},
                new Setting {Name = "Proxy Dhcp", Value = ddlProxyDHCP.Text,  Id = BLL.Setting.GetSetting("Proxy Dhcp").Id},
                new Setting {Name = "Proxy Bios File", Value = ddlProxyBios.Text,  Id = BLL.Setting.GetSetting("Proxy Bios File").Id},
                new Setting {Name = "Proxy Efi32 File", Value = ddlProxyEfi32.Text,  Id = BLL.Setting.GetSetting("Proxy Efi32 File").Id},
                new Setting {Name = "Proxy Efi64 File", Value = ddlProxyEfi64.Text,  Id = BLL.Setting.GetSetting("Proxy Efi64 File").Id}
            };


            var newBootMenu = false;
            if (BLL.Setting.UpdateSetting(listSettings))
            {
                if (!new BLL.Workflows.CopyPxeBinaries().CopyFiles())
                {
                    EndUserMessage = "Could Not Copy PXE Binaries";
                    return;
                }
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
            else
            {
                EndUserMessage = "Could Not Update PXE Settings";
            }

            if (newBootMenu)
            {


                lblTitle.Text =
                    "Your Settings Changes Require A New PXE Boot File Be Created.  <br>Go There Now?";

                ClientScript.RegisterStartupScript(GetType(), "modalscript",
                    "$(function() {  var menuTop = document.getElementById('confirmbox'),body = document.body;classie.toggle(menuTop, 'confirm-box-outer-open'); });",
                    true);
                Session.Remove("Message");
            }
            else
            {
                EndUserMessage = "Successfully Updated PXE Settings";
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
        Response.Redirect("~/views/admin/bootmenu/defaultmenu.aspx?level=2");
    }

    protected bool ValidateSettings()
    {
        if (ddlProxyDHCP.Text == "No" && ddlPXEMode.Text.Contains("winpe"))
        {
            if (
               !new Helpers.FileOps().FileExists(Settings.TftpPath + Path.DirectorySeparatorChar + "boot" +
                                                 Path.DirectorySeparatorChar + "boot.sdi"))
            {
                EndUserMessage =
                    "Cannot Use WinPE.  You Have Not Updated Your tftpboot Folder With CloneDeploy PE Maker";
                return false;
            }
        }
        else if (ddlProxyDHCP.Text == "Yes" && ( ddlProxyBios.Text.Contains("winpe") || ddlProxyEfi32.Text.Contains("winpe") || ddlProxyEfi64.Text.Contains("winpe") ))
        {
            if (
                !new Helpers.FileOps().FileExists(Settings.TftpPath + Path.DirectorySeparatorChar + "boot" +
                                                  Path.DirectorySeparatorChar + "boot.sdi"))
            {
                EndUserMessage =
                    "Cannot Use WinPE.  You Have Not Updated Your tftpboot Folder With CloneDeploy PE Maker";
                return false;
            }
        }

        return true;
    }
}