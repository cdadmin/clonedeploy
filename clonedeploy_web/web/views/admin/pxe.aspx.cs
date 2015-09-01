using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Global;
using Models;

public partial class views_admin_pxe : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
       
        ddlPXEMode.SelectedValue = Settings.PxeMode;
        ddlProxyDHCP.SelectedValue = Settings.ProxyDhcp;
        ddlProxyBios.SelectedValue = Settings.ProxyBiosFile;
        ddlProxyEfi32.SelectedValue = Settings.ProxyEfi32File;
        ddlProxyEfi64.SelectedValue = Settings.ProxyEfi64File;
       ShowProxyMode();
    }

    protected void btnUpdateSettings_OnClick(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    protected void ProxyDhcp_Changed(object sender, EventArgs e)
    {
        ShowProxyMode();
    }

    protected void ShowProxyMode()
    {
        ddlProxyBios.BackColor = Color.White;
        ddlProxyEfi32.BackColor = Color.White;
        ddlProxyEfi64.BackColor = Color.White;
        ddlProxyBios.Font.Strikeout = false;
        ddlProxyEfi32.Font.Strikeout = false;
        ddlProxyEfi64.Font.Strikeout = false;
        ddlPXEMode.BackColor = Color.White;
        ddlPXEMode.Font.Strikeout = false;
        if (ddlProxyDHCP.Text == "No")
        {
            ddlProxyBios.BackColor = Color.LightGray;
            ddlProxyEfi32.BackColor = Color.LightGray;
            ddlProxyEfi64.BackColor = Color.LightGray;
            ddlProxyBios.Font.Strikeout = true;
            ddlProxyEfi32.Font.Strikeout = true;
            ddlProxyEfi64.Font.Strikeout = true;
        }
        else
        {
            ddlPXEMode.BackColor = Color.LightGray;
            ddlPXEMode.Font.Strikeout = true;
        }
    }
}