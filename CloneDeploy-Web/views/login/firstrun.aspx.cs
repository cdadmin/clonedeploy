using System;
using System.Collections.Generic;
using System.Diagnostics;

using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Web;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;


public partial class views_login_firstrun : PageBaseMaster
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(Call.CdVersionApi.IsFirstRunCompleted())
            Response.Redirect("~/views/dashboard/dash.aspx");
    }

    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {
        string unixDist = null;
        if (ValidateForm())
        {
            var adminUser = Call.CloneDeployUserApi.GetByName("clonedeploy");
            adminUser.Salt = Utility.CreateSalt(64);
            adminUser.Password = Utility.CreatePasswordHash(txtUserPwd.Text, adminUser.Salt);
            adminUser.Token = Utility.GenerateKey();
            Call.CloneDeployUserApi.Put(adminUser.Id,adminUser);

            string tftpPath = null;
            if (Environment.OSVersion.ToString().Contains("Unix"))
            {
                string dist = null;
                var distInfo = new ProcessStartInfo
                {
                    UseShellExecute = false,
                    FileName = "uname",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                using (var process = Process.Start(distInfo))
                    if (process != null) dist = process.StandardOutput.ReadToEnd();

                unixDist = dist != null && dist.ToLower().Contains("bsd") ? "bsd" : "linux";
                tftpPath = unixDist == "bsd" ? "/usr/pbi/clonedeploy-amd64/tftpboot/" : "/tftpboot/";

            }
            else
            {
                tftpPath = Call.FilesystemApi.GetServerPaths("defaultTftp", "");
            }

            var physicalPath = "";
            if (Environment.OSVersion.ToString().Contains("Unix"))
                physicalPath = unixDist == "bsd" ? "/usr/pbi/clonedeploy-amd64/cd_dp" : "/cd_dp";
            else
            {
                physicalPath = Call.FilesystemApi.GetServerPaths("defaultDp", "");
            }

            var listSettings = new List<SettingEntity>
            {
                new SettingEntity()
                {
                    Name = "Server IP",
                    Value = txtServerIP.Text,
                    Id = Call.SettingApi.GetSetting("Server IP").Id
                },
                new SettingEntity()
                {
                    Name = "Tftp Path",
                    Value = tftpPath,
                    Id = Call.SettingApi.GetSetting("Tftp Path").Id
                },
                new SettingEntity
                {
                    Name = "Image Share Type",
                    Value = "Local",
                    Id = Call.SettingApi.GetSetting("Image Share Type").Id
                },
                new SettingEntity
                {
                    Name = "Image Server Ip",
                    Value = "[server-ip]",
                    Id = Call.SettingApi.GetSetting("Image Server Ip").Id
                },
                new SettingEntity
                {
                    Name = "Image Share Name",
                    Value = "cd_share",
                    Id = Call.SettingApi.GetSetting("Image Share Name").Id
                },
                new SettingEntity
                {
                    Name = "Image Domain",
                    Value = "Workgroup",
                    Id = Call.SettingApi.GetSetting("Image Domain").Id
                },
                new SettingEntity
                {
                    Name = "Image ReadWrite Username",
                    Value = "cd_share_rw",
                    Id = Call.SettingApi.GetSetting("Image ReadWrite Username").Id
                },
                new SettingEntity
                {
                    Name = "Image ReadWrite Password Encrypted",
                    Value = txtReadWrite.Text,
                    Id = Call.SettingApi.GetSetting("Image ReadWrite Password Encrypted").Id
                },
                new SettingEntity
                {
                    Name = "Image ReadOnly Username",
                    Value = "cd_share_ro",
                    Id = Call.SettingApi.GetSetting("Image ReadOnly Username").Id
                },
                new SettingEntity
                {
                    Name = "Image ReadOnly Password Encrypted",
                    Value = txtReadOnly.Text,
                    Id = Call.SettingApi.GetSetting("Image ReadOnly Password Encrypted").Id
                },
                 new SettingEntity
                {
                    Name = "Image Physical Path",
                    Value = physicalPath,
                    Id = Call.SettingApi.GetSetting("Image Physical Path").Id
                },
                 new SettingEntity
                {
                    Name = "Image Queue Size",
                    Value = "3",
                    Id = Call.SettingApi.GetSetting("Image Queue Size").Id
                },
            };

            if (unixDist == "bsd")
            {
                listSettings.Add(new SettingEntity()
                {
                    Name = "Sender Args",
                    Value = "--interface " + txtServerIP.Text,
                    Id = Call.SettingApi.GetSetting("Sender Args").Id
                });
            }


            Call.SettingApi.UpdateSettings(listSettings);

            var defaultBootMenuOptions = new BootMenuGenOptionsDTO();
            defaultBootMenuOptions.Kernel = Settings.DefaultKernel32;
            defaultBootMenuOptions.BootImage = "initrd.xz";
            defaultBootMenuOptions.Type = "standard";
            Call.WorkflowApi.CreateDefaultBootMenu(defaultBootMenuOptions);

            var cdVersion = Call.CdVersionApi.Get(1);
            cdVersion.FirstRunCompleted = 1;

            if (Call.CdVersionApi.Put(cdVersion.Id,cdVersion).Success)
            {
                EndUserMessage = "Setup Complete";
                Response.Redirect("~/views/dashboard/dash.aspx");
            }
        }
    }
    

    private bool ValidateForm()
    {
        if(string.IsNullOrEmpty(txtUserPwd.Text) || string.IsNullOrEmpty(txtUserPwdConfirm.Text) || string.IsNullOrEmpty(txtServerIP.Text) || string.IsNullOrEmpty(txtReadOnly.Text) || string.IsNullOrEmpty(txtReadWrite.Text))
        {
            EndUserMessage = "Values Cannot Be Empty";
            return false;
        }

        if (!string.IsNullOrEmpty(txtUserPwd.Text))
        {
            if (txtUserPwd.Text == txtUserPwdConfirm.Text)
            {
                return true;
            }
            else
            {
                EndUserMessage = "Admin Passwords Did Not Match";
                return false;
            }
        }
        return true;
    }
}