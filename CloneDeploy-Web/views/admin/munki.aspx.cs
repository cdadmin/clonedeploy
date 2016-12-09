using System;
using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Web;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

public partial class views_admin_munki : Admin
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
        List<SettingEntity> listSettings = new List<SettingEntity>
        {
            new SettingEntity {Name = "Munki Base Path", Value = txtBasePath.Text, Id = Call.SettingApi.GetSetting("Munki Base Path").Id},
            new SettingEntity {Name = "Munki Path Type", Value = ddlPathType.Text, Id = Call.SettingApi.GetSetting("Munki Path Type").Id},
            new SettingEntity {Name = "Munki SMB Username", Value = txtSmbUsername.Text, Id = Call.SettingApi.GetSetting("Munki SMB Username").Id},
            new SettingEntity {Name = "Munki SMB Domain", Value = txtDomain.Text, Id = Call.SettingApi.GetSetting("Munki SMB Domain").Id}
           

        };
        if (!string.IsNullOrEmpty(txtSmbPassword.Text))
            listSettings.Add(new SettingEntity { Name = "Munki SMB Password Encrypted", Value = txtSmbPassword.Text, Id = Call.SettingApi.GetSetting("Munki SMB Password Encrypted").Id });



        EndUserMessage = Call.SettingApi.UpdateSettings(listSettings) ? "Successfully Updated Settings" : "Could Not Update Settings";
    }
}