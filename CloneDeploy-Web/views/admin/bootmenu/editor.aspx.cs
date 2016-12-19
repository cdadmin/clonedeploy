using System;
using System.Net;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;
using log4net;

public partial class views_admin_bootmenu_editor : Admin
{
    private readonly ILog log = LogManager.GetLogger("FrontEndLog");
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
            string path = Call.FilesystemApi.GetDefaultBootFilePath(ddlEditProxyType.Text);
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
                   
                }
            }
            else
            {
                proxyEditor.Visible = false;
            }
            lblFileName1.Text = path;
            try
            {
                if (path != null) scriptEditorText.Value = Call.FilesystemApi.ReadFileText(path);
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
        }

      

        protected void saveEditor_Click(object sender, EventArgs e)
        {
            RequiresAuthorization(Authorizations.UpdateAdmin);

            EndUserMessage = Call.FilesystemApi.EditDefaultBootMenu(ddlEditProxyType.Text, scriptEditorText.Value) ? "Success" : "Could Not Save Boot Menu";
        }

    protected void PopulateForm()
    {
        string path = Call.FilesystemApi.GetDefaultBootFilePath(ddlEditProxyType.Text);
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

                }
                else if (mode.Contains("grub"))
                {
                    grubShaGen.Visible = true;
                    syslinuxShaGen.Visible = false;

                }
                else
                {
                    grubShaGen.Visible = false;
                    syslinuxShaGen.Visible = true;

                }
            }
            lblFileName1.Text = path;
            try
            {
                if (path != null) scriptEditorText.Value = Call.FilesystemApi.ReadFileText(path);
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
        }

    }

}