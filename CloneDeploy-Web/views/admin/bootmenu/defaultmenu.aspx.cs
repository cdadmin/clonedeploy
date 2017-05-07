using System;
using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

public partial class views_admin_bootmenu_defaultmenu : Admin
{
    public string biosLbl;
    public string efi32Lbl;
    public string efi64Lbl;
    public string noProxyLbl;


    public void btnSubmit_Click(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.UpdateAdmin);
        if (Settings.ProxyDhcp == "Yes")
            CreateProxyMenu();
        else
            CreateStandardMenu();
        EndUserMessage = "Complete";
    }

    protected void CreateProxyMenu()
    {
        var listSettings = new List<SettingEntity>
        {
            new SettingEntity
            {
                Name = "Ipxe Requires Login",
                Value = chkIpxeProxy.Checked.ToString(),
                Id = Call.SettingApi.GetSetting("Ipxe Requires Login").Id
            }
        };

        Call.SettingApi.UpdateSettings(listSettings);

        var defaultBootMenuOptions = new BootMenuGenOptionsDTO
        {
            DebugPwd = consoleShaProxy.Value,
            AddPwd = addcomputerShaProxy.Value,
            OndPwd = ondshaProxy.Value,
            DiagPwd = diagshaProxy.Value,
            GrubUserName = txtGrubProxyUsername.Text,
            GrubPassword = txtGrubProxyPassword.Text
        };

        defaultBootMenuOptions.Kernel = ddlBiosKernel.SelectedValue;
        defaultBootMenuOptions.BootImage = ddlBiosBootImage.SelectedValue;
        defaultBootMenuOptions.Type = "bios";
        Call.WorkflowApi.CreateDefaultBootMenu(defaultBootMenuOptions);

        defaultBootMenuOptions.Kernel = ddlEfi32Kernel.SelectedValue;
        defaultBootMenuOptions.BootImage = ddlEfi32BootImage.SelectedValue;
        defaultBootMenuOptions.Type = "efi32";
        Call.WorkflowApi.CreateDefaultBootMenu(defaultBootMenuOptions);

        defaultBootMenuOptions.Kernel = ddlEfi64Kernel.SelectedValue;
        defaultBootMenuOptions.BootImage = ddlEfi64BootImage.SelectedValue;
        defaultBootMenuOptions.Type = "efi64";
        Call.WorkflowApi.CreateDefaultBootMenu(defaultBootMenuOptions);
    }

    protected void CreateStandardMenu()
    {
        var listSettings = new List<SettingEntity>
        {
            new SettingEntity
            {
                Name = "Ipxe Requires Login",
                Value = chkIpxeLogin.Checked.ToString(),
                Id = Call.SettingApi.GetSetting("Ipxe Requires Login").Id
            }
        };

        Call.SettingApi.UpdateSettings(listSettings);

        var defaultBootMenuOptions = new BootMenuGenOptionsDTO();
        var pxeMode = Settings.PxeMode;
        if (pxeMode.Contains("grub"))
        {
            defaultBootMenuOptions.GrubUserName = txtGrubUsername.Text;
            defaultBootMenuOptions.GrubPassword = txtGrubPassword.Text;
        }
        else
        {
            defaultBootMenuOptions.DebugPwd = consoleSha.Value;
            defaultBootMenuOptions.AddPwd = addcomputerSha.Value;
            defaultBootMenuOptions.OndPwd = ondsha.Value;
            defaultBootMenuOptions.DiagPwd = diagsha.Value;
        }
        defaultBootMenuOptions.Kernel = ddlComputerKernel.SelectedValue;
        defaultBootMenuOptions.BootImage = ddlComputerBootImage.SelectedValue;
        defaultBootMenuOptions.Type = "standard";
        Call.WorkflowApi.CreateDefaultBootMenu(defaultBootMenuOptions);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) PopulateForm();
    }

    protected void PopulateForm()
    {
        var proxyDhcp = Settings.ProxyDhcp;

        if (proxyDhcp == "Yes")
        {
            divProxyDHCP.Visible = true;
            btnSubmitDefaultProxy.Visible = true;
            btnSubmitDefault.Visible = false;
            var biosFile = Settings.ProxyBiosFile;
            biosLbl = biosFile;
            var efi32File = Settings.ProxyEfi32File;
            efi32Lbl = efi32File;
            var efi64File = Settings.ProxyEfi64File;
            efi64Lbl = efi64File;
            if (biosFile.Contains("winpe"))
            {
                divProxyBios.Visible = false;
                lblBiosHidden.Text = "Bios Boot Menus Are Not Used When Proxy Bios Is Set To WinPE";
                lblBiosHidden.Visible = true;
            }
            if (efi32File.Contains("winpe"))
            {
                divProxyEfi32.Visible = false;
                lblEfi32Hidden.Text = "Efi32 Boot Menus Are Not Used When Proxy Efi32 Is Set To WinPE";
                lblEfi32Hidden.Visible = true;
            }
            if (efi64File.Contains("winpe"))
            {
                divProxyEfi64.Visible = false;
                lblEfi64Hidden.Text = "Efi64 Boot Menus Are Not Used When Proxy Efi64 Is Set To WinPE";
                lblEfi64Hidden.Visible = true;
            }
            if (biosFile.Contains("linux") || efi32File.Contains("linux") || efi64File.Contains("linux"))
                proxyPassBoxes.Visible = true;
            if (biosFile.Contains("ipxe") || efi32File.Contains("ipxe") || efi64File.Contains("ipxe"))
            {
                ipxeProxyPasses.Visible = true;
                chkIpxeProxy.Checked = Convert.ToBoolean(Settings.IpxeRequiresLogin);
            }
            if (efi64File.Contains("grub"))
                grubProxyPasses.Visible = true;

            ddlBiosKernel.DataSource = Call.FilesystemApi.GetKernels();
            ddlBiosKernel.DataBind();
            ddlBiosKernel.SelectedValue = Settings.DefaultKernel32;
            ddlEfi32Kernel.DataSource = Call.FilesystemApi.GetKernels();
            ddlEfi32Kernel.DataBind();
            ddlEfi32Kernel.SelectedValue = Settings.DefaultKernel32;
            ddlEfi64Kernel.DataSource = Call.FilesystemApi.GetKernels();
            ddlEfi64Kernel.DataBind();
            ddlEfi64Kernel.SelectedValue = Settings.DefaultKernel64;

            ddlBiosBootImage.DataSource = Call.FilesystemApi.GetBootImages();
            ddlBiosBootImage.DataBind();
            ddlBiosBootImage.SelectedValue = Settings.DefaultInit;
            ddlEfi32BootImage.DataSource = Call.FilesystemApi.GetBootImages();
            ddlEfi32BootImage.DataBind();
            ddlEfi32BootImage.SelectedValue = Settings.DefaultInit;
            ddlEfi64BootImage.DataSource = Call.FilesystemApi.GetBootImages();
            ddlEfi64BootImage.DataBind();
            ddlEfi64BootImage.SelectedValue = Settings.DefaultInit;
        }
        else
        {
            var pxeMode = Settings.PxeMode;
            noProxyLbl = pxeMode;
            if (pxeMode.Contains("winpe"))
            {
                lblNoMenu.Visible = true;
                btnSubmitDefaultProxy.Visible = false;
                btnSubmitDefault.Visible = false;
                return;
            }

            btnSubmitDefaultProxy.Visible = false;
            btnSubmitDefault.Visible = true;
            bootPasswords.Visible = true;
            divStandardMode.Visible = true;


            if (pxeMode.Contains("ipxe"))
            {
                passboxes.Visible = false;
                grubPassBoxes.Visible = false;
                ipxePassBoxes.Visible = true;
                chkIpxeLogin.Checked = Convert.ToBoolean(Settings.IpxeRequiresLogin);
            }
            else if (pxeMode.Contains("grub"))
            {
                passboxes.Visible = false;
                grubPassBoxes.Visible = true;
                ipxePassBoxes.Visible = false;
            }

            ddlComputerKernel.DataSource = Call.FilesystemApi.GetKernels();
            ddlComputerKernel.DataBind();
            ddlComputerKernel.SelectedValue = Settings.DefaultKernel32;


            ddlComputerBootImage.DataSource = Call.FilesystemApi.GetBootImages();
            ddlComputerBootImage.DataBind();
            ddlComputerBootImage.SelectedValue = Settings.DefaultInit;
        }
    }
}