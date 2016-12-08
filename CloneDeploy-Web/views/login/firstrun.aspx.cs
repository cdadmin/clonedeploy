using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using BasePages;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Web;



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
                tftpPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) +
                Path.DirectorySeparatorChar + "clonedeploy" +
                Path.DirectorySeparatorChar + "tftpboot" + Path.DirectorySeparatorChar;
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
                }           
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

            var distributionPoint = new DistributionPointEntity();
            distributionPoint.DisplayName = "Default";
            distributionPoint.Server = "[server-ip]";
            distributionPoint.Protocol = "SMB";
            distributionPoint.ShareName = "cd_share";
            distributionPoint.Domain = "Workgroup";
            distributionPoint.RwUsername = "cd_share_rw";
            distributionPoint.RwPassword = new Encryption().EncryptText(txtReadWrite.Text);
            distributionPoint.RoUsername = "cd_share_ro";
            distributionPoint.RoPassword = new Encryption().EncryptText(txtReadOnly.Text);
            distributionPoint.IsPrimary = 1;
            if (Environment.OSVersion.ToString().Contains("Unix"))
                distributionPoint.PhysicalPath = unixDist == "bsd" ? "/usr/pbi/clonedeploy-amd64/cd_dp" : "/cd_dp";
            else
            {
                distributionPoint.PhysicalPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) +
                                                 Path.DirectorySeparatorChar + "clonedeploy" +
                                                 Path.DirectorySeparatorChar + "cd_dp";
            }
            Call.DistributionPointApi.Post(distributionPoint);

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