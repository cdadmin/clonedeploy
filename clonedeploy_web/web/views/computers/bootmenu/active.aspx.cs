using System;
using Helpers;
using Pxe;

public partial class views_computers_bootmenu_active : BasePages.Computers
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) DisplayActiveMenu();
    }

    protected void EditProxy_Changed(object sender, EventArgs e)
    {
        DisplayActiveMenu();
    }

    protected void DisplayActiveMenu()
    {
        var proxyDhcp = Settings.ProxyDhcp;
        var active = BLL.ActiveImagingTask.IsComputerActive(Computer.Id);
        var pxeFileOps = new PxeFileOps();
        string path;

        if (proxyDhcp == "Yes")
            divProxy.Visible = true;

        if (active)
        {
            path = proxyDhcp == "Yes"
                ? pxeFileOps.GetHostProxyPath(Computer, true, ddlProxyMode.Text)
                : pxeFileOps.GetHostNonProxyPath(Computer, true);
            lblActiveBoot.Text = "Active Task Found <br> Displaying Task Boot Menu";
        }
        else
        {
            if (Convert.ToBoolean(Convert.ToInt16(Computer.CustomBootEnabled)))
            {
                path = proxyDhcp == "Yes"
                    ? pxeFileOps.GetHostProxyPath(Computer, true, ddlProxyMode.Text)
                    : pxeFileOps.GetHostNonProxyPath(Computer, true);

                lblActiveBoot.Text =
                    "No Active Task Found <br> Custom Boot Menu Found <br> Displaying Custom Boot Menu";
            }

            else //Not Active, display default global boot menu
            {
                path = proxyDhcp == "Yes"
                    ? pxeFileOps.GetHostProxyPath(Computer, false, ddlProxyMode.Text)
                    : pxeFileOps.GetHostNonProxyPath(Computer, false);

                lblActiveBoot.Text =
                    "No Active Task Found <br> No Custom Boot Menu Found <br> Displaying Global Default Boot Menu";
            }
        }

        lblFileName.Text = path;
        txtBootMenu.Text = new FileOps().ReadAllText(path);
    }
}