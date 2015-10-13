using System;
using System.IO;
using System.Net;
using BasePages;
using Helpers;
using Models;
using Pxe;
using Security;
using BootTemplate = BLL.BootTemplate;

namespace views.admin
{
    public partial class AdminBootMenu : Admin
    {
        protected void btnDeleteTemplate_Click(object sender, EventArgs e)
        {
            //FIX ME
            /*var template = new BootTemplate {Name = txtModifyTemplate.Text};
            template.Delete();
            modifyTemplate.Visible = false;
            showTemplates_Click(sender, e);
             * */
        }

        protected void btnGrubGen_Click(object sender, EventArgs e)
        {
            try
            {
                txtGrubSha.Text =
                    new WebClient().DownloadString("http://docs.cruciblewds.org/grub-pass-gen/encrypt.php?password=" +
                                                   txtGrubPass.Text);
                txtGrubSha.Text = txtGrubSha.Text.Replace("\n \n\n\n", "");
            }
            catch
            {
                txtGrubSha.Text = "Coud not contact http://cruciblewds.org to encrypt password.";
            }
        }

        public void btnNewTemplate_Click(object sender, EventArgs e)
        {
            if (txtNewTemplate.Text != "" && !txtNewTemplate.Text.Contains(" "))
            {
                //FIX ME
               /* var template = new BootTemplate
                {
                    Name = txtNewTemplate.Text,
                    Content = scriptEditorNewTemplate.Value
                };
                template.Create();
                * */
                showTemplates_Click(sender, e);
            }

            //else
                //Message.Text = "Template Name Cannot Be Empty Or Contain Spaces";
        }

        public void btnProxSubmitDefault_Click(object sender, EventArgs e)
        {
            var defaultBootMenu = new DefaultBootMenu
            {
                DebugPwd = proxconsoleSha.Value,
                AddPwd = proxaddhostSha.Value,
                OndPwd = proxondsha.Value,
                DiagPwd = proxdiagsha.Value,
                GrubUserName = txtGrubProxyUsername.Text,
                GrubPassword = txtGrubProxyPassword.Text,
                Kernel = ddlBiosKernel.SelectedValue,
                BootImage = ddlBiosBootImage.SelectedValue,
                Type = "bios"
            };
            defaultBootMenu.CreateGlobalDefaultBootMenu();

            defaultBootMenu.Kernel = ddlEfi32Kernel.SelectedValue;
            defaultBootMenu.BootImage = ddlEfi32BootImage.SelectedValue;
            defaultBootMenu.Type = "efi32";
            defaultBootMenu.CreateGlobalDefaultBootMenu();

            defaultBootMenu.Kernel = ddlEfi64Kernel.SelectedValue;
            defaultBootMenu.BootImage = ddlEfi64BootImage.SelectedValue;
            defaultBootMenu.Type = "efi64";
            defaultBootMenu.CreateGlobalDefaultBootMenu();

        }

        public void btnSubmitDefault_Click(object sender, EventArgs e)
        {
            var defaultBootMenu = new DefaultBootMenu();
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
            defaultBootMenu.Type = "noprox";
            defaultBootMenu.CreateGlobalDefaultBootMenu();
 
        }

        protected void btnUpdateTemplate_Click(object sender, EventArgs e)
        {
            //FIX ME
            /*
            var template = new BootTemplate
            {
                Name = txtModifyTemplate.Text,
                Content = scriptEditorModify.Value
            };
            template.Update();
             * */
        }

