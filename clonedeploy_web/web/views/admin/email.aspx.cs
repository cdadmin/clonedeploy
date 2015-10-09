using System;
using System.Collections.Generic;
using System.Web;
using BasePages;
using Helpers;
using Models;

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

        if (Settings.NotifySuccessfulLogin == "1")
            chkLoginSuccess.Checked = true;
        if (Settings.NotifyFailedLogin == "1")
            chkLoginFailed.Checked = true;
        if (Settings.NotifyTaskStarted == "1")
            chkTaskStarted.Checked = true;
        if (Settings.NotifyTaskCompleted == "1")
            chkTaskCompleted.Checked = true;
        if (Settings.NotifyImageApproved == "1")
            chkImageApproved.Checked = true;
        if (Settings.NotifyResizeFailed == "1")
            chkResizeFailed.Checked = true;
    }

    protected void btnUpdateSettings_OnClick(object sender, EventArgs e)
    {
        List<Setting> listSettings = new List<Setting>
        {


            new Setting {Name = "Smtp Server", Value = txtSmtpServer.Text},
            new Setting {Name = "Smtp Port", Value = txtSmtpPort.Text},
            new Setting {Name = "Smtp Username", Value = txtSmtpUsername.Text},
            new Setting {Name = "Smtp Mail From", Value = txtSmtpFrom.Text},
            new Setting {Name = "Smtp Mail To", Value = txtSmtpTo.Text},
            new Setting {Name = "Smtp Ssl", Value = ddlSmtpSsl.Text}

        };
        if (!string.IsNullOrEmpty(txtSmtpPassword.Text))
            listSettings.Add(new Setting { Name = "Smtp Password", Value = txtSmtpPassword.Text });

        var chkValue = "0";
        chkValue = chkLoginSuccess.Checked ? "1" : "0";
        listSettings.Add(new Setting { Name = "Notify Successful Login", Value = chkValue });

        chkValue = chkLoginFailed.Checked ? "1" : "0";
        listSettings.Add(new Setting { Name = "Notify Failed Login", Value = chkValue });

        chkValue = chkTaskStarted.Checked ? "1" : "0";
        listSettings.Add(new Setting { Name = "Notify Task Started", Value = chkValue });

        chkValue = chkTaskCompleted.Checked ? "1" : "0";
        listSettings.Add(new Setting { Name = "Notify Task Completed", Value = chkValue });

        chkValue = chkImageApproved.Checked ? "1" : "0";
        listSettings.Add(new Setting { Name = "Notify Image Approved", Value = chkValue });

        chkValue = chkResizeFailed.Checked ? "1" : "0";
        listSettings.Add(new Setting { Name = "Notify Resize Failed", Value = chkValue });


     
        BLL.Setting.UpdateSetting(listSettings);
    }

    protected void btnTestMessage_Click(object sender, EventArgs e)
    {
        var mail = new Mail
        {
            Subject = "Test Message",
            Body = HttpContext.Current.User.Identity.Name
        };
        mail.Send("Test Message");

    }
}