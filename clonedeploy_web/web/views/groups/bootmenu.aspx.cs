using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Global;
using Models;
using Pxe;

namespace views.groups
{
    public partial class GroupBootMenu : Page
    {
        protected void btnRemoveBootMenu_Click(object sender, EventArgs e)
        {
            var group = new Group {Id = Convert.ToInt32(Request.QueryString["groupid"])};
            group.Read();

            switch (@group.Type)
            {
                case "standard":
                    foreach (var host in @group.GroupMembers())
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
                TypeId = @group.Id.ToString()
            };
            historyg.CreateEvent();

            Master.Master.Msgbox(Utility.Message);
        }

        protected void btnSetBootMenu_Click(object sender, EventArgs e)
        {
            var group = new Group {Id = Convert.ToInt32(Request.QueryString["groupid"])};
            group.Read();

            switch (@group.Type)
            {
                case "standard":
                    foreach (var host in @group.GroupMembers())
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
                TypeId = @group.Id.ToString()
            };
            historyg.CreateEvent();

            Master.Master.Msgbox(Utility.Message);
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
                        Master.Master.Msgbox("Could Not Read Default Boot Menu.  Check The Exception Log For More Info");
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
            var group = new Group {Id = Convert.ToInt32(Request.QueryString["groupid"])};
            group.Read();
            var subTitle = Master.Master.FindControl("SubNav").FindControl("labelSubTitle") as Label;
            if (subTitle != null) subTitle.Text = group.Name + " | Boot Menu";
            if (!IsPostBack) DisplayCustomBootMenu();
        }
    }
}