using System;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

public partial class views_computers_bootmenu_active : Computers
{
    protected void DisplayActiveMenu()
    {
        var proxyDhcp = Settings.ProxyDhcp;
        var active = Call.ComputerApi.IsComputerActive(Computer.Id);
        string path;

        if (proxyDhcp == "Yes")
            divProxy.Visible = true;

        if (active)
        {
            path = proxyDhcp == "Yes"
                ? Call.ComputerApi.GetProxyPath(Computer.Id, true, ddlProxyMode.Text)
                : Call.ComputerApi.GetNonProxyPath(Computer.Id, true);
            lblActiveBoot.Text = "Active Task Found <br> Displaying Task Boot Menu";
        }
        else
        {
            if (Convert.ToBoolean(Computer.CustomBootEnabled))
            {
                path = proxyDhcp == "Yes"
                    ? Call.ComputerApi.GetProxyPath(Computer.Id, true, ddlProxyMode.Text)
                    : Call.ComputerApi.GetNonProxyPath(Computer.Id, true);

                lblActiveBoot.Text =
                    "No Active Task Found <br> Custom Boot Menu Found <br> Displaying Custom Boot Menu";
            }

            else //Not Active, display default global boot menu
            {
                path = proxyDhcp == "Yes"
                    ? Call.ComputerApi.GetProxyPath(Computer.Id, false, ddlProxyMode.Text)
                    : Call.ComputerApi.GetNonProxyPath(Computer.Id, false);

                lblActiveBoot.Text =
                    "No Active Task Found <br> No Custom Boot Menu Found <br> Displaying Global Default Boot Menu";
            }
        }

        lblFileName.Text = path;
        scriptEditor.Value = Call.FilesystemApi.ReadFileText(path);
    }

    protected void EditProxy_Changed(object sender, EventArgs e)
    {
        DisplayActiveMenu();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) DisplayActiveMenu();
    }
}