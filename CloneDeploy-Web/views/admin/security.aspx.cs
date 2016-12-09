using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using CloneDeploy_Entities;
using CloneDeploy_Web;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

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
            ddlWebTasksLogin.Text == "No" || ddlClobberLogin.Text == "No")
            universal.Visible = true;

        //These require pxe boot menu or client iso to be recreated 
        ViewState["serverKey"] = txtToken.Text;
        ViewState["forceSSL"] = ddlSSL.Text;
        ViewState["debugLogin"] = ddlDebugLogin.Text;
        ViewState["ondLogin"] = ddlOndLogin.Text;
        ViewState["registerLogin"] = ddlRegisterLogin.Text;
        ViewState["webTaskLogin"] = ddlWebTasksLogin.Text;
        ViewState["clobberLogin"] = ddlClobberLogin.Text;
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        txtToken.Text = Utility.GenerateKey();
    }

    protected void btnUpdateSettings_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.UpdateAdmin);
        if (ddlDebugLogin.Text == "Yes" && ddlOndLogin.Text == "Yes" && ddlRegisterLogin.Text == "Yes" &&
            ddlWebTasksLogin.Text == "Yes" && ddlClobberLogin.Text == "Yes")
            txtToken.Text = "";
        var listSettings = new List<SettingEntity>
        {
           
            new SettingEntity
            {
                Name = "Require Image Approval",
                Value = chkImageApproval.Checked.ToString(),
                Id = Call.SettingApi.GetSetting("Require Image Approval").Id
            },
            new SettingEntity {Name = "On Demand", Value = ddlOnd.Text, Id = Call.SettingApi.GetSetting("On Demand").Id},
            new SettingEntity {Name = "Universal Token", Value = txtToken.Text, Id = Call.SettingApi.GetSetting("Universal Token").Id},
            new SettingEntity {Name = "Force SSL", Value = ddlSSL.Text, Id = Call.SettingApi.GetSetting("Force SSL").Id},
            new SettingEntity {Name = "Ldap Enabled", Value = Convert.ToInt16(chkldap.Checked).ToString(), Id = Call.SettingApi.GetSetting("Ldap Enabled").Id},
            new SettingEntity {Name = "Ldap Server", Value = txtldapServer.Text, Id = Call.SettingApi.GetSetting("Ldap Server").Id},
            new SettingEntity {Name = "Ldap Port", Value = txtldapPort.Text, Id = Call.SettingApi.GetSetting("Ldap Port").Id},
            new SettingEntity {Name = "Ldap Auth Attribute", Value = txtldapAuthAttribute.Text, Id = Call.SettingApi.GetSetting("Ldap Auth Attribute").Id},
            new SettingEntity {Name = "Ldap Base DN", Value = txtldapbasedn.Text, Id = Call.SettingApi.GetSetting("Ldap Base DN").Id},
            new SettingEntity {Name = "Ldap Auth Type", Value = ddlldapAuthType.Text, Id = Call.SettingApi.GetSetting("Ldap Auth Type").Id},
            new SettingEntity
            {
                Name = "Web Task Requires Login",
                Value = ddlWebTasksLogin.Text,
                Id = Call.SettingApi.GetSetting("Web Task Requires Login").Id
            }
        };



        listSettings.Add(new SettingEntity
            {
                Name = "On Demand Requires Login",
                Value = ddlOndLogin.Text,
                Id = Call.SettingApi.GetSetting("On Demand Requires Login").Id
            });
        listSettings.Add(new SettingEntity
            {
                Name = "Debug Requires Login",
                Value = ddlDebugLogin.Text,
                Id = Call.SettingApi.GetSetting("Debug Requires Login").Id
            });
        listSettings.Add(new SettingEntity
            {
                Name = "Register Requires Login",
                Value = ddlRegisterLogin.Text,
                Id = Call.SettingApi.GetSetting("Register Requires Login").Id
            });
        listSettings.Add(new SettingEntity
            {
                Name = "Clobber Requires Login",
                Value = ddlClobberLogin.Text,
                Id = Call.SettingApi.GetSetting("Clobber Requires Login").Id
            });
        


        var newBootMenu = false;
        var newClientIso = false;
        if (Call.SettingApi.UpdateSettings(listSettings))
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
            if ((string)ViewState["clobberLogin"] != ddlClobberLogin.Text)
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
                var sslSettingList = new List<SettingEntity>
                {
                    new SettingEntity
                    {
                        Name = "Web Path",
                        Value = updatedWebService,
                        Id = Call.SettingApi.GetSetting("Web Path").Id
                    }
                };
                Call.SettingApi.UpdateSettings(sslSettingList);
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
                    "This Is Highly Discouraged Unless You Are Operating In An Isolated Network. <br> The Universal Token Is Stored In Plain Text In All PXE Boot Files." +
                    "<br> Remember To Generate The Universal Token.<br><br>";
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