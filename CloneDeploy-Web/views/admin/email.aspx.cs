using System;
using System.Collections.Generic;
using BasePages;
using CloneDeploy_Web.Models;
using Helpers;

public partial class views_admin_email : Admin
{
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

    protected void btnUpdateSettings_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.UpdateAdmin);
        List<Setting> listSettings = new List<Setting>
        {
            new Setting {Name = "Smtp Server", Value = txtSmtpServer.Text, Id = BLL.Setting.GetSetting("Smtp Server").Id},
            new Setting {Name = "Smtp Port", Value = txtSmtpPort.Text, Id = BLL.Setting.GetSetting("Smtp Port").Id},
            new Setting {Name = "Smtp Username", Value = txtSmtpUsername.Text, Id = BLL.Setting.GetSetting("Smtp Username").Id},
            new Setting {Name = "Smtp Mail From", Value = txtSmtpFrom.Text, Id = BLL.Setting.GetSetting("Smtp Mail From").Id},
            new Setting {Name = "Smtp Mail To", Value = txtSmtpTo.Text, Id = BLL.Setting.GetSetting("Smtp Mail To").Id},
            new Setting {Name = "Smtp Ssl", Value = ddlSmtpSsl.Text, Id = BLL.Setting.GetSetting("Smtp Ssl").Id}

        };
        if (!string.IsNullOrEmpty(txtSmtpPassword.Text))
            listSettings.Add(new Setting { Name = "Smtp Password Encrypted", Value = new Helpers.Encryption().EncryptText(txtSmtpPassword.Text), Id = BLL.Setting.GetSetting("Smtp Password Encrypted").Id });

        var chkValue = chkEnabled.Checked ? "1" : "0";
        listSettings.Add(new Setting { Name = "Smtp Enabled", Value = chkValue, Id = BLL.Setting.GetSetting("Smtp Enabled").Id });

        EndUserMessage = BLL.Setting.UpdateSetting(listSettings) ? "Successfully Updated Settings" : "Could Not Update Settings";
    }

    protected void btnTestMessage_Click(object sender, EventArgs e)
    {
        var mail = new Mail
        {
            Subject = "Test Message",
            Body = "Email Notifications Are Working!",
            MailTo = Settings.SmtpMailTo
        };

        mail.Send();
        EndUserMessage = "Test Message Sent";
    }
}