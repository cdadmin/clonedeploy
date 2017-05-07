using System;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

namespace views.groups
{
    public partial class GroupBootMenu : Groups
    {
        protected void buttonUpdate_OnClick(object sender, EventArgs e)
        {
            RequiresAuthorizationOrManagedGroup(Authorizations.UpdateGroup, Group.Id);
            var bootMenu = Call.GroupApi.GetCustomBootMenu(Group.Id) ?? new GroupBootMenuEntity();
            bootMenu.GroupId = Group.Id;
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
            EndUserMessage = Call.GroupBootMenuApi.Post(bootMenu).Success
                ? "Successfully Updated Custom Boot Menu"
                : "Could Not Update Custom Boot Menu";
        }

        protected void chkDefault_OnCheckedChanged(object sender, EventArgs e)
        {
            var group = Group;
            group.SetDefaultBootMenu = Convert.ToInt16(chkDefault.Checked);
            Call.GroupApi.Put(group.Id, group);
        }

        protected void ddlProxyMode_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateForm();
        }

        protected void ddlTemplates_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            scriptEditor.Value = "";
            if (ddlTemplates.SelectedValue == "-1") return;
            scriptEditor.Value = Call.BootTemplateApi.Get(Convert.ToInt32(ddlTemplates.SelectedValue)).Contents;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) PopulateForm();
        }

        protected void PopulateForm()
        {
            chkDefault.Checked = Group.SetDefaultBootMenu == 1;
            PopulateBootTemplatesDdl(ddlTemplates);
            var bootMenu = Call.GroupApi.GetCustomBootMenu(Group.Id);

            if (Settings.ProxyDhcp == "Yes")
            {
                divProxy.Visible = true;
                if (bootMenu == null) return;
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
                if (bootMenu == null) return;
                scriptEditor.Value = bootMenu.BiosMenu;
            }
        }
    }
}