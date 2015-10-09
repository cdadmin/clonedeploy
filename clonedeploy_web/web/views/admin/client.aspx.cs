using System;
using System.Collections.Generic;
using System.Drawing;
using BasePages;
using BLL;
using Helpers;

public partial class views_admin_client : Admin
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
      

        if (IsPostBack) return;
     
        txtQSize.Text = Settings.QueueSize;    
        txtNFSPath.Text = Setting.GetValueForAdminView(Settings.NfsUploadPath);   
        ddlCompAlg.SelectedValue = Settings.CompressionAlgorithm;
        ddlCompLevel.SelectedValue = Settings.CompressionLevel;   
        ddlImageXfer.SelectedValue = Settings.ImageTransferMode;  
        txtNFSDeploy.Text = Setting.GetValueForAdminView(Settings.NfsDeployPath);       
        txtSMBPath.Text = Setting.GetValueForAdminView(Settings.SmbPath);
        txtSMBUser.Text = Settings.SmbUserName;
        txtSMBPass.Text = Settings.SmbPassword;      
        txtGlobalHostArgs.Text = Settings.GlobalHostArgs;
    
        ShowXferMode();

    }

    protected void btnUpdateSettings_OnClick(object sender, EventArgs e)
    {
        if (ValidateSettings())
        {
            List<Models.Setting> listSettings = new List<Models.Setting>
            {
               
                    new Models.Setting {Name = "Queue Size", Value = txtQSize.Text},
                 
                    new Models.Setting {Name = "Nfs Upload Path", Value = txtNFSPath.Text},
                  
                    new Models.Setting {Name = "Compression Algorithm", Value = ddlCompAlg.Text},
                    new Models.Setting {Name = "Compression Level", Value = ddlCompLevel.Text},
                   
                    new Models.Setting {Name = "Image Transfer Mode", Value = ddlImageXfer.Text},
                  
                    new Models.Setting {Name = "Nfs Deploy Path", Value = txtNFSDeploy.Text},
                  
                    new Models.Setting {Name = "SMB Path", Value = txtSMBPath.Text},
                    new Models.Setting {Name = "SMB User Name", Value = txtSMBUser.Text},
                    
                    new Models.Setting {Name = "Global Host Args", Value = txtGlobalHostArgs.Text}
                  
            };

            if (!string.IsNullOrEmpty(txtSMBPass.Text))
                listSettings.Add(new Models.Setting { Name = "SMB Password", Value = txtSMBPass.Text });
            Setting.UpdateSetting(listSettings);
        }
    }

    protected void ImageXfer_Changed(object sender, EventArgs e)
    {
        ShowXferMode();
    }

    protected void ShowXferMode()
    {
        txtNFSDeploy.BackColor = Color.White;
        txtNFSDeploy.Font.Strikeout = false;
        txtNFSPath.BackColor = Color.White;
        txtNFSPath.Font.Strikeout = false;

        txtSMBPath.BackColor = Color.White;
        txtSMBPath.Font.Strikeout = false;
        txtSMBUser.BackColor = Color.White;
        txtSMBUser.Font.Strikeout = false;
        txtSMBPass.BackColor = Color.White;
        txtSMBPass.Font.Strikeout = false;

        if (ddlImageXfer.Text == "smb" || ddlImageXfer.Text == "smb+http")
        {
            txtNFSDeploy.BackColor = Color.LightGray;
            txtNFSDeploy.Font.Strikeout = true;
            txtNFSPath.BackColor = Color.LightGray;
            txtNFSPath.Font.Strikeout = true;
        }

        if (ddlImageXfer.Text != "smb" && ddlImageXfer.Text != "smb+http")
        {
            txtSMBPath.BackColor = Color.LightGray;
            txtSMBPath.Font.Strikeout = true;
            txtSMBUser.BackColor = Color.LightGray;
            txtSMBUser.Font.Strikeout = true;
            txtSMBPass.BackColor = Color.LightGray;
            txtSMBPass.Font.Strikeout = true;
        }

        if (ddlImageXfer.Text == "nfs+http")
        {
            txtNFSDeploy.BackColor = Color.LightGray;
            txtNFSDeploy.Font.Strikeout = true;
        }

        if (ddlImageXfer.Text == "udp+http")
        {
            txtNFSDeploy.BackColor = Color.LightGray;
            txtNFSDeploy.Font.Strikeout = true;
            txtNFSPath.BackColor = Color.LightGray;
            txtNFSPath.Font.Strikeout = true;

            txtSMBPath.BackColor = Color.LightGray;
            txtSMBPath.Font.Strikeout = true;
            txtSMBUser.BackColor = Color.LightGray;
            txtSMBUser.Font.Strikeout = true;
            txtSMBPass.BackColor = Color.LightGray;
            txtSMBPass.Font.Strikeout = true;
        }
    }

    protected bool ValidateSettings()
    {
        if (ActiveImagingTask.ReadAll().Count > 0)
        {
            //Message.Text = "Settings Cannot Be Changed While Tasks Are Active";
            return false;
        }

        if (!txtNFSPath.Text.Trim().EndsWith("/"))
            txtNFSPath.Text += "/";

        if (!txtNFSDeploy.Text.Trim().EndsWith("/"))
            txtNFSDeploy.Text += ("/");

        if (txtSMBPath.Text.Contains("\\"))
            txtSMBPath.Text = txtSMBPath.Text.Replace("\\", "/");

        return true;
    }
}