        protected void ddlTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ddlTemplate.Text)
            {
                case "default":

                    createNewTemplate.Visible = false;
                    modifyTemplate.Visible = false;

                    var proxyDhcp = Settings.ProxyDhcp;

                    if (proxyDhcp == "Yes")
                    {
                        divProxyDHCP.Visible = true;
                        var biosFile = Settings.ProxyBiosFile;
                        var efi32File = Settings.ProxyEfi32File;
                        var efi64File = Settings.ProxyEfi64File;
                        if (biosFile.Contains("linux") || efi32File.Contains("linux") || efi64File.Contains("linux"))
                        {
                            proxyPassBoxes.Visible = true;
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
                        divPXEMode.Visible = true;
                        var pxeMode = Settings.PxeMode;
                        if (pxeMode.Contains("ipxe"))
                        {
                            passboxes.Visible = false;
                            grubPassBoxes.Visible = false;
                        }
                        else if (pxeMode.Contains("grub"))
                        {
                            passboxes.Visible = false;
                            grubPassBoxes.Visible = true;
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
                    break;
                case "new template":
                    createNewTemplate.Visible = true;
                    bootPasswords.Visible = false;
                    modifyTemplate.Visible = false;
                    break;
                case "select template":
                    createNewTemplate.Visible = false;
                    bootPasswords.Visible = false;
                    modifyTemplate.Visible = false;
                    break;
                case "---------------":
                    //do nothing
                    break;
                default:
                    modifyTemplate.Visible = true;
                    createNewTemplate.Visible = false;
                    bootPasswords.Visible = false;
                    txtModifyTemplate.ReadOnly = true;
                    //FIX ME
                    /*
                    var template = new BootTemplate {Name = ddlTemplate.Text};
                    template.Read();
                    txtModifyTemplate.Text = template.Name;
                    scriptEditorModify.Value = template.Content;
                     * */
                    break;
            }
        }

        protected void EditProxy_Changed(object sender, EventArgs e)
        {
            string path = null;
            var tftpPath = Settings.TftpPath;
            var mode = Settings.PxeMode;
            var proxyDhcp = Settings.ProxyDhcp;

            if (proxyDhcp == "Yes")
            {
                var biosFile = Settings.ProxyBiosFile;
                var efi32File = Settings.ProxyEfi32File;
                var efi64File = Settings.ProxyEfi64File;
                proxyEditor.Visible = true;
                if (ddlEditProxyType.Text == "bios")
                {
                    if (biosFile.Contains("ipxe"))
                        path = tftpPath + "proxy" + Path.DirectorySeparatorChar +
                               ddlEditProxyType.Text + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + "default.ipxe";
                    else
                        path = tftpPath + "proxy" + Path.DirectorySeparatorChar +
                               ddlEditProxyType.Text + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + "default";
                }

                if (ddlEditProxyType.Text == "efi32")
                {
                    if (efi32File.Contains("ipxe"))
                        path = tftpPath + "proxy" + Path.DirectorySeparatorChar +
                               ddlEditProxyType.Text + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + "default.ipxe";
                    else
                        path = tftpPath + "proxy" + Path.DirectorySeparatorChar +
                               ddlEditProxyType.Text + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + "default";
                }

                if (ddlEditProxyType.Text == "efi64")
                {
                    if (efi64File.Contains("ipxe"))
                        path = tftpPath + "proxy" + Path.DirectorySeparatorChar +
                               ddlEditProxyType.Text + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + "default.ipxe";
                    else if (efi64File.Contains("grub"))
                        path = tftpPath + "grub" + Path.DirectorySeparatorChar + "grub.cfg";
                    else
                        path = tftpPath + "proxy" + Path.DirectorySeparatorChar +
                               ddlEditProxyType.Text + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + "default";
                }
            }
            else
            {
                proxyEditor.Visible = false;

                if (mode.Contains("ipxe"))
                    path = tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                           "default.ipxe";
                else if (mode.Contains("grub"))
                    path = tftpPath + "grub" + Path.DirectorySeparatorChar + "grub.cfg";
                else
                    path = tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar + "default";
            }
            lblFileName1.Text = path;
            try
            {
                if (path != null) scriptEditorText.Value = File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                if (path != null)
                {
                    path = path.Replace(@"\", @"\\");
                    //Message.Text = "Could Not Find " + path;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
          

            var opendefault = Request.QueryString["defaultmenu"];

            if (IsPostBack) return;
            if (opendefault != "true") return;
            bootEditor.Visible = false;
            bootTemplates.Visible = true;

            createNewTemplate.Visible = false;
            modifyTemplate.Visible = false;

            var proxyDhcp = Settings.ProxyDhcp;

            if (proxyDhcp == "Yes")
            {
                divProxyDHCP.Visible = true;
                var biosFile = Settings.ProxyBiosFile;
                var efi32File = Settings.ProxyEfi32File;
                var efi64File = Settings.ProxyEfi64File;
                if (biosFile.Contains("linux") || efi32File.Contains("linux") || efi64File.Contains("linux"))
                {
                    proxyPassBoxes.Visible = true;
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
                divPXEMode.Visible = true;
                var pxeMode = Settings.PxeMode;
                if (pxeMode.Contains("ipxe"))
                {
                    passboxes.Visible = false;
                    grubPassBoxes.Visible = false;
                }
                else if (pxeMode.Contains("grub"))
                {
                    passboxes.Visible = false;
                    grubPassBoxes.Visible = true;
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

        protected void saveEditor_Click(object sender, EventArgs e)
        {
            string path = null;
            var tftpPath = Settings.TftpPath;
            var mode = Settings.PxeMode;
            var proxyDhcp = Settings.ProxyDhcp;

            if (proxyDhcp == "Yes")
            {
                var biosFile = Settings.ProxyBiosFile;
                var efi32File = Settings.ProxyEfi32File;
                var efi64File = Settings.ProxyEfi64File;
                proxyEditor.Visible = true;
                if (ddlEditProxyType.Text == "bios")
                {
                    if (biosFile.Contains("ipxe"))
                        path = tftpPath + "proxy" + Path.DirectorySeparatorChar +
                               ddlEditProxyType.Text + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + "default.ipxe";
                    else
                        path = tftpPath + "proxy" + Path.DirectorySeparatorChar +
                               ddlEditProxyType.Text + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + "default";
                }

                if (ddlEditProxyType.Text == "efi32")
                {
                    if (efi32File.Contains("ipxe"))
                        path = tftpPath + "proxy" + Path.DirectorySeparatorChar +
                               ddlEditProxyType.Text + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + "default.ipxe";
                    else
                        path = tftpPath + "proxy" + Path.DirectorySeparatorChar +
                               ddlEditProxyType.Text + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + "default";
                }

                if (ddlEditProxyType.Text == "efi64")
                {
                    if (efi64File.Contains("ipxe"))
                        path = tftpPath + "proxy" + Path.DirectorySeparatorChar +
                               ddlEditProxyType.Text + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + "default.ipxe";
                    else if (mode.Contains("grub"))
                        path = tftpPath + "grub" + Path.DirectorySeparatorChar + "grub.cfg";
                    else
                        path = tftpPath + "proxy" + Path.DirectorySeparatorChar +
                               ddlEditProxyType.Text + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + "default";
                }
            }
            else
            {
                if (mode.Contains("ipxe"))
                    path = tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                           "default.ipxe";
                else if (mode.Contains("grub"))
                {
                    path = tftpPath + "grub" + Path.DirectorySeparatorChar + "grub.cfg";
                }
                else
                    path = tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar + "default";
            }
            try
            {
                if (path != null)
                {
                    using (var file = new StreamWriter(path))
                    {
                        file.WriteLine(scriptEditorText.Value);
                    }
                    new FileOps().SetUnixPermissions(path);
                }
                //Message.Text = "Successfully Updated Default Global Boot Menu";
            }

            catch (Exception ex)
            {
                
                    //Message.Text = "Could Not Update Default Global Boot Menu.  Check The Exception Log For More Info.";
                Logger.Log(ex.Message);
            }
        }

        protected void showEditor_Click(object sender, EventArgs e)
        {
            bootEditor.Visible = true;
            bootTemplates.Visible = false;
            string path = null;
            var tftpPath = Settings.TftpPath;
            var mode = Settings.PxeMode;
            var proxyDhcp = Settings.ProxyDhcp;

            if (proxyDhcp == "Yes")
            {
                var biosFile = Settings.ProxyBiosFile;
                var efi32File = Settings.ProxyEfi32File;
                var efi64File = Settings.ProxyEfi64File;
                proxyEditor.Visible = true;
                if (ddlEditProxyType.Text == "bios")
                {
                    if (biosFile.Contains("ipxe"))
                        path = tftpPath + "proxy" + Path.DirectorySeparatorChar +
                               ddlEditProxyType.Text + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + "default.ipxe";
                    else
                        path = tftpPath + "proxy" + Path.DirectorySeparatorChar +
                               ddlEditProxyType.Text + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + "default";
                }

                if (ddlEditProxyType.Text == "efi32")
                {
                    if (efi32File.Contains("ipxe"))
                        path = tftpPath + "proxy" + Path.DirectorySeparatorChar +
                               ddlEditProxyType.Text + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + "default.ipxe";
                    else
                        path = tftpPath + "proxy" + Path.DirectorySeparatorChar +
                               ddlEditProxyType.Text + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + "default";
                }

                if (ddlEditProxyType.Text == "efi64")
                {
                    if (efi64File.Contains("ipxe"))
                        path = tftpPath + "proxy" + Path.DirectorySeparatorChar +
                               ddlEditProxyType.Text + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + "default.ipxe";
                    else if (mode.Contains("grub"))
                        path = tftpPath + "grub" + Path.DirectorySeparatorChar + "grub.cfg";
                    else
                        path = tftpPath + "proxy" + Path.DirectorySeparatorChar +
                               ddlEditProxyType.Text + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + "default";
                }
            }
            else
            {
                proxyEditor.Visible = false;

                if (mode.Contains("ipxe"))
                {
                    grubShaGen.Visible = false;
                    syslinuxShaGen.Visible = false;
                    path = tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                           "default.ipxe";
                }
                else if (mode.Contains("grub"))
                {
                    grubShaGen.Visible = true;
                    syslinuxShaGen.Visible = false;
                    path = tftpPath + "grub" + Path.DirectorySeparatorChar + "grub.cfg";
                }
                else
                {
                    grubShaGen.Visible = false;
                    syslinuxShaGen.Visible = true;
                    path = tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar + "default";
                }
            }
            lblFileName1.Text = path;
            try
            {
                if (path != null) scriptEditorText.Value = File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                if (path != null)
                {
                    path = path.Replace(@"\", @"\\");
                    //Message.Text = "Could Not Find " + path;
                }
            }
        }

        protected void showTemplates_Click(object sender, EventArgs e)
        {
            bootEditor.Visible = false;
            bootTemplates.Visible = true;

            //FIX ME
            //ddlTemplate.DataSource = new BootTemplate().ListAll();
            ddlTemplate.DataBind();
            ddlTemplate.Items.Insert(0, "select template");
            ddlTemplate.Items.Insert(1, "default");
            ddlTemplate.Items.Insert(2, "new template");
            ddlTemplate.Items.Insert(3, "---------------");
        }
    }
}