using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Global;
using Models;

public partial class views_admin_security : System.Web.UI.Page
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
        if (ValidateSettings())
        {
            List<Setting> listSettings = new List<Setting>
            {
               
                    new Setting {Name = "AD Login Domain", Value = txtADLogin.Text},
                   
                    new Setting {Name = "Image Checksum", Value = ddlImageChecksum.Text},
                  
                    new Setting {Name = "On Demand", Value = ddlOnd.Text},
                   
                    new Setting {Name = "Server Key", Value = txtServerKey.Text},
                  
                    new Setting {Name = "Force SSL", Value = ddlSSL.Text},
                   
                    new Setting {Name = "Web Task Requires Login", Value = ddlWebTasksLogin.Text}
                   
            };

        


            if (ddlWebTasksLogin.Text == "Yes")
            {
                listSettings.Add(new Setting { Name = "On Demand Requires Login", Value = "Yes" });
                listSettings.Add(new Setting { Name = "Debug Requires Login", Value = "Yes" });
                listSettings.Add(new Setting { Name = "Register Requires Login", Value = "Yes" });
            }
            else
            {
                listSettings.Add(new Setting { Name = "On Demand Requires Login", Value = ddlOndLogin.Text });
                listSettings.Add(new Setting { Name = "Debug Requires Login", Value = ddlDebugLogin.Text });
                listSettings.Add(new Setting { Name = "Register Requires Login", Value = ddlRegisterLogin.Text });
            }


            var newBootMenu = false;
            var newClientIso = false;
            if (new BLL.Setting().UpdateSetting(listSettings))
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

            if (newBootMenu)
            {
                var confirmTitle = Master.Master.FindControl("Content3").FindControl("lblTitle") as Label;
                confirmTitle.Text = Utility.Message;
                confirmTitle.Text +=
                    "<br> Your Settings Changes Require A New PXE Boot File Be Created.  <br>Create It Now?";
                if (newClientIso)
                {
                    var isoTitle = Master.Master.FindControl("Content3").FindControl("lblClientISO") as Label;
                    isoTitle.Text = "If You Are Using The Client ISO, It Must Also Be Manually Updated.";
                }
                ClientScript.RegisterStartupScript(GetType(), "modalscript",
                    "$(function() {  var menuTop = document.getElementById('confirmbox'),body = document.body;classie.toggle(menuTop, 'confirm-box-outer-open'); });",
                    true);
                Session.Remove("Message");
            }



        }
        new Utility().Msgbox(Utility.Message);

    }
    protected bool ValidateSettings()
    {
        if (new BLL.ActiveImagingTask().ReadAll().Count > 0)
        {
            Utility.Message = "Settings Cannot Be Changed While Tasks Are Active";
            return false;
        }
      
        return true;
    }
}