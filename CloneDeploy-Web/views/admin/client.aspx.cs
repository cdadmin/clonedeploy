using System;
using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

namespace CloneDeploy_Web.views.admin
{
    public partial class Client : Admin
    {
   
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
     
            txtQSize.Text = Settings.QueueSize;        
            txtGlobalComputerArgs.Text = Settings.GlobalComputerArgs;
            //These require pxe boot menu or client iso to be recreated
            ViewState["globalArgs"] = txtGlobalComputerArgs.Text;
        }

        protected void btnUpdateSettings_OnClick(object sender, EventArgs e)
        {
            RequiresAuthorization(Authorizations.UpdateAdmin);

            List<SettingEntity> listSettings = new List<SettingEntity>
            {
                new SettingEntity {Name = "Queue Size", Value = txtQSize.Text, Id = Call.SettingApi.GetSetting("Queue Size").Id},
                new SettingEntity()
                {
                    Name = "Global Computer Args",
                    Value = txtGlobalComputerArgs.Text,
                    Id = Call.SettingApi.GetSetting("Global Computer Args").Id
                }
            };


            EndUserMessage = Call.SettingApi.UpdateSettings(listSettings)
                ? "Successfully Updated Settings"
                : "Could Not Update Settings";
            if ((string) ViewState["globalArgs"] != txtGlobalComputerArgs.Text)
            {
                lblTitle.Text =
                    "Your Settings Changes Require A New PXE Boot File Be Created.  <br>Go There Now?";

                ClientScript.RegisterStartupScript(GetType(), "modalscript",
                    "$(function() {  var menuTop = document.getElementById('confirmbox'),body = document.body;classie.toggle(menuTop, 'confirm-box-outer-open'); });",
                    true);
                Session.Remove("Message");
            }

        }

        protected void OkButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/views/admin/bootmenu/defaultmenu.aspx?level=2");
        }
    }
}