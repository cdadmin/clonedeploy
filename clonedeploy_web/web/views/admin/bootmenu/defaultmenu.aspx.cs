using System;
using System.Collections.Generic;
using System.Web.UI;
using BLL;
using BLL.Workflows;
using Helpers;

public partial class views_admin_bootmenu_defaultmenu : Page
{
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
            var biosFile = Settings.ProxyBiosFile;
            var efi32File = Settings.ProxyEfi32File;
            var efi64File = Settings.ProxyEfi64File;
            if (biosFile.Contains("linux") || efi32File.Contains("linux") || efi64File.Contains("linux"))
                proxyPassBoxes.Visible = true;
            if (biosFile.Contains("ipxe") || efi32File.Contains("ipxe") || efi64File.Contains("ipxe"))
            {
                ipxeProxyPasses.Visible = true;
                chkIpxeProxy.Checked = Convert.ToBoolean(Settings.IpxeRequiresLogin);
            }
            if (efi64File.Contains("grub"))
                grubProxyPasses.Visible = true;
            try
            {
                ddlBiosKernel.DataSource = Utility.GetKernels();
                ddlBiosKernel.DataBind();
                ddlEfi32Kernel.DataSource = Utility.GetKernels();
                ddlEfi32Kernel.DataBind();
                ddlEfi64Kernel.DataSource = Utility.GetKernels();
                ddlEfi64Kernel.DataBind();

                var itemKernel = ddlBiosKernel.Items.FindByText(Settings.DefaultKernel32);
                if (itemKernel != null)
                {
                    ddlBiosKernel.SelectedValue = Settings.DefaultKernel32;
                    ddlEfi32Kernel.SelectedValue = Settings.DefaultKernel32;
                }
                else
                {
                    ddlBiosKernel.Items.Insert(0, "Select Kernel");
                    ddlEfi32Kernel.Items.Insert(0, "Select Kernel");
                }

                var itemKernel64 = ddlEfi64Kernel.Items.FindByText(Settings.DefaultKernel64);
                if (itemKernel64 != null)
                    ddlEfi64Kernel.SelectedValue = Settings.DefaultKernel64;
                else
                    ddlEfi64Kernel.Items.Insert(0, "Select Kernel");


                ddlBiosBootImage.DataSource = Utility.GetBootImages();
                ddlBiosBootImage.DataBind();
                ddlEfi32BootImage.DataSource = Utility.GetBootImages();
                ddlEfi32BootImage.DataBind();
                ddlEfi64BootImage.DataSource = Utility.GetBootImages();
                ddlEfi64BootImage.DataBind();

                var itemBootImage = ddlBiosBootImage.Items.FindByText("initrd.gz");
                if (itemBootImage != null)
                {
                    ddlBiosBootImage.SelectedValue = "initrd.gz";
                    ddlEfi32BootImage.SelectedValue = "initrd.gz";
                }
                else
                {
                    ddlBiosBootImage.Items.Insert(0, "Select Boot Image");
                    ddlEfi32BootImage.Items.Insert(0, "Select Boot Image");
                }

                var itemBootImage64 = ddlEfi64BootImage.Items.FindByText("initrd64.gz");
                if (itemBootImage64 != null)
                    ddlEfi64BootImage.SelectedValue = "initrd64.gz";
                else
                    ddlEfi64BootImage.Items.Insert(0, "Select Boot Image");
            }
            catch
            {
                // ignored
            }
        }
        else
        {
            bootPasswords.Visible = true;
            divStandardMode.Visible = true;
            var pxeMode = Settings.PxeMode;
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
            try
            {
                ddlHostKernel.DataSource = Utility.GetKernels();
                ddlHostKernel.DataBind();
                var itemKernel = ddlHostKernel.Items.FindByText(Settings.DefaultKernel32);
                if (itemKernel != null)
                    ddlHostKernel.SelectedValue = Settings.DefaultKernel32;
                else
                    ddlHostKernel.Items.Insert(0, "Select Kernel");
            }
            catch
            {
                // ignored
            }
            try
            {
                ddlHostBootImage.DataSource = Utility.GetBootImages();
                ddlHostBootImage.DataBind();
                var itemBootImage = ddlHostBootImage.Items.FindByText("initrd.gz");
                if (itemBootImage != null)
                    ddlHostBootImage.SelectedValue = "initrd.gz";
                else
                    ddlHostBootImage.Items.Insert(0, "Select Boot Image");
            }
            catch
            {
                // ignored
            }
        }
    }



    public void btnSubmit_Click(object sender, EventArgs e)
    {
        if(Settings.ProxyDhcp == "Yes")
            CreateProxyMenu();
        else
            CreateStandardMenu();
    }

    protected void CreateProxyMenu()
    {
        var defaultBootMenu = new BLL.Workflows.DefaultBootMenu
        {
            DebugPwd = consoleSha.Value,
            AddPwd = addhostSha.Value,
            OndPwd = ondsha.Value,
            DiagPwd = diagsha.Value,
            GrubUserName = txtGrubProxyUsername.Text,
            GrubPassword = txtGrubProxyPassword.Text,
        };

        defaultBootMenu.Kernel = ddlBiosKernel.SelectedValue;
        defaultBootMenu.BootImage = ddlBiosBootImage.SelectedValue;
        defaultBootMenu.Type = "bios";
        defaultBootMenu.CreateGlobalDefaultBootMenu();

        defaultBootMenu.Kernel = ddlEfi32Kernel.SelectedValue;
        defaultBootMenu.BootImage = ddlEfi32BootImage.SelectedValue;
        defaultBootMenu.Type = "efi32";
        defaultBootMenu.CreateGlobalDefaultBootMenu();

        defaultBootMenu.Kernel = ddlEfi64Kernel.SelectedValue;
        defaultBootMenu.BootImage = ddlEfi64BootImage.SelectedValue;
        defaultBootMenu.Type = "efi64";
        defaultBootMenu.CreateGlobalDefaultBootMenu();

        var listSettings = new List<Models.Setting>
        {
            new Models.Setting
            {
                Name = "Ipxe Requires Login",
                Value = chkIpxeProxy.Checked.ToString(),
                Id = Setting.GetSetting("Ipxe Requires Login").Id
            },
        };

        Setting.UpdateSetting(listSettings);
    }

    protected void CreateStandardMenu()
    {
        var defaultBootMenu = new BLL.Workflows.DefaultBootMenu();
        var pxeMode = Settings.PxeMode;
        if (pxeMode.Contains("grub"))
        {
            defaultBootMenu.GrubUserName = txtGrubUsername.Text;
            defaultBootMenu.GrubPassword = txtGrubPassword.Text;
        }
        else
        {
            defaultBootMenu.DebugPwd = consoleSha.Value;
            defaultBootMenu.AddPwd = addhostSha.Value;
            defaultBootMenu.OndPwd = ondsha.Value;
            defaultBootMenu.DiagPwd = diagsha.Value;
        }
        defaultBootMenu.Kernel = ddlHostKernel.SelectedValue;
        defaultBootMenu.BootImage = ddlHostBootImage.SelectedValue;
        defaultBootMenu.Type = "standard";
        defaultBootMenu.CreateGlobalDefaultBootMenu();

        var listSettings = new List<Models.Setting>
        {
            new Models.Setting
            {
                Name = "Ipxe Requires Login",
                Value = chkIpxeLogin.Checked.ToString(),
                Id = Setting.GetSetting("Ipxe Requires Login").Id
            },
        };

        Setting.UpdateSetting(listSettings);
    }
}