using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using BasePages;
using Helpers;
using Models;
using ActiveImagingTask = BLL.ActiveImagingTask;

public partial class views_admin_security : Admin
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;

        chkldap.Checked = Settings.LdapEnabled == "1";
        if (chkldap.Checked)
        {
            ad.Visible = true;
            txtldapServer.Text = Settings.LdapServer;
            txtldapPort.Text = Settings.LdapPort;
            txtldapAuthAttribute.Text = Settings.LdapAuthAttribute;
            txtldapbasedn.Text = Settings.LdapBaseDN;
            ddlldapAuthType.Text = Settings.LdapAuthType;
        }
        ddlOnd.SelectedValue = Settings.OnDemand;
        txtToken.Text = Settings.UniversalToken;
        ddlSSL.SelectedValue = Settings.ForceSsL;
        chkImageApproval.Checked = Convert.ToBoolean(Settings.RequireImageApproval);
        ddlWebTasksLogin.Text = Settings.WebTaskRequiresLogin;
        ddlOndLogin.Text = Settings.OnDemandRequiresLogin;
        ddlDebugLogin.Text = Settings.DebugRequiresLogin;
        ddlRegisterLogin.Text = Settings.RegisterRequiresLogin;
        ddlClobberLogin.Text = Settings.ClobberRequiresLogin;

        if (ddlDebugLogin.Text == "No" || ddlOndLogin.Text == "No" || ddlRegisterLogin.Text == "No" ||
            ddlWebTasksLogin.Text == "No")
            universal.Visible = true;

        //These require pxe boot menu or client iso to be recreated 
        ViewState["serverKey"] = txtToken.Text;
        ViewState["forceSSL"] = ddlSSL.Text;
        ViewState["debugLogin"] = ddlDebugLogin.Text;
        ViewState["ondLogin"] = ddlOndLogin.Text;
        ViewState["registerLogin"] = ddlRegisterLogin.Text;
        ViewState["webTaskLogin"] = ddlWebTasksLogin.Text;
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        txtToken.Text = Utility.GenerateKey();
    }

    protected void btnUpdateSettings_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.UpdateAdmin);
        if (ddlDebugLogin.Text == "Yes" && ddlOndLogin.Text == "Yes" && ddlRegisterLogin.Text == "Yes" &&
            ddlWebTasksLogin.Text == "Yes")
            txtToken.Text = "";
        var listSettings = new List<Setting>
        {
           
            new Setting
            {
                Name = "Require Image Approval",
                Value = chkImageApproval.Checked.ToString(),
                Id = BLL.Setting.GetSetting("Require Image Approval").Id
            },
            new Setting {Name = "On Demand", Value = ddlOnd.Text, Id = BLL.Setting.GetSetting("On Demand").Id},
            new Setting {Name = "Universal Token", Value = txtToken.Text, Id = BLL.Setting.GetSetting("Universal Token").Id},
            new Setting {Name = "Force SSL", Value = ddlSSL.Text, Id = BLL.Setting.GetSetting("Force SSL").Id},
            new Setting {Name = "Ldap Enabled", Value = Convert.ToInt16(chkldap.Checked).ToString(), Id = BLL.Setting.GetSetting("Ldap Enabled").Id},
            new Setting {Name = "Ldap Server", Value = txtldapServer.Text, Id = BLL.Setting.GetSetting("Ldap Server").Id},
            new Setting {Name = "Ldap Port", Value = txtldapPort.Text, Id = BLL.Setting.GetSetting("Ldap Port").Id},
            new Setting {Name = "Ldap Auth Attribute", Value = txtldapAuthAttribute.Text, Id = BLL.Setting.GetSetting("Ldap Auth Attribute").Id},
            new Setting {Name = "Ldap Base DN", Value = txtldapbasedn.Text, Id = BLL.Setting.GetSetting("Ldap Base DN").Id},
            new Setting {Name = "Ldap Auth Type", Value = ddlldapAuthType.Text, Id = BLL.Setting.GetSetting("Ldap Auth Type").Id},
            new Setting
            {
                Name = "Web Task Requires Login",
                Value = ddlWebTasksLogin.Text,
                Id = BLL.Setting.GetSetting("Web Task Requires Login").Id
            }
        };


       
            listSettings.Add(new Setting
            {
                Name = "On Demand Requires Login",
                Value = ddlOndLogin.Text,
                Id = BLL.Setting.GetSetting("On Demand Requires Login").Id
            });
            listSettings.Add(new Setting
            {
                Name = "Debug Requires Login",
                Value = ddlDebugLogin.Text,
                Id = BLL.Setting.GetSetting("Debug Requires Login").Id
            });
            listSettings.Add(new Setting
            {
                Name = "Register Requires Login",
                Value = ddlRegisterLogin.Text,
                Id = BLL.Setting.GetSetting("Register Requires Login").Id
            });
            listSettings.Add(new Setting
            {
                Name = "Clobber Requires Login",
                Value = ddlClobberLogin.Text,
                Id = BLL.Setting.GetSetting("Clobber Requires Login").Id
            });
        


        var newBootMenu = false;
        var newClientIso = false;
        if (BLL.Setting.UpdateSetting(listSettings))
        {
            EndUserMessage = "Successfully Updated Settings";
            if ((string) ViewState["serverKey"] != txtToken.Text)
            {
                newBootMenu = true;
                newClientIso = true;
            }

            if ((string) ViewState["debugLogin"] != ddlDebugLogin.Text)
            {
                newBootMenu = true;
                newClientIso = true;
            }
            if ((string) ViewState["ondLogin"] != ddlOndLogin.Text)
            {
                newBootMenu = true;
                newClientIso = true;
            }
            if ((string) ViewState["registerLogin"] != ddlRegisterLogin.Text)
            {
                newBootMenu = true;
                newClientIso = true;
            }
            if ((string) ViewState["webTaskLogin"] != ddlWebTasksLogin.Text)
            {
                newBootMenu = true;
                newClientIso = true;
            }

            if ((string) ViewState["forceSSL"] != ddlSSL.Text)
            {
                newBootMenu = true;
                newClientIso = true;
                var webService = Settings.WebPath;
                string updatedWebService = null;
                if (ddlSSL.Text == "Yes")
                {
                    if (webService.ToLower().Contains("http://"))
                        updatedWebService = webService.Replace("http://", "https://");
                }
                else
                {
                    if (webService.ToLower().Contains("https://"))
                        updatedWebService = webService.Replace("https://", "http://");
                }
                var sslSettingList = new List<Setting>
                {
                    new Setting
                    {
                        Name = "Web Path",
                        Value = updatedWebService,
                        Id = BLL.Setting.GetSetting("Web Path").Id
                    }
                };
                BLL.Setting.UpdateSetting(sslSettingList);
            }
        }
        else
        {
            EndUserMessage = "Could Not Update Settings";
        }


        if (!newBootMenu) return;

        lblTitle.Text =
            "Your Settings Changes Require A New PXE Boot File Be Created.  <br>Create It Now?";
        if (newClientIso)
        {
            lblClientISO.Text = "The Client ISO Must Also Be Updated.";
        }
        ClientScript.RegisterStartupScript(GetType(), "modalscript",
            "$(function() {  var menuTop = document.getElementById('confirmbox'),body = document.body;classie.toggle(menuTop, 'confirm-box-outer-open'); });",
            true);
        Session.Remove("Message");
    }



    protected void OkButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/views/admin/bootmenu/defaultmenu.aspx?level=2");
    }

    protected void LoginsChanged(object sender, EventArgs e)
    {
        var ddl = sender as DropDownList;
        if (ddlDebugLogin.Text == "No" || ddlOndLogin.Text == "No" || ddlRegisterLogin.Text == "No" ||
            ddlWebTasksLogin.Text == "No" || ddlClobberLogin.Text == "No")
        {
            universal.Visible = true;
            if (ddl != null && ddl.Text == "No")
            {
                lblDiscouraged.Text =
                    "This Is Highly Discouraged Unless You Are Operating In An Isolated Network.  The Universal Token Is Stored In Plain Text In All PXE Boot Files";
                Page.ClientScript.RegisterStartupScript(GetType(), "modalscript",
                    "$(function() {  var menuTop = document.getElementById('discouraged'),body = document.body;classie.toggle(menuTop, 'confirm-box-outer-open'); });",
                    true);
            }
        }
        else
        {
            universal.Visible = false;
        }
    }

    protected void chkldap_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chkldap.Checked)
        {
            ad.Visible = true;
        }
        else
        {
            ad.Visible = false;
        }
    }
}