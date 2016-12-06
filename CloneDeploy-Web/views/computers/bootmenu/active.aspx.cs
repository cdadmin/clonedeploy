using System;

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
        string path;

        if (proxyDhcp == "Yes")
            divProxy.Visible = true;

        if (active)
        {
            path = proxyDhcp == "Yes"
                ? BLL.ComputerBootMenu.GetComputerProxyPath(Computer, true, ddlProxyMode.Text)
                : BLL.ComputerBootMenu.GetComputerNonProxyPath(Computer, true);
            lblActiveBoot.Text = "Active Task Found <br> Displaying Task Boot Menu";
        }
        else
        {
            if (Convert.ToBoolean(Computer.CustomBootEnabled))
            {
                path = proxyDhcp == "Yes"
                    ? BLL.ComputerBootMenu.GetComputerProxyPath(Computer, true, ddlProxyMode.Text)
                    : BLL.ComputerBootMenu.GetComputerNonProxyPath(Computer, true);

                lblActiveBoot.Text =
                    "No Active Task Found <br> Custom Boot Menu Found <br> Displaying Custom Boot Menu";
            }

            else //Not Active, display default global boot menu
            {
                path = proxyDhcp == "Yes"
                    ? BLL.ComputerBootMenu.GetComputerProxyPath(Computer, false, ddlProxyMode.Text)
                    : BLL.ComputerBootMenu.GetComputerNonProxyPath(Computer, false);

                lblActiveBoot.Text =
                    "No Active Task Found <br> No Custom Boot Menu Found <br> Displaying Global Default Boot Menu";
            }
        }

        lblFileName.Text = path;
        scriptEditor.Value = new FileOps().ReadAllText(path);
    }
}