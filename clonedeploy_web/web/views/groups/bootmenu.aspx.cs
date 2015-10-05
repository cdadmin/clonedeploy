using System;
using System.IO;
using BasePages;
using Helpers;
using Models;
using Pxe;
using Group = BLL.Group;
using GroupMembership = BLL.GroupMembership;

namespace views.groups
{
    public partial class GroupBootMenu : Groups
    {
        private readonly Group _bllGroup = new Group();
        private readonly GroupMembership _bllGroupMembership = new GroupMembership();
        protected void btnRemoveBootMenu_Click(object sender, EventArgs e)
        {
          

            switch (Group.Type)
            {
                case "standard":
                    foreach (var host in _bllGroupMembership.GetGroupMembers(Group.Id, ""))
                    {
                        var customBootMenu = new CustomBootMenu {Host = host};
                        customBootMenu.RemoveCustomBootMenu();
                    }
                    break;
               
            }

            var historyg = new History
            {
                Event = "Remove Boot Menu",
                Type = "Group",
                TypeId = Group.Id.ToString()
            };
            historyg.CreateEvent();
        }

        protected void btnSetBootMenu_Click(object sender, EventArgs e)
        {
          

            switch (Group.Type)
            {
                case "standard":
                    foreach (var host in _bllGroupMembership.GetGroupMembers(Group.Id, ""))
                    {                    
                        var customBootMenu = new CustomBootMenu
                        {
                            Host = host,
                            FileName = txtCustomBootMenu.Text
                        };
                        customBootMenu.SetCustomBootMenu();
                    }
                    break;
               
            }

            var historyg = new History
            {
                Event = "Set Boot Menu",
                Type = "Group",
                TypeId = Group.Id.ToString()
            };
            historyg.CreateEvent();
        }

        protected void ddlTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            var mode = Settings.PxeMode;

            switch (ddlTemplate.Text)
            {
                case "default":
                    string path;
                    if (mode.Contains("ipxe"))
                        path = Settings.TftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                               "default.ipxe";
                    else
                        path = Settings.TftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                               "default";

                    try
                    {
                        txtCustomBootMenu.Text = File.ReadAllText(path);
                    }
                    catch (Exception ex)
                    {
                        Message.Text = "Could Not Read Default Boot Menu.  Check The Exception Log For More Info";
                        Logger.Log(ex.Message);
                    }
                    break;
                case "select template":
                    txtCustomBootMenu.Text = "";
                    break;
                default:
                    var template = new BootTemplate {Name = ddlTemplate.Text};
                    template.Read();
                    txtCustomBootMenu.Text = template.Content;
                    break;
            }
        }

        protected void DisplayCustomBootMenu()
        {
            ddlTemplate.DataSource = new BootTemplate().ListAll();
            ddlTemplate.DataBind();
            ddlTemplate.Items.Insert(0, "select template");
            ddlTemplate.Items.Insert(1, "default");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
           
       
            if (!IsPostBack) DisplayCustomBootMenu();
        }
    }
}