using System;
using System.Collections.Generic;
using System.Drawing;
using BasePages;
using BLL;
using Helpers;

public partial class views_admin_client : Admin
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
     
        txtQSize.Text = Settings.QueueSize;        
        txtGlobalComputerArgs.Text = Settings.GlobalComputerArgs;  
    }

    protected void btnUpdateSettings_OnClick(object sender, EventArgs e)
    {
        if (ValidateSettings())
        {
            List<Models.Setting> listSettings = new List<Models.Setting>
            {              
                    new Models.Setting {Name = "Queue Size", Value = txtQSize.Text, Id = Setting.GetSetting("Queue Size").Id},
                    new Models.Setting {Name = "Global Computer Args", Value = txtGlobalComputerArgs.Text, Id = Setting.GetSetting("Global Computer Args").Id}                
            };


            EndUserMessage = Setting.UpdateSetting(listSettings) ? "Successfully Updated Settings" : "Could Not Update Settings";
        }
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