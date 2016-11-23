using System;
using System.Collections.Generic;
using CloneDeploy_Web.Models;
using Helpers;

public partial class views_admin_munki : BasePages.Admin
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;

        txtBasePath.Text = Settings.MunkiBasePath;
        txtSmbUsername.Text = Settings.MunkiSMBUsername;
        ddlPathType.Text = Settings.MunkiPathType;
        txtDomain.Text = Settings.MunkiSMBDomain;

    }

    protected void btnUpdateSettings_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.UpdateAdmin);
        List<Setting> listSettings = new List<Setting>
        {
            new Setting {Name = "Munki Base Path", Value = txtBasePath.Text, Id = BLL.Setting.GetSetting("Munki Base Path").Id},
            new Setting {Name = "Munki Path Type", Value = ddlPathType.Text, Id = BLL.Setting.GetSetting("Munki Path Type").Id},
            new Setting {Name = "Munki SMB Username", Value = txtSmbUsername.Text, Id = BLL.Setting.GetSetting("Munki SMB Username").Id},
            new Setting {Name = "Munki SMB Domain", Value = txtDomain.Text, Id = BLL.Setting.GetSetting("Munki SMB Domain").Id}
           

        };
        if (!string.IsNullOrEmpty(txtSmbPassword.Text))
            listSettings.Add(new Setting { Name = "Munki SMB Password Encrypted", Value = new Helpers.Encryption().EncryptText(txtSmbPassword.Text), Id = BLL.Setting.GetSetting("Munki SMB Password Encrypted").Id });

      

        EndUserMessage = BLL.Setting.UpdateSetting(listSettings) ? "Successfully Updated Settings" : "Could Not Update Settings";
    }
}