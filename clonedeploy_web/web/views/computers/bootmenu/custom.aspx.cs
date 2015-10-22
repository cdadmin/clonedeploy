using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Helpers;
using Models;

public partial class views_computers_bootmenu_custom : BasePages.Computers
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack) PopulateForm();
    }

    protected void PopulateForm()
    {
        chkEnabled.Checked = Computer.CustomBootEnabled == 1;
        PopulateBootTemplatesDdl(ddlTemplates);
        var bootMenu = BLL.ComputerBootMenu.GetComputerBootMenu(Computer.Id);
        

        if (Helpers.Settings.ProxyDhcp == "Yes")
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
        scriptEditor.Value = BLL.BootTemplate.GetBootTemplate(Convert.ToInt32(ddlTemplates.SelectedValue)).Contents;
    }

    protected void buttonUpdate_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.UpdateComputer);
        var bootMenu = BLL.ComputerBootMenu.GetComputerBootMenu(Computer.Id) ?? new ComputerBootMenu();
        bootMenu.ComputerId = Computer.Id;
        if (Helpers.Settings.ProxyDhcp == "Yes")
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
        EndUserMessage = BLL.ComputerBootMenu.UpdateComputerBootMenu(bootMenu)
            ? "Successfully Updated Custom Boot Menu"
            : "Could Not Update Custom Boot Menu";

        if(chkEnabled.Checked)
            BLL.ComputerBootMenu.CreateBootFiles(Computer);
    }

    protected void chkEnabled_OnCheckedChanged(object sender, EventArgs e)
    {
        BLL.ComputerBootMenu.ToggleComputerBootMenu(Computer, chkEnabled.Checked);
        
    }

    protected void ddlProxyMode_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateForm();
    }
}