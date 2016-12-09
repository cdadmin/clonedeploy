using System;
using CloneDeploy_Entities;
using CloneDeploy_Web;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

public partial class views_computers_bootmenu_custom : Computers
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack) PopulateForm();
    }

    protected void PopulateForm()
    {
        chkEnabled.Checked = Computer.CustomBootEnabled == 1;
        PopulateBootTemplatesDdl(ddlTemplates);
        var bootMenu = Call.ComputerApi.GetBootMenu(Computer.Id);
        

        if (Settings.ProxyDhcp == "Yes")
        {
            divProxy.Visible = true;
            if(bootMenu == null) return;
            switch (ddlProxyMode.Text)
            {
                case "bios":
                    scriptEditor.Value = bootMenu.BiosMenu;
                    break;
                case "efi32":
                    scriptEditor.Value = bootMenu.Efi32Menu;
                    break;
                case "efi64":
                    scriptEditor.Value = bootMenu.Efi64Menu;
                    break;
            }
        }
        else
        {
            if(bootMenu == null) return;
            scriptEditor.Value = bootMenu.BiosMenu;
        }

        
    }

    protected void ddlTemplates_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        scriptEditor.Value = "";
        if (ddlTemplates.SelectedValue == "-1") return;
        scriptEditor.Value = Call.BootTemplateApi.Get(Convert.ToInt32(ddlTemplates.SelectedValue)).Contents;
    }

    protected void buttonUpdate_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorizationOrManagedComputer(Authorizations.UpdateComputer, Computer.Id);
        var bootMenu = Call.ComputerApi.GetBootMenu(Computer.Id) ?? new ComputerBootMenuEntity();
        bootMenu.ComputerId = Computer.Id;
        if (Settings.ProxyDhcp == "Yes")
        {
            switch (ddlProxyMode.Text)
            {
                case "bios":
                    bootMenu.BiosMenu = scriptEditor.Value.Replace("\r\n", "\n");
                    break;
                case "efi32":
                    bootMenu.Efi32Menu = scriptEditor.Value.Replace("\r\n", "\n");
                    break;
                case "efi64":
                    bootMenu.Efi64Menu = scriptEditor.Value.Replace("\r\n", "\n");
                    break;
            }
        }
        else
        {
            bootMenu.BiosMenu = scriptEditor.Value.Replace("\r\n", "\n");
        }
        EndUserMessage = Call.ComputerBootMenuApi.Post(bootMenu).Success
            ? "Successfully Updated Custom Boot Menu"
            : "Could Not Update Custom Boot Menu";

        if (chkEnabled.Checked)
            Call.ComputerApi.CreateCustomBootFiles(Computer.Id);
    }

    protected void chkEnabled_OnCheckedChanged(object sender, EventArgs e)
    {
        Call.ComputerApi.ToggleBootMenu(Computer.Id, chkEnabled.Checked);     
    }

    protected void ddlProxyMode_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateForm();
    }
}