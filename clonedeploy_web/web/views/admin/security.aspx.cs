using System;
using System.Collections.Generic;
using BasePages;
using Helpers;
using Models;
using ActiveImagingTask = BLL.ActiveImagingTask;

public partial class views_admin_security : Admin
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
      
        txtADLogin.Text = Settings.AdLoginDomain;
      
        ddlImageChecksum.SelectedValue = Settings.ImageChecksum;
       
        ddlOnd.SelectedValue = Settings.OnDemand;
      
        txtServerKey.Text = Settings.ServerKey;
      
        ddlSSL.SelectedValue = Settings.ForceSsL;
        
        ddlWebTasksLogin.Text = Settings.WebTaskRequiresLogin;
        ddlOndLogin.Text = Settings.OnDemandRequiresLogin;
        ddlDebugLogin.Text = Settings.DebugRequiresLogin;
        ddlRegisterLogin.Text = Settings.RegisterRequiresLogin;

        //These require pxe boot menu or client iso to be recreated 
        ViewState["serverKey"] = txtServerKey.Text;    
        ViewState["forceSSL"] = ddlSSL.Text;  
        ViewState["webTaskRequiresLogin"] = ddlWebTasksLogin.SelectedValue;
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        txtServerKey.Text = Utility.GenerateKey();
    }
    protected void btnUpdateSettings_OnClick(object sender, EventArgs e)
    {
        if (!ValidateSettings()) return;
        var listSettings = new List<Setting>
        {
               
            new Setting {Name = "AD Login Domain", Value = txtADLogin.Text, Id = BLL.Setting.GetSetting("AD Login Domain").Id},
                   
            new Setting {Name = "Image Checksum", Value = ddlImageChecksum.Text, Id = BLL.Setting.GetSetting("Image Checksum").Id},
                  
            new Setting {Name = "Require Image Approval", Value = chkImageApproval.Checked.ToString(), Id = BLL.Setting.GetSetting("Require Image Approval").Id},

            new Setting {Name = "On Demand", Value = ddlOnd.Text, Id = BLL.Setting.GetSetting("On Demand").Id},
                   
            new Setting {Name = "Server Key", Value = txtServerKey.Text, Id = BLL.Setting.GetSetting("Server Key").Id},
                  
            new Setting {Name = "Force SSL", Value = ddlSSL.Text, Id = BLL.Setting.GetSetting("Force SSL").Id},
                   
            new Setting {Name = "Web Task Requires Login", Value = ddlWebTasksLogin.Text, Id = BLL.Setting.GetSetting("Web Task Requires Login").Id}
                   
        };

        


        if (ddlWebTasksLogin.Text == "Yes")
        {
            listSettings.Add(new Setting { Name = "On Demand Requires Login", Value = "Yes", Id = BLL.Setting.GetSetting("On Demand Requires Login").Id });
            listSettings.Add(new Setting { Name = "Debug Requires Login", Value = "Yes", Id = BLL.Setting.GetSetting("Debug Requires Login").Id } );
            listSettings.Add(new Setting { Name = "Register Requires Login", Value = "Yes", Id = BLL.Setting.GetSetting("Register Requires Login").Id } );
        }
        else
        {
            listSettings.Add(new Setting { Name = "On Demand Requires Login", Value = ddlOndLogin.Text, Id = BLL.Setting.GetSetting("On Demand Requires Login").Id });
            listSettings.Add(new Setting { Name = "Debug Requires Login", Value = ddlDebugLogin.Text, Id = BLL.Setting.GetSetting("Debug Requires Login").Id });
            listSettings.Add(new Setting { Name = "Register Requires Login", Value = ddlRegisterLogin.Text, Id = BLL.Setting.GetSetting("Register Requires Login").Id });
        }


        var newBootMenu = false;
        var newClientIso = false;
        if (BLL.Setting.UpdateSetting(listSettings))
        {

            if ((string)ViewState["webTaskRequiresLogin"] != ddlWebTasksLogin.Text)
                newBootMenu = true;
              
            if ((string)ViewState["serverKey"] != txtServerKey.Text)
            {
                newBootMenu = true;
                newClientIso = true;
            }

            if ((string)ViewState["forceSSL"] != ddlSSL.Text)
            {
                newBootMenu = true;
                newClientIso = true;
            }

        }

        if (!newBootMenu) return;
        lblTitle.Text = EndUserMessage;
        lblTitle.Text +=
            "<br> Your Settings Changes Require A New PXE Boot File Be Created.  <br>Create It Now?";
        if (newClientIso)
        {
            lblClientISO.Text = "If You Are Using The Client ISO, It Must Also Be Manually Updated.";
        }
        ClientScript.RegisterStartupScript(GetType(), "modalscript",
            "$(function() {  var menuTop = document.getElementById('confirmbox'),body = document.body;classie.toggle(menuTop, 'confirm-box-outer-open'); });",
            true);
        Session.Remove("Message");
    }

    protected void OkButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/views/admin/bootmenu.aspx?defaultmenu=true");
    }

    protected bool ValidateSettings()
    {
        if (ActiveImagingTask.ReadAll().Count > 0)
        {
            EndUserMessage = "Settings Cannot Be Changed While Tasks Are Active";
            return false;
        }
      
        return true;
    }
}