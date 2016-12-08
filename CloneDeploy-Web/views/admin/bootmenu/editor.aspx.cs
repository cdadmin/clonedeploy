using System;
using System.IO;
using System.Net;
using BasePages;

using CloneDeploy_Web;

public partial class views_admin_bootmenu_editor : Admin
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) PopulateForm();
    }

        protected void btnGrubGen_Click(object sender, EventArgs e)
        {
            try
            {
                txtGrubSha.Text =
                    new WebClient().DownloadString("http://docs.clonedeploy.org/grub-pass-gen/encrypt.php?password=" +
                                                   txtGrubPass.Text);
                txtGrubSha.Text = txtGrubSha.Text.Replace("\n \n\n\n", "");
            }
            catch
            {
                txtGrubSha.Text = "Coud not contact http://clonedeploy.org to encrypt password.";
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
                btnSaveEditor.Visible = true;
              
                srvEdit.Visible = true;

                var biosFile = Settings.ProxyBiosFile;
                var efi32File = Settings.ProxyEfi32File;
                var efi64File = Settings.ProxyEfi64File;
                proxyEditor.Visible = true;
                if (ddlEditProxyType.Text == "bios")
                {
                     if (biosFile.Contains("winpe"))
                    {
                        btnSaveEditor.Visible = false;
                        lblFileName1.Text = "Bios Boot Menus Are Not Used With WinPE";
                       
                        srvEdit.Visible = false;
                        return;
                    }
                    else if (biosFile.Contains("ipxe"))
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
                    if (efi32File.Contains("winpe"))
                    {
                        btnSaveEditor.Visible = false;
                        lblFileName1.Text = "Efi32 Boot Menus Are Not Used With WinPE";
                      
                        srvEdit.Visible = false;
                        return;
                    }
                    else if (efi32File.Contains("ipxe"))
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
                    if (efi64File.Contains("winpe"))
                    {
                        btnSaveEditor.Visible = false;
                        lblFileName1.Text = "Efi64 Boot Menus Are Not Used With WinPE";
                       
                        srvEdit.Visible = false;
                        return;
                    }
                    else if (efi64File.Contains("ipxe"))
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
            }
        }

      

        protected void saveEditor_Click(object sender, EventArgs e)
        {
            RequiresAuthorization(Authorizations.UpdateAdmin);
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
                    Call.FilesystemApi.SetUnixPermissions(path);
                }
                EndUserMessage = "Successfully Updated Default Global Boot Menu";
            }

            catch (Exception ex)
            {
                
                    EndUserMessage = "Could Not Update Default Global Boot Menu.  Check The Exception Log For More Info.";
                Logger.Log(ex.Message);
            }
        }

        protected void PopulateForm()
        {
            string path = null;
            var tftpPath = Settings.TftpPath;
            var mode = Settings.PxeMode;
            var proxyDhcp = Settings.ProxyDhcp;

            if (proxyDhcp == "Yes")
            {
                btnSaveEditor.Visible = true;
               
                srvEdit.Visible = true;

                var biosFile = Settings.ProxyBiosFile;
                var efi32File = Settings.ProxyEfi32File;
                var efi64File = Settings.ProxyEfi64File;
                proxyEditor.Visible = true;
                if (ddlEditProxyType.Text == "bios")
                {

                    if (biosFile.Contains("winpe"))
                    {
                        btnSaveEditor.Visible = false;
                        lblFileName1.Text = "Bios Boot Menus Are Not Used With WinPE";
                       
                        srvEdit.Visible = false;
                        return;
                    }
                    else if (biosFile.Contains("ipxe"))
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
                    if (efi32File.Contains("winpe"))
                    {
                        btnSaveEditor.Visible = false;
                        lblFileName1.Text = "Efi32 Boot Menus Are Not Used With WinPE";
                       
                        srvEdit.Visible = false;
                        return;
                    }
                    else if (efi32File.Contains("ipxe"))
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
                    if (efi64File.Contains("winpe"))
                    {
                        btnSaveEditor.Visible = false;
                        lblFileName1.Text = "Efi64 Boot Menus Are Not Used With WinPE";
                        
                        srvEdit.Visible = false;
                        return;
                    }
                    else if (efi64File.Contains("ipxe"))
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
                if (mode.Contains("winpe"))
                {
                    btnSaveEditor.Visible = false;
                    lblFileName1.Text = "Boot Menus Are Not Used With WinPE";
                  
                    srvEdit.Visible = false;
                    return;
                }
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
            }
        }

     
    
}