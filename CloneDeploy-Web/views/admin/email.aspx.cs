using System;
using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

public partial class views_admin_email : Admin
{
    protected void btnTestMessage_Click(object sender, EventArgs e)
    {
        Call.SettingApi.SendEmailTest();
        EndUserMessage = "Test Message Sent";
    }

    protected void btnUpdateSettings_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.UpdateAdmin);
        var listSettings = new List<SettingEntity>
        {
            new SettingEntity
            {
                Name = "Smtp Server",
                Value = txtSmtpServer.Text,
                Id = Call.SettingApi.GetSetting("Smtp Server").Id
            },
            new SettingEntity
            {
                Name = "Smtp Port",
                Value = txtSmtpPort.Text,
                Id = Call.SettingApi.GetSetting("Smtp Port").Id
            },
            new SettingEntity
            {
                Name = "Smtp Username",
                Value = txtSmtpUsername.Text,
                Id = Call.SettingApi.GetSetting("Smtp Username").Id
            },
            new SettingEntity
            {
                Name = "Smtp Mail From",
                Value = txtSmtpFrom.Text,
                Id = Call.SettingApi.GetSetting("Smtp Mail From").Id
            },
            new SettingEntity
            {
                Name = "Smtp Mail To",
                Value = txtSmtpTo.Text,
                Id = Call.SettingApi.GetSetting("Smtp Mail To").Id
            },
            new SettingEntity
            {
                Name = "Smtp Ssl",
                Value = ddlSmtpSsl.Text,
                Id = Call.SettingApi.GetSetting("Smtp Ssl").Id
            }
        };
        if (!string.IsNullOrEmpty(txtSmtpPassword.Text))
            listSettings.Add(new SettingEntity
            {
                Name = "Smtp Password Encrypted",
                Value = txtSmtpPassword.Text,
                Id = Call.SettingApi.GetSetting("Smtp Password Encrypted").Id
            });

        var chkValue = chkEnabled.Checked ? "1" : "0";
        listSettings.Add(new SettingEntity
        {
            Name = "Smtp Enabled",
            Value = chkValue,
            Id = Call.SettingApi.GetSetting("Smtp Enabled").Id
        });

        EndUserMessage = Call.SettingApi.UpdateSettings(listSettings)
            ? "Successfully Updated Settings"
            : "Could Not Update Settings";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;

        txtSmtpServer.Text = Settings.SmtpServer;
        txtSmtpPort.Text = Settings.SmtpPort;
        ddlSmtpSsl.SelectedValue = Settings.SmtpSsl;
        txtSmtpUsername.Text = Settings.SmtpUsername;
        txtSmtpPassword.Text = Settings.SmtpPassword;
        txtSmtpFrom.Text = Settings.SmtpMailFrom;
        txtSmtpTo.Text = Settings.SmtpMailTo;

        if (Settings.SmtpEnabled == "1")
            chkEnabled.Checked = true;
    }
}