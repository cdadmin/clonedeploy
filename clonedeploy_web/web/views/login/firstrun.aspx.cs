using System;
using System.Collections.Generic;
using System.IO;
using BasePages;
using BLL;
using Helpers;


public partial class views_login_firstrun : PageBaseMaster
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(BLL.CdVersion.FirstRunCompleted())
            Response.Redirect("~/views/dashboard/dash.aspx");
    }

    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {
        if (ValidateForm())
        {
            var adminUser = BLL.User.GetUser("clonedeploy");
            adminUser.Salt = Helpers.Utility.CreateSalt(64);
            adminUser.Password = Helpers.Utility.CreatePasswordHash(txtUserPwd.Text, adminUser.Salt);
            adminUser.Token = Utility.GenerateKey();
            BLL.User.UpdateUser(adminUser);

            string tftpPath = null;
            if (Environment.OSVersion.ToString().Contains("Unix"))
            {
                tftpPath = "/tftpboot/";
            }
            else
            {
                tftpPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) +
                Path.DirectorySeparatorChar + "clonedeploy" +
                Path.DirectorySeparatorChar + "tftpboot" + Path.DirectorySeparatorChar;
            }
            var listSettings = new List<Models.Setting>
            {
                new Models.Setting
                {
                    Name = "Server IP",
                    Value = txtServerIP.Text,
                    Id = Setting.GetSetting("Server IP").Id
                },
                new Models.Setting
                {
                    Name = "Tftp Path",
                    Value = tftpPath,
                    Id = Setting.GetSetting("Tftp Path").Id
                }

            };
            Setting.UpdateSetting(listSettings);

            var distributionPoint = new Models.DistributionPoint();
            distributionPoint.DisplayName = "Default";
            distributionPoint.Server = "[server-ip]";
            distributionPoint.Protocol = "SMB";
            distributionPoint.ShareName = "cd_share";
            distributionPoint.Domain = "Workgroup";
            distributionPoint.RwUsername = "cd_share_rw";
            distributionPoint.RwPassword = new Helpers.Encryption().EncryptText(txtReadWrite.Text);
            distributionPoint.RoUsername = "cd_share_ro";
            distributionPoint.RoPassword = new Helpers.Encryption().EncryptText(txtReadOnly.Text);
            distributionPoint.IsPrimary = 1;
            if (Environment.OSVersion.ToString().Contains("Unix"))
                distributionPoint.PhysicalPath = "/cd_dp";
            else
            {
                distributionPoint.PhysicalPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) +
                                                 Path.DirectorySeparatorChar + "clonedeploy" +
                                                 Path.DirectorySeparatorChar + "cd_dp";
            }
            BLL.DistributionPoint.AddDistributionPoint(distributionPoint);

            var defaultBootMenu = new BLL.Workflows.DefaultBootMenu();
            defaultBootMenu.Kernel = Settings.DefaultKernel32;
            defaultBootMenu.BootImage = "initrd.xz";
            defaultBootMenu.CreateGlobalDefaultBootMenu();

            var cdVersion = BLL.CdVersion.Get(1);
            cdVersion.FirstRunCompleted = 1;

            if (BLL.CdVersion.Update(cdVersion))
